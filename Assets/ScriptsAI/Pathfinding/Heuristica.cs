using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Clase que representa una heuristica y que nos permite obtener los vecinos de una serie de celdas y tambien nos permite saber el valor heuristico de una celda a otra.
 */
public interface Heuristica
{
    /*
     * Dada una celda obtiene las celdas generadas hasta cierta profundidad usando la distancia que represente la clase que implemente esta interfaz, garantizando
     * que las celdas son validas y que se puede llegar desde una a las otras, es decir que forman una componente conexa.
     */
    public List<Vector2Int> espacioLocal(Vector2Int celda,int prof,int filas,int cols,Nodo [,] nodos);
    public List<Vector2Int> espacioLocal(Vector2Int celda,int prof,int filas,int cols,TypeTerrain [,] celdas);

    /*
     * Dada 2 celdas obtiene la distancia que sera claculada segun la heuristica
     */
    public float coste(Vector2Int celdaOrigen, Vector2Int celdaDestino);
}
