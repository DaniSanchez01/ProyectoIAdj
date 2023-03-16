using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FormationManager : MonoBehaviour
{
    // Tamaño de una celda del grid
    public float cellSize = 2.0f;

    // Grid usado
    public GridFormation grid;
    
    //Diseño de formación usado
    private Pattern pattern;
    //Lider de la formación
    private AgentNPC leader;


    //Función que se realiza al pulsar SHIFT+F. Pone en formación a todos los personajes seleccionados
    public void formar()
    {
        // Cogemos todos los agentes de la escena seleccionados
        AgentNPC[] allAgents = GameObject.FindObjectOfType<LectorTeclado>().getSelectedUnitsAgents();
        
        //Si todavia no se ha creado el grid, crearlo y prepararlo para ser usado
        if (grid==null) {
            leader = allAgents[0];
            //AQUI ELEGIMOS LA FORMACIÓN QUE QUEREMOS USAR
            pattern = new AttackPattern();
            //Celda que le corresponde en la formación específica
            (int,int) leaderSlot = pattern.getLeaderSlot();
            //Orientación que le corresponde en la formación específica
            float leaderAngle = pattern.getAngle(0);
            //Creamos y preparamos el grid
            grid = gameObject.AddComponent<GridFormation>();
            grid.CreateGridManager(cellSize, leader, leaderSlot.Item1, leaderSlot.Item2,leaderAngle,4,4);
            //Ponemos el lider en estado de formación
            leader.agentState = State.Formation;
        }
        //Mover el grid a la posición del lider si no está ahi
        if (!grid.activated) {
            grid.moveGrid(leader.Position);
            grid.activated = true;
        } 
        int i = 1;
        //Añade cada agente al grid (menos el lider que ya ha sido añadido)
        foreach (var agent in allAgents) {
            if ((agent != leader) && (pattern.supportAgent(i))){
                //Celda que le corresponde en la formación específica
                (int,int) cell = pattern.getSlot(i);
                //Orientación que le corresponde en la formación específica
                float angle = pattern.getAngle(i);
                //Conectar el npc a la celda correspondiente
                grid.linkToSlot(cell.Item1,cell.Item2,angle,agent);
                //Pasar a estado de formación
                agent.agentState = State.Formation;
                i++;
            }
        }
        //Posicionar a todos los agentes
        grid.leaderFollowing();
    }

    //Si hay una formación activa y se pulsa SPACE, desactivarla
    public void acabarFormacion() {
        if (grid!=null) {
            grid.liberarAgents();
        }
    }

    //Notifica al grid cuando el lider ha llegado al grid (en leaderFollowing)
    public void notifyLeaderArrival() {
        grid.agentsToCell();
    }

    //Mover el grid y hacer que los personajes vayan a el (Ocurre cuando los personajes están en formación y se pincha en un nuevo lugar)
    public void moveToPoint(Vector3 point) {
        grid.moveGrid(point);
        grid.leaderFollowing();
    }
    // se detecta si se ha pulsado Shift+F
    private void Update(){
    
    }
}
