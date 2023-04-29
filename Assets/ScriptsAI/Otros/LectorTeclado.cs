using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * L. Daniel Hernández. 2018. Copyleft
 * 
 * Una propuesta para dar órdenes a un grupo de agentes sin formación.
 * 
 * Recursos:
 * Los rayos de Cámara: https://docs.unity3d.com/es/current/Manual/CameraRays.html
 * "Percepción" mediante Physics.Raycast: https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
 * SendMessage to external functions: https://www.youtube.com/watch?v=4j-lh3C_w1Q
 * 
 * */

public class LectorTeclado : MonoBehaviour
{
    List<GameObject> selectedUnits = new List<GameObject>();
    private Agent virt;

    public GameObject textoEsquina;
    public MapasTacticos scriptMapas;
    public bool giz = true;
    private bool depuracion = false;
    private bool guerraTotal = false;
    private bool defensiveRed = true;
    private bool defensiveBlue = true;
    private bool tactico = true;

    private typeMap mapa = typeMap.Ninguno;
    private string changeToString(bool a){
        if (a) return "Activado";
        else return "Desactivado";
    }

    void Start() {
        virt = Agent.CreateStaticVirtual(Vector3.zero,paint:false);
    }

    public AgentNPC[] getSelectedUnitsAgents() {
        AgentNPC[] agents = new AgentNPC[selectedUnits.Count];
        int i = 0;
        foreach (var unit in selectedUnits) {
            agents[i] = unit.GetComponent<AgentNPC>();
            i+=1;
        }
        return agents;
    }

    public bool allInFormation( AgentNPC[] listAgents) {
        if (listAgents.Length == 0) return false;
        bool answer = true;
        foreach (var agent in listAgents) {
            if (agent.agentState != State.Formation) {
                if (agent.agentState != State.leaderFollowing) {
                    answer = false;
                }
            }
        }
        return answer;
    }

    // Update is called once per frame
    void Update()
    {
        //Si mantenemos pulsado el shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //Al pulsar con el raton
            if (Input.GetMouseButtonUp(0))
            {
                if (selectedUnits.Count==0) {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    //Si da en algo
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        AgentNPC a;
                        //Si es un NPC
                        if (hitInfo.transform.gameObject.TryGetComponent<AgentNPC>(out a))
                        {
                            selectedUnits.Add(hitInfo.transform.gameObject);
                            hitInfo.transform.gameObject.GetComponent<Cubo>().enable();
                        }
                        
                    }
                }

            }
            // Shift+F
            /*else if (Input.GetKeyDown(KeyCode.F)) {
                    GameObject.FindObjectOfType<FormationManager>().formar();       
            }*/
        }
        //Si estamos pulsando el espacio
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //Si hay una formación activa, descativarla
            //GameObject.FindObjectOfType<FormationManager>().acabarFormacion();

            //Hacer desaparecer los cubos y borrar la lista
            foreach (var npc in selectedUnits)
            {
                npc.GetComponent<Cubo>().enable();
                AgentNPC n = npc.GetComponent<AgentNPC>();
                n.changeArbitro(typeArbitro.Quieto);
            }
            selectedUnits.Clear();
        }
        // Damos una orden cuando levantemos el botón del ratón.
        else if (Input.GetMouseButtonUp(0))
        {
            // Comprobamos si el ratón golpea a algo en el escenario.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                // Si lo que golpea es un punto del terreno entonces da la orden a todas las unidades NPC
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Floor"))
                {
                    Vector3 newTarget = hitInfo.point;
                    //Si todos los npcs seleccionados pertenecer a una formación, moverse en formación
                    /*if (allInFormation(getSelectedUnitsAgents())) {
                        GameObject.FindObjectOfType<FormationManager>().moveToPoint(newTarget);
                    }*/
                    //Si no, mover cada npc de manera individual al punto
                    if (selectedUnits.Count!=0) {
                        AgentNPC[] agents = getSelectedUnitsAgents();
                        virt.Position = newTarget;
                        foreach (var npc in agents)
                        {
                            
                            npc.agentState = State.runningToPoint;
                            npc.TryGetComponent<GridPathFinding>(out GridPathFinding grid);
                            //Calcula en que celda se encuentra el npc
                            Vector2Int celda = grid.getCeldaDePuntoPlano(npc.Position);
                            Nodo posicion = grid.GetNodo(celda.x,celda.y);
                            //Calcula en que celda se encuentra el objetivo
                            Vector2Int celdaObjetivo = grid.getCeldaDePuntoPlano(newTarget);
                            Nodo obj = grid.GetNodo(celdaObjetivo.x,celdaObjetivo.y);
                            //Crea el algoritmo de PathFinding
                            PathFinding algorithm= new PathFinding(grid,posicion,obj, npc, 1, false);
                            algorithm.A();
                            //angle = Bodi.MapToRange(angle+angleDistance,Range.grados180);

                        }
                    }
                        
                    
                    
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject.FindObjectOfType<makePathfinding>().prepareLRTA();
        
        }
        else if (Input.GetKeyDown(KeyCode.B)){
            GameObject[] agents = GameObject.FindGameObjectsWithTag("NPCazul");
                if (defensiveBlue) {
                    defensiveBlue = false;
                    foreach (var npc in agents) {
                        npc.GetComponent<AgentNPC>().changeToOffensive();
                    }
                }
                else {
                    defensiveBlue = true;
                    foreach (var npc in agents) {
                        npc.GetComponent<AgentNPC>().changeToDefensive();
                    }
                }

        }
        else if (Input.GetKeyDown(KeyCode.R)){
            GameObject[] agents = GameObject.FindGameObjectsWithTag("NPCrojo");
                if (defensiveRed) {
                    defensiveRed = false;
                    foreach (var npc in agents) {
                        npc.GetComponent<AgentNPC>().changeToOffensive();
                    }
                }
                else {
                    defensiveRed = true;
                    foreach (var npc in agents) {
                        npc.GetComponent<AgentNPC>().changeToDefensive();
                    }
                }

        }
        else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.V)) {
            if (Input.GetKeyDown(KeyCode.Z)) {
                depuracion = !depuracion;
                GameObject[] agents = GameObject.FindGameObjectsWithTag("NPCrojo");
                foreach (var npc in agents) {
                    npc.GetComponent<AgentNPC>().changeDepuration();
                }
                agents = GameObject.FindGameObjectsWithTag("NPCazul");
                foreach (var npc in agents) {
                    npc.GetComponent<AgentNPC>().changeDepuration();
                }

            }
            else if (Input.GetKeyDown(KeyCode.C)) {
                scriptMapas.nextMap();
                mapa = scriptMapas.TipoMapa;
            }
            else if (Input.GetKeyDown(KeyCode.V)) {
                tactico = !tactico;
                GameObject[] agents = GameObject.FindGameObjectsWithTag("NPCrojo");
                foreach (var npc in agents) {
                    if (npc.TryGetComponent<GridPathFinding>(out GridPathFinding a)){
                        a.Tactics = tactico;
                    }
                }
                agents = GameObject.FindGameObjectsWithTag("NPCazul");
                foreach (var npc in agents) {
                    if (npc.TryGetComponent<GridPathFinding>(out GridPathFinding a)){
                        a.Tactics = tactico;
                    }
                }
            }
            else guerraTotal = !guerraTotal;
            TMP_Text t = textoEsquina.GetComponent<TMP_Text>();
            t.text = "Modo Depuracion: "+changeToString(depuracion)+"\nGuerra Total: "+changeToString(guerraTotal)+"\nMapa activo: "+mapa+"\nPathFinding Tactico: "+changeToString(tactico);


        }
        
        
    }
    
}
