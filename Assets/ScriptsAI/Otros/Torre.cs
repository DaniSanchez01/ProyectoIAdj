using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Torre: MonoBehaviour
{

    protected int vida = 30;
    protected TMP_Text contador;
    public GameObject textoVictoria;
    public Team team;
    public Agent virtualAgent;
    // Start is called before the first frame update
    public void Start()
    {
        contador = transform.Find("ContadorVida").GetComponent<TMP_Text>();
        virtualAgent = Agent.CreateStaticVirtual(this.gameObject.transform.position);
    }


    public void recibirDamage(int x) {
        vida = vida-x;
        if (vida<0) vida=0;
        contador.text = "Vida: "+vida;
        if (vida == 0) finishGame();
    }

    public void finishGame() {
        AgentNPC[] listaNPCs = FindObjectsOfType<AgentNPC>();
        foreach (var npc in listaNPCs) {
            npc.Inmovil = true;
        }
        textoVictoria.SetActive(true);
        gameObject.SetActive(false);
        
    }
}
