using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Estados en los que se encuentra el pathfinding (no se si hará falta)
public enum PathFindingState {
    NonUsed,
    Start,
    Walking,
}

public class PathFinding
{
    
    //Representación del infinito
    int infinito = 9999999;
    //Grid que se está usando
    GridPathFinding grid;
    //Nodo al que debemos llegar
    Nodo objetivo;
    //Nodo en el que nos encontramos
    Nodo posicion;
    //Agente que debe realizar el PathFinding
    AgentNPC agente;
    //Profundidad que usaremos
    int prof;
    //Lista que guardara el camino a seguir
    List<Nodo> nodePath = new List<Nodo>();
    //Lista que guarda el espacio local de cada momento
    List<Nodo> localSpace = new List<Nodo>();
    public bool giz;

    
    public PathFinding(GridPathFinding grid, Nodo posicion, Nodo objetivo, AgentNPC npc, int prof, bool giz) {
        this.grid = grid;
        this.objetivo = objetivo;
        this.posicion = posicion;
        this.agente = npc;
        this.prof = prof;
        this.giz = giz;
    }

    //Algoritmo completo
    public void LRTA() {
        if ((posicion.Celda.x != objetivo.Celda.x) && (posicion.Celda.y != objetivo.Celda.y)) {
            grid.setValoresHeuristicos(objetivo);
            //Mientras no llegamos al objetivo
            while (posicion!=objetivo) {
                //Calcular el espacio local desde donde nos encontramos
                searchLocalSpace();
                /*Debug.Log(localSpace.Count);
                foreach (var nodo in localSpace) {
                    Debug.Log(nodo.Celda.x +" "+nodo.Celda.y);
                }*/
                //Actualizar los vostes heuristicos del espacio local
                updateValues();
                //Seleccionar el mejor camino hasta salir del espacio local
                actionSelection();
            }
            //Preparar al npc para seguir el camino calculado
            executeAction();
        }
        
        else {
            agente.agentState = State.Formation;
            GameObject.FindObjectOfType<FormationManager>().notifyEndLRTA(agente);
        }
        
    }

    //Calcular el espacio local desde donde nos encontramos
    public void searchLocalSpace()
    {
        objetivo.Transitable = false;
        //Crea una lista con todos los nodos validos a una profundidad x de la posición donde estamos
        localSpace = grid.getVecinosValidosProf(posicion,prof);
        objetivo.Transitable = true;
    }

    //Comprueba si algun nodo del espacio local sigue sin determinar su coste heurístico
    public bool existsInfinite(List<Nodo> nodos) {
        foreach (var nodo in nodos) {
            if (nodo.CosteHeuristica == infinito) {
                return true;
            }
        }
        return false;
    }

    //Calcular el coste heurístico minimo entre una lista de vecinos
    public float calculateMinimum(List<Nodo> vecinos) {
        if (vecinos.Count == 0) {} /*¿Que pasaria si no tiene nigun vecino transitable?*/
        float min = infinito;
        //Para cada vecino
        foreach (var nodo in vecinos) {
            //Sumamos el coste heuristico que tiene con el peso de avanzar hacia el
            float valor = nodo.Weight + nodo.CosteHeuristica;
            //Si es un mejor valor que el de los nodos calculados anteriormente guardarlos
            if (valor < min) min = valor;
        }
        return min;

        
    }

    //Actualizar los vostes heuristicos del espacio local
    public void updateValues()
    {
        //Asignar infinito como coste heuristico de cada nodo del espacio local
        foreach (var nodo in localSpace) {
            nodo.TempH = nodo.CosteHeuristica;
            nodo.CosteHeuristica = infinito;
        }
        //Mientras no se haya terminado de determinar el coste heurístico de cada uno de los nodos continuar
        while (existsInfinite(localSpace)) {
            float minCoste = infinito;
            Nodo minN = null;
            //Para cada nodo que todavia no ha determinado su coste heurístico
            foreach (var nodo in localSpace) {
                if (nodo.CosteHeuristica==infinito){
                    //Crea una lista de sus vecinos
                    List<Nodo> vecinos = grid.getVecinosValidosProf(nodo,1);
                    //Calcula el coste heuristico minimo de avanzar a uno de sus vecinos
                    float minimum = calculateMinimum(vecinos);
                    //Cogemos el maximo entre este dato y el coste heurístico que teniamos antes
                    minimum = Mathf.Max(minimum, nodo.TempH);
                    //Si el coste resultante es mejor que el de los demás nodos del espacio local guardarlo
                    if (minimum <= minCoste) {
                        minCoste = minimum;
                        minN = nodo;
                    }
                }
            }
            //Ya calculados todos los costes, determinamos el del nodo con el coste más pequeño (Solo se determina 1 por iteración)
            minN.CosteHeuristica = minCoste;
            //Debug.Log("("+minN.Celda.x+","+minN.Celda.y+")= "+minCoste);

        }
    }

    //Devuelve el nodo con el coste heurístico mas bajo
    public Nodo cheapestNode(List<Nodo> nodos) {
        Nodo answer = null;
        float min = infinito;
        foreach (var nodo in nodos) {
            if (nodo.CosteHeuristica < min) {
                min = nodo.CosteHeuristica;
                answer = nodo;
            }
        }
        return answer;
    }

    public bool esContenido(Nodo n) {
        foreach (var nodo in localSpace){
            if ((nodo.Celda.x == n.Celda.x) && (nodo.Celda.y == n.Celda.y)) return true;
        }
        return false;
    }
    //Seleccionar el mejor camino hasta salir del espacio local
    public void actionSelection() {
        //Mientras no no salgamos del espacio local
        while(esContenido(posicion)) {
            //Actualizar la posición a la del vecino más barato
            List<Nodo> vecinos = grid.getVecinosValidosProf(posicion,1);
            posicion = cheapestNode(vecinos);
            //Añadir al camino la nueva posicion
            nodePath.Add(posicion);

        }
    }

    //Preparar al npc para seguir el camino calculado
    public void executeAction() {
        List<Vector3> path = new List<Vector3>();
        //Convertimos los nodos en los puntos del mundo real en los que se encuentran
        foreach(var nodo in nodePath){
            Vector3 point = grid.getPuntoPlanoDeCelda(nodo.Celda.x,nodo.Celda.y);
            path.Add(point);
        }
        //Cambiamos el comportamiento del npc para que siga un camino
        agente.changeArbitro(typeArbitro.RecorreCamino);
        //Establecemos el camino calculado
        PathFollowingNoOffset a = (PathFollowingNoOffset) agente.takeSteering("PathFollowing");
        a.setPath(path);
        a.mode = 0;
        a.intRadius = 2f;
        a.giz = giz;
        Face b = (Face) agente.takeSteering("Face");
        b.path = a;

    }

}
