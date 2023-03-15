using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool giz = true;

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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                //Si da en algo
                if (Physics.Raycast(ray, out hitInfo))
                {
                    AgentNPC a;
                    //Si es un NPC
                    if (hitInfo.transform.gameObject.TryGetComponent<AgentNPC>(out a))
                    {
                        //Si este NPC todavia no se encuentra en la lista
                        if (!selectedUnits.Contains(hitInfo.transform.gameObject))
                        {
                            //Añadirlos a la lista
                            selectedUnits.Add(hitInfo.transform.gameObject);
                            hitInfo.transform.gameObject.GetComponent<Cubo>().enable();
                        }
                    }
                }
            }
        }
        //Si estamos pulsando el espacio
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //Si hay una formación activa, descativarla
            GameObject.FindObjectOfType<FormationManager>().acabarFormacion();

            //Hacer desaparecer los cubos y borrar la lista
            foreach (var npc in selectedUnits)
            {
                npc.GetComponent<Cubo>().enable();
                AgentNPC n = npc.GetComponent<AgentNPC>();
                if (npc.TryGetComponent<Arrive>(out Arrive a)) {
                    npc.GetComponent<Arrive>().NewTarget(n);
                }
                
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
                    if (allInFormation(getSelectedUnitsAgents())) {
                        GameObject.FindObjectOfType<FormationManager>().moveToPoint(newTarget);
                    }
                    //Si no, mover cada npc de manera individual al punto
                    else {
                        Agent target = virt;
                        target.Position = newTarget;
                        target.giz = this.giz;

                        foreach (var npc in selectedUnits)
                        {
                            // Llama al método denominado "NewTarget" en TODOS y cada uno de los MonoBehaviour de este game object (npc)
                            //npc.SendMessage("NewTarget", newTarget);
                            
                            // Nota: Estructura jerárquica -> BroadcastMessage.
                            // Se asume que cada NPC tiene varias componentes scripts (es decir, varios MonoBehaviour).
                            // En algunos de esos scripts está la función "NewTarget(Vector3 target)"
                            // Dicha función contendrá las instrucciones necesarias para ir o no al nuevo destino.
                            // P.e. Dejar lo que esté haciendo y  disparar a target.
                            // P.e. Si no tengo vida suficiente huir de target.
                            // P.e. Si fui seleccionado en una acción anterio y estoy a la espera de nuevas órdenes, entonces hacer un Arrive a target.


                            Arrive a;
                            if (!npc.TryGetComponent<Arrive>(out a)) {
                                a = npc.AddComponent<Arrive>();
                            }
                            a.NewTarget(target); 
                        }
                    }
                    
                }
            }
        }
    }
}
