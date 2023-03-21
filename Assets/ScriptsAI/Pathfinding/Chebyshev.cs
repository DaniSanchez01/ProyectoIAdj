using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Esta clase representa la distancia de chebyshev donde se tienen 8 vecinos y la heuristica que se obtiene tomando las coordenadas de ambos puntos restando sus componentes x e y y quedandonos
 * con el valor absoulto mas grande
 */
public class Chebychev : Heuristica
{
    /*
     * En este caso se obtienen 8 vecinos. Como chebychev se expande tambien en diagonales realizar una expansion usando busqueda en anchura equivale a crear un cuadrado donde
     * el centro es la casilla origen. Y el numero de filas y columnas
     */
    public List<Vector2Int> espacioLocal(Vector2Int celda,int prof)
    {
        List<Vector2Int> celdas = new List<Vector2Int>();

        for(int i=-prof;i<=prof;i++)
        {
            for(int j=-prof;j<prof;j++)
            {
                celdas.Add(new Vector2Int(celda.x + i, celda.y + j));
            }
        }
        return celdas;

    }

    /*
     * En este caso se queda con la maxima diferencia quue hay en la distancia entre las 2 celdas en el eje x o y.
     */
    public float coste(Vector2Int celdaOrigen, Vector2Int celdaDestino)
    {
        return Mathf.Max(Mathf.Abs(celdaDestino.x - celdaOrigen.x), Mathf.Abs(celdaDestino.y - celdaOrigen.y));
    }
}
