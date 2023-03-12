using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MatrixAgentFormation : MonoBehaviour
{
    public float cellSize = 2.0f; // Espacio entre agentes en la matriz // public float spacing = 2.0f;
    public Agent leader; 
    // la matriz de agentes se encuentra en grid
    public FormationGridManager grid;
    // public Agent[,] agents; // Matriz de agentes de la formación // no necesario, tenemos el grid para eso
    public Arrive arrive;
    private Agent target;
    



    public void formar()
    {
        // Cogemos todos los agentes de la escena
        // Esto habría que cambiarlo a solo coger los que seleccionemos
        AgentNPC[] allAgents = GameObject.FindObjectOfType<SeguirPunto>().getSelectedUnitsAgents();

        // Ordena los agentes en función de su distancia al lider de menor a mayor
        Array.Sort(allAgents, (a1, a2) => {
            float dist1 = Vector3.Distance(a1.Position, leader.Position);
            float dist2 = Vector3.Distance(a2.Position, leader.Position);
            return dist1.CompareTo(dist2);
        });
        // Creamos un FormationGridManager
        grid = new FormationGridManager(cellSize, leader, allAgents);
        // Crea matriz de agentes
        // agents = new Agent[width, height];

        // Coloca los agentes en la matriz
        for (int i = 0; i < grid.numColumns; i++) 
        {
            for (int j = 0; j < grid.numRows; j++)
            {
                // Calcula la posición del agente en la formación
                Vector3 pos = leader.Position + new Vector3(i * cellSize, 0, j * cellSize);

                // Mueve el agente a su posición en la formación
                // agents[i, j].Position = pos; // Directamente

                // Con un Arrive
                Agent target = Agent.CreateStaticVirtual(pos);
                Arrive a;
                if (!grid.slots[i, j].npc.TryGetComponent<Arrive>(out a)) {
                    a = grid.slots[i, j].npc.gameObject.AddComponent<Arrive>();
                }
                a.NewTarget(target);                
            }
        }
        
        // El leader se "quita" de los agentes para que no le afecte la formacion
        //leader.GetComponent<Agent>().enabled = false;
    }


    // se detecta si se ha pulsado Shift+F
    private void Update()
    {
        // Shift+F
        if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftShift))
        {
            // Busca la clase MatrixAgentFormation en la escena
            MatrixAgentFormation formation = FindObjectOfType<MatrixAgentFormation>();

            // Si se encuentra la clase, llama al metodo Init()
            if (formation != null)
            {
                formation.formar();
            }
        }
    }
    private void LateUpdate()
    {
        // Cogemos todos los agentes
        Agent[] allAgents = GameObject.FindObjectsOfType<AgentNPC>();

        // Actualizamos la posición de los agentes en la formación
        // Util en caso de que el lider se mueva durante la formacion
        foreach (Agent agent in allAgents)
        {
            Vector3 offset = agent.Position - leader.Position;
            //offset = leader.Rotation * offset; //así no funciona
            offset = leader.transform.rotation * offset;
            agent.Position = leader.Position + offset;
            //agent.Rotation = leader.Rotation; //así no funciona
            agent.transform.rotation = leader.transform.rotation;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(leader.Position, 0.5f);

        Gizmos.color = Color.green;
        Agent[] allAgents = GameObject.FindObjectsOfType<AgentNPC>();
        if (allAgents != null)
        {
            foreach (Agent agent in allAgents)
            {
                Gizmos.DrawSphere(agent.Position, 0.3f);
            }
        }
    }

}
