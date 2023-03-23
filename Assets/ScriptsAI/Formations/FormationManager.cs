using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum typePattern {
        Ataque,
        Defensa
}

public enum criterio {
    leaderFollowing,
    pathfinding
}

public class FormationManager : MonoBehaviour
{
    // Tamaño de una celda del grid
    public float cellSize = 2.0f;

    // Grid usado
    public GridFormation grid;
    
    //Diseño de formación usado
    private Pattern pattern;
    public typePattern tipoFormacion;
    
    public criterio criterio;
    public typeHeuristica heuristica;
    public int profundidad = 1;
    public int filasPathFinding=19;
    public int columnasPathFinding=19;
    
    public float cellSizePathFinding = 3;
    public bool gizPathFinding = false;

    //Lider de la formación
    private AgentNPC leader;
    private int inicio;
    private bool waiting = false;
    private bool doingWander = false;

    //Función que se realiza al pulsar SHIFT+F. Pone en formación a todos los personajes seleccionados
    public void formar()
    {
        // Cogemos todos los agentes de la escena seleccionados
        AgentNPC[] allAgents = GameObject.FindObjectOfType<LectorTeclado>().getSelectedUnitsAgents();
        
        //Si todavia no se ha creado el grid, crearlo y prepararlo para ser usado
        if (grid==null) {
            leader = allAgents[0];
            //AQUI ELEGIMOS LA FORMACIÓN QUE QUEREMOS USAR
            createPattern();
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
        if (criterio==criterio.leaderFollowing) grid.leaderFollowing();
        else grid.pathfinding(heuristica, profundidad, filasPathFinding,columnasPathFinding,cellSizePathFinding,gizPathFinding);
    }

    public void createPattern() {
        if (tipoFormacion == typePattern.Ataque) pattern = new AttackPattern();
        else pattern = new DefensivePattern();
    }
    //Si hay una formación activa y se pulsa SPACE, desactivarla
    public void acabarFormacion() {
        if (grid!=null) {
            NoWait();
            grid.liberarAgents();
        }
    }

    //Notifica al grid cuando el lider ha llegado al grid (en leaderFollowing)
    public void notifyLeaderArrival() {
        grid.agentsToCell();
    }

    public void notifyEndLRTA(AgentNPC npc) {
        grid.LRTAtoCell(npc);
    }

    //Mover el grid y hacer que los personajes vayan a el (Ocurre cuando los personajes están en formación y se pincha en un nuevo lugar)
    public void moveToPoint(Vector3 point) {
        grid.moveGrid(point);
        if (criterio==criterio.leaderFollowing) grid.leaderFollowing();
        else grid.pathfinding(heuristica,profundidad,filasPathFinding,columnasPathFinding,cellSizePathFinding,gizPathFinding);
    }
    // se detecta si se ha pulsado Shift+F

    public void startTimer() {
        inicio = Environment.TickCount;
        waiting = true;
    }

    public void NoWait() {
        waiting = false;
    }

    public void Wait() {
        waiting = true;
    }

    public void finishTimer(){
        if (waiting) {
            if ((Environment.TickCount - inicio) > 10000){
                if (doingWander) {
                    formar();
                    doingWander = false;
                }
                else {
                    grid.leaderWander();
                    doingWander = true;
                }
                
            }
        }
    }
    private void Update(){
        finishTimer();
    }

    public void disactivateGrid() {
        if (grid!=null) {
            grid.activated = false;
        }
    }
}
