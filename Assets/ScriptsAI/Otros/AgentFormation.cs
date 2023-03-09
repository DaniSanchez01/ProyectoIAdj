using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentFormation : MonoBehaviour
{
    public int width = 3; // Ancho de la matriz
    public int height = 3; // Altura de la matriz
    public float spacing = 2.0f; // Espacio entre agentes en la matriz
    public Agent leader; 
    public Agent[,] agents; // Matriz de agentes de la formación
    Arrive arrive;
    public int countAgents;


    public void formar()
    {
        // Cogemos todos los agentes de la escena
        // Esto habría que cambiarlo a solo coger los que seleccionemos
        AgentNPC[] allAgents = GameObject.FindObjectOfType<SeguirPunto>().getSelectedUnitsAgents();
        countAgents = allAgents.Length;

        // Ordena los agentes en función de su distancia al lider de menor a mayor
        Array.Sort(allAgents, (a1, a2) => {
            float dist1 = Vector3.Distance(a1.Position, leader.Position);
            float dist2 = Vector3.Distance(a2.Position, leader.Position);
            return dist1.CompareTo(dist2);
        });

        // Crea matriz de agentes
        agents = new Agent[width, height];

        // Coloca los agentes en la matriz
        int index = 0;
        for (int i = 0; i < width && index < allAgents.Length; i++)
        {
            for (int j = 0; j < height && index < allAgents.Length; j++)
            {
                Debug.Log(j); 
                // Sería necesario comprobar que haya tantos agentes como casillas en el grid?
                //if (index >= allAgents.Length)
                //{
                //    Debug.LogError("No hay suficientes agentes en la escena para llenar la formación");
                //    return;
                //}

                // Mete los agentes a la matriz
                agents[i, j] = allAgents[index];

                // Calcula la posición del agente en la formación
                Vector3 pos = leader.Position + new Vector3(i * spacing, 0, j * spacing);

                // Mueve el agente a su posición en la formación
                //agents[i, j].Position = pos; // Directamente

                // Con un Arrive
                Agent target = Agent.CreateStaticVirtual(pos);
                Arrive a;
                if (!agents[i, j].TryGetComponent<Arrive>(out a)) {
                    a = agents[i, j].gameObject.AddComponent<Arrive>();
                }
                a.NewTarget(target);                
                index++;
            }
        }
        
        // El leader se "quita" de los agentes para que no le afecte la formacion
        leader.GetComponent<Agent>().enabled = false;
    }


    // se detecta si se ha pulsado Shift+F
    private void Update()
    {
        // Shift+F
        if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftShift))
        {
            // Busca la clase AgentFormation en la escena
            AgentFormation formation = FindObjectOfType<AgentFormation>();

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
        if (agents != null)
        {
            foreach (Agent agent in allAgents)
            {
                Gizmos.DrawSphere(agent.Position, 0.3f);
            }
        }
    }

}
