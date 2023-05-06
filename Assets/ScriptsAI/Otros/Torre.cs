using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Torre: MonoBehaviour
{

    protected int vida = 600;
    protected TMP_Text contador;
    public GameObject textoVictoria;
    public Team team;
    public Agent virtualAgent;
    TerrainMap mapa;
    bool puedoPedirAyuda = true;
    bool guerraTotal = false;

    public List<AgentNPC> ejercito;
    // Start is called before the first frame update
    public void Start()
    {
        mapa = FindObjectOfType<TerrainMap>();
        contador = transform.Find("ContadorVida").GetComponent<TMP_Text>();
        UpdateContador();
        virtualAgent = Agent.CreateStaticVirtual(this.gameObject.transform.position);
        AgentNPC[] listaNPCs = FindObjectsOfType<AgentNPC>();
        foreach (var npc in listaNPCs) {
            if (npc.team == this.team) {
                ejercito.Add(npc);
            }
        }
        StartCoroutine(disponibilidadAyuda());
    }

    public void UpdateContador() {
        contador.text = "Vida: "+vida;
    }

    public void recibirDamage(int x) {
        //Comprobamos si el ejercito se encuentra en guerra Total
        guerraTotal = ejercito[0].GuerraTotal;
        //Si se puede pedir ayuda y no se está en guerra total, se llama a los 2 npcs más cercanos
        if (puedoPedirAyuda && !guerraTotal) pedirAyuda();
        //Restamos la vida
        vida = vida-x;
        if (vida<0) vida=0;
        contador.text = "Vida: "+vida;
        //Si la torre ha caido, terminamos el juego
        if (vida == 0) finishGame();
    }

    public void finishGame() {
        AgentNPC[] listaNPCs = FindObjectsOfType<AgentNPC>();
        foreach (var npc in listaNPCs) {
            npc.Inmovil = true;
            npc.BaseDamage = 0;
        }
        textoVictoria.SetActive(true);
        gameObject.SetActive(false);
        
    }

    //Cuando pegan a la base por primera vez avisa a los dos npcs más cercanos
    public void pedirAyuda() {
        puedoPedirAyuda = false;
        //Ordena los npcs según su distancia a la base
        ComparadorPorDistancia comparer = new ComparadorPorDistancia();
        comparer.torre = this;
        ejercito.Sort(comparer);
        //Hace que los 2 más cercanos vayan a defender la torre
        for (int i=0;i<2;i++) {
            AgentNPC npc = ejercito[i];
            // if (team == Team.Red) {
            //     Debug.Log(npc.gameObject.name);
            // }
            npc.changeToDefensive();
            npc.salir(npc.agentState);
            npc.finalidadPathFinding = typeRecorrerCamino.aDefender;
            if (team == Team.Red) {
                npc.PuntoInteres = mapa.waypointBaseRojo[0];
            }
            else npc.PuntoInteres = mapa.waypointBaseAzul[0];
            npc.entrar(State.RecorriendoCamino);
        }
    }

    IEnumerator disponibilidadAyuda() {
        while(true) {
            puedoPedirAyuda = true;
            yield return new WaitForSeconds(15f);
        }
    }
}
class ComparadorPorDistancia : IComparer<AgentNPC> {
    public Torre torre;
    public int Compare(AgentNPC a, AgentNPC b) {
        int distA = 99999999;
        int distB = 99999999;
        if (!(a.agentState == State.Curandose || a.agentState == State.Curandose || a.agentState == State.Muerto)){
            distA =  (int) Vector3.Distance(torre.transform.position, a.Position)*100;
        }
        if (!(b.agentState == State.Curandose || b.agentState == State.Curandose || b.agentState == State.Muerto)){
            distB =  (int) Vector3.Distance(torre.transform.position, b.Position)*100;
        }
        return distA.CompareTo(distB);
    }
}
