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
            // Shift+F
            else if (Input.GetKeyDown(KeyCode.F)) {
                    GameObject.FindObjectOfType<FormationManager>().formar();       
            }
        {
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
                        virt.Position = newTarget;
                        int nUnits = selectedUnits.Count;
                        float angleDistance = 360f;
                        if (nUnits >0) {
                            angleDistance = 360.0f/nUnits;
                        }
                        float angle = 0f;
                        float distance = 0.3f*nUnits;
                        AgentNPC[] agents = getSelectedUnitsAgents();
        

                        foreach (var npc in agents)
                        {
                            //npc.SendMessage("NewTarget", newTarget);
                            // Nota: Estructura jerárquica -> BroadcastMessage.
                            Vector3 newPosition = npc.OrientationToVector(angle)*distance+newTarget;
                            float newOrientation = Bodi.MapToRange(angle+180,Range.grados180);
                            Agent newVirt = Agent.CreateStaticVirtual(newPosition,ori: newOrientation,paint:false);
                            if (!npc.TryGetComponent<Arrive>(out Arrive a)) {
                                a = npc.gameObject.AddComponent<Arrive>();
                            }
                            a.NewTarget(newVirt);
                            if (!npc.TryGetComponent<Align>(out Align b)) {
                                b = npc.gameObject.AddComponent<Align>();
                            }
                            b.NewTarget(newVirt);
                            angle = Bodi.MapToRange(angle+angleDistance,Range.grados180);

                        }
                    }
                    
                }
            }
        }
    }
}
