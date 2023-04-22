using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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

    public bool sameNode(Nodo a, Nodo b) {
        return (a.Celda.x == b.Celda.x) && (a.Celda.y == b.Celda.y);
    }
    public bool sameNode(Nodo a, int x, int y) {
        return (a.Celda.x == x) && (a.Celda.y == y);
    }

    public bool contiene(List<Nodo> lista, Nodo a) {
        foreach (var n in lista) {
            if (sameNode(a,n)) {
                return true;
            }
        }
        return false;
    }
    //Algoritmo completo
    public void LRTA() {
        if (sameNode(posicion,objetivo)) {
            agente.agentState = State.Formation;
            GameObject.FindObjectOfType<FormationManager>().notifyEndLRTA(agente);
        }
        else {
            grid.setValoresHeuristicos(objetivo);
            //Mientras no llegamos al objetivo
            int count = 0;
            bool success = true;
            while (posicion!=objetivo) {
                //Calcular el espacio local desde donde nos encontramos
                searchLocalSpace();
                //Actualizar los vostes heuristicos del espacio local
                success = updateValues();
                if (!success) {
                    break;
                }
                //Seleccionar el mejor camino hasta salir del espacio local
                actionSelection();
                count+=1;
                if (count>=400) {
                    break;
                }
            }
            if (count>=400 || !success) {
            agente.agentState = State.Formation;
            GameObject.FindObjectOfType<FormationManager>().notifyEndLRTA(agente);
        }
            //Preparar al npc para seguir el camino calculado
            executeAction();
        }  
    }

    public void A() {
        Nodo[,] nodes = grid.CeldasGrid;

        //La lista abierta tendrá prioridad(siempre devolverá el elemento cuyo coste sea menor)
        ColaPrioridad openList = new ColaPrioridad();
        List<Nodo> closedList = new List<Nodo>();
        openList.Enqueue(posicion);
        //Establece g = Infinito para todo nodo
        for (int i=0;i<grid.Filas;i++) {
            for (int j=0;j<grid.Columnas;j++) {
                nodes[i,j].g = Mathf.Infinity; 
            }
        }

        //El coste de llegar del nodo inicial a si mismo es 0
        posicion.g = 0;
        posicion.Parent = posicion;
        //Mientras queden nodos por estudiar
        while (openList.Count > 0) {
            //Coger el nodo con mejor coste total
            Nodo nodoActual = openList.Dequeue();
            //Debug.LogFormat("{0},{1}: Padre = {2},{3}",nodoActual.Celda.x,nodoActual.Celda.y,nodoActual.Parent.Celda.x,nodoActual.Parent.Celda.y);
            closedList.Add(nodoActual);

            //Si este nodo es el objetivo
            if (sameNode(nodoActual,objetivo)) {
                //Reconstruir el camino de vuelta al nodo inicial
                nodePath = creaCamino(posicion,objetivo);
                break;
            }
            //Devuelve los vecinos cuyas celdas son válidas
            List<Nodo> vecinos = grid.getVecinosValidosProf(nodoActual,1);
            //Para cada vecino disponible
            foreach (Nodo vecino in vecinos) {
                // Si el vecino ya ha sido explorado, ignorarlo
                if (contiene(closedList,vecino)) {
                    continue;
                }
                //Calculo el posible nuevo conste de g
                float possibleG = nodoActual.g + coste(nodoActual,vecino);
                //Si el nuevo coste para llegar a este nodo es menor del que había
                if (possibleG < vecino.g) {
                    //Actualiza los valores del nodo
                    vecino.g = possibleG;
                    vecino.Parent = nodoActual;
                    //Si todavia no se havia encontrado este nodo
                    if (!openList.contiene(vecino)) {
                        //Calcula la heurística para llegar al objetivo
                        vecino.h = grid.HeuristicaGrid.coste(vecino.Celda,objetivo.Celda);
                        //Anadelo a la lista de nodos por explorar
                        openList.Enqueue(vecino);
                    }
                    else {
                        //Actualiza el orden de la lista de nodos por explorar
                        openList.Update(vecino);
                    }
                }
            }
        }
        if (nodePath.Count!=0) {
            executeAction();
        }
    }

    //Coste entre un nodo y otro
    public float coste(Nodo a, Nodo b) {
        float c = (agente.getTerrainCost(a) + agente.getTerrainCost(b)) / 2;
        return c;
    }

    //Una vez encontrado el nodo final, reconstruye el camino
    public List<Nodo> creaCamino(Nodo inicio, Nodo fin) {
        List<Nodo> camino = new List<Nodo>();
        //Añade el destino al camino
        camino.Add(fin);
        Nodo anterior= fin.Parent;
        //Para cada nodo que hemos añadido al camino, añadimos también a su padre, hasta que encontramos el nodo inicial
        while (!sameNode(inicio,anterior)) {
            camino.Add(anterior);
            anterior = anterior.Parent;
        }
        //Le damos la vuelta al camino
        camino.Reverse();
        TerrainMap t = GameObject.FindObjectOfType<TerrainMap>();
        /*foreach (var n in camino) {
            Debug.LogFormat("{0},{1} Terreno: {2}",n.Celda.x,n.Celda.y,t.getTerrenoCasilla(n.Celda.x,n.Celda.y));

        }*/
        return camino;
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
    public bool updateValues()
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
            if (minCoste == infinito) {
                return false;
            }

            //Debug.Log("("+minN.Celda.x+","+minN.Celda.y+")= "+minCoste);

        }
        return true;
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
