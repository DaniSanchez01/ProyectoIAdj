using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Clase que representa una heuristica y que nos permite obtener los vecinos de una serie de celdas y tambien nos permite saber el valor heuristico de una celda a otra.
 */
public interface Heuristica
{
    /*
     * Dada una celda nos da la lista de vecinos correspondiente a esa celda segun la heuristica
     */
    public List<Vector2Int> vecinos(Vector2Int celda);

    /*
     * Dada 2 celdas obtiene la distancia que sera claculada segun la heuristica
     */
    public float coste(Vector2Int celdaOrigen, Vector2Int celdaDestino);
}
