using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Esta clase representa la distancia de Manhattan que es un tipo de heuristica y en este caso los vecinos representan las 4 direcciones a las que se puede ir desde la casilla actual
 */
public class Manhattan : Heuristica
{
    /*
     *  Metodo que devuelve los veciones en las 4 direcciones acorde a Manhattan
     */
    public List<Vector2Int> vecinos(Vector2Int celda)
    {
        List<Vector2Int> vecinosCelda = new List<Vector2Int>();
        vecinosCelda.Add(new Vector2Int(celda.x + 1, celda.y));
        vecinosCelda.Add(new Vector2Int(celda.x, celda.y + 1));
        vecinosCelda.Add(new Vector2Int(celda.x - 1, celda.y));
        vecinosCelda.Add(new Vector2Int(celda.x, celda.y -1));
        return vecinosCelda;
    }

    //Para manhattan la heuristica del coste entre 2 celdas son la suma de los  movimientos en el eje x e en el eje y que se deben realizar con el fin de llegar a la celda objetivo 
    public float coste(Vector2Int celdaOrigen, Vector2Int celdaDestino)
    {
        return Mathf.Abs(celdaOrigen.x - celdaDestino.y) + Mathf.Abs(celdaOrigen.y - celdaDestino.y);
    } 
}
