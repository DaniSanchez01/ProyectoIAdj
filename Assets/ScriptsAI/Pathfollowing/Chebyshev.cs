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
     * En este caso se obtienen 8 vecinos
     */
    public List<Vector2Int> espacioLocal(Vector2Int celda)
    {
        List<Vector2Int> vecinosCelda = new List<Vector2Int>();
        vecinosCelda.Add(new Vector2Int(celda.x + 1, celda.y));
        vecinosCelda.Add(new Vector2Int(celda.x, celda.y + 1));
        vecinosCelda.Add(new Vector2Int(celda.x - 1, celda.y));
        vecinosCelda.Add(new Vector2Int(celda.x, celda.y - 1));


        vecinosCelda.Add(new Vector2Int(celda.x +1 , celda.y + 1));
        vecinosCelda.Add(new Vector2Int(celda.x - 1, celda.y -1));
        vecinosCelda.Add(new Vector2Int(celda.x +1 , celda.y - 1));
        vecinosCelda.Add(new Vector2Int(celda.x - 1, celda.y + 1));

        return vecinosCelda;
    }

    /*
     * En este caso se queda con la maxima diferencia quue hay en la distancia entre las 2 celdas en el eje x o y.
     */
    public float coste(Vector2Int celdaOrigen, Vector2Int celdaDestino)
    {
        return Mathf.Max(Mathf.Abs(celdaDestino.x - celdaOrigen.x), Mathf.Abs(celdaDestino.y - celdaOrigen.y));
    }
}
