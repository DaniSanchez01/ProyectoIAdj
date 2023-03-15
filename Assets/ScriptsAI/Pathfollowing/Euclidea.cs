using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Euclidea : Heuristica
{

    public List<Vector2Int> vecinos(Vector2Int celda)
    {
        List<Vector2Int> vecinosCelda = new List<Vector2Int>();
        vecinosCelda.Add(new Vector2Int(celda.x + 1, celda.y));
        vecinosCelda.Add(new Vector2Int(celda.x, celda.y + 1));
        vecinosCelda.Add(new Vector2Int(celda.x - 1, celda.y));
        vecinosCelda.Add(new Vector2Int(celda.x, celda.y - 1));


        vecinosCelda.Add(new Vector2Int(celda.x + 1, celda.y + 1));
        vecinosCelda.Add(new Vector2Int(celda.x - 1, celda.y - 1));
        vecinosCelda.Add(new Vector2Int(celda.x + 1, celda.y - 1));
        vecinosCelda.Add(new Vector2Int(celda.x - 1, celda.y + 1));

        return vecinosCelda;
    }

  
    public float coste(Vector2Int celdaOrigen, Vector2Int celdaDestino)
    {
        return Mathf.Sqrt(Mathf.Pow(celdaDestino.x - celdaOrigen.x, 2) + Mathf.Pow(celdaDestino.y - celdaOrigen.y, 2));
    }



}
