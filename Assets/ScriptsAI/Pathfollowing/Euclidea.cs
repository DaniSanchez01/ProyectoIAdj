using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Euclidea : Heuristica
{

    public List<Vector2Int> espacioLocal(Vector2Int celda,int prof)
    {
        List<Vector2Int> celdas = new List<Vector2Int>();
        

        for (int i = -prof; i <= prof; i++)
        {
            for (int j = -prof; j < prof; j++)
            {
                celdas.Add(new Vector2Int(celda.x + i, celda.y + j));
            }
        }
        return celdas;
    }

  
    public float coste(Vector2Int celdaOrigen, Vector2Int celdaDestino)
    {
        return Mathf.Sqrt(Mathf.Pow(celdaDestino.x - celdaOrigen.x, 2) + Mathf.Pow(celdaDestino.y - celdaOrigen.y, 2));
    }



}
