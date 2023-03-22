using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class makePathfinding: MonoBehaviour
{
    public AgentNPC npc;
    public int filaObjetivo;
    public int columnaObjetivo;
    public int prof;
    public typeHeuristica heur;

    public void prepareLRTA() {


        GridPathFinding grid = npc.gameObject.AddComponent<GridPathFinding>();
        grid.inicializarGrid(20,20,3,heur,true);
        Vector2Int celda = grid.getCeldaDePuntoPlano(npc.Position);
        Nodo posicion = grid.GetNodo(celda.x,celda.y);
        Nodo objetivo = grid.GetNodo(filaObjetivo,columnaObjetivo);
        PathFinding algorithm= new PathFinding(grid,posicion,objetivo, npc, prof);
        algorithm.LRTA();
    }
}
