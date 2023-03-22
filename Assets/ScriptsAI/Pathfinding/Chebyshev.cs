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
    public List<Vector2Int> espacioLocal(Vector2Int celdaO,int prof,int filas,int cols,Nodo [,] nodosgrid)
    {
        List<Vector2Int> celdasExpandir = new List<Vector2Int>(); //representan las celdas que aun se tienen que obtener sus vecinos
        List<Vector2Int> celdasGeneradas = new List<Vector2Int>(); //representan las celdas que ya han sido generadas y por tanto ya no se tratan

        celdasExpandir.Add(celdaO);

        while (celdasExpandir.Count != 0)
        {
            Vector2Int celdaActual = celdasExpandir[0]; //se obtiene la 1 celda 
            celdasExpandir.RemoveAt(0); //se elimina la celda de la lista

            //hay que comprobar que la celda sea valida
            bool valida = (0 <= celdaActual.x && celdaActual.x < filas) && (0 <= celdaActual.y && celdaActual.y < cols) && nodosgrid[celdaActual.x, celdaActual.y].Transitable;

            //Para poder obtener los vecinos de una celda se debe cumplir que esta no este a una profundidad igual o mayor que el origen y tiene que ser valida esto es que sea transitable
            //y este dentro del grid
            if (valida && coste(celdaO, celdaActual) < prof)
            {
                float difx = celdaActual.x - celdaO.x;
                float dify = celdaActual.y - celdaO.y;

                if (celdaActual.Equals(celdaO)) //Si estamos en la celda que es el origen del espacio local se generan las primeras celdas en las 8 direcciones
                {
                    celdasExpandir.Add(new Vector2Int(celdaActual.x - 1, celdaActual.y));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x + 1, celdaActual.y));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x, celdaActual.y + 1));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x, celdaActual.y - 1));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x + 1, celdaActual.y - 1));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x - 1, celdaActual.y - 1));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x + 1, celdaActual.y + 1));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x - 1, celdaActual.y + 1));
                }


                //si estamos en una celda que esta en diagonal respecto al origen lo que podemos saber porque las distancias en el eje x e y son iguales respecto al origen
                //de hecho estas celdas serian las esquinas del cuadrados
                else if (Mathf.Abs(difx) == Mathf.Abs(dify))
                {

                    celdasExpandir.Add(new Vector2Int(celdaActual.x + ((int)(1 * Mathf.Sign(difx))), celdaActual.y));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x, celdaActual.y + ((int)(1 * Mathf.Sign(dify)))));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x + ((int)(1 * Mathf.Sign(difx))), celdaActual.y + ((int)(1 * Mathf.Sign(dify)))));
                }

                //a partir de este punto si la celda no es el origen ni es una esquina tiene que estar en algun lado ya sea el de arriba,abajo,izquierda o derecha. Esto lo podemos
                //saber analizando las compoenentes x e y y viendo cual es mayor en valor absoluto.

                //si esta  a la drecha o izquierda se expande una en ese sentido
                else if (Mathf.Abs(difx) > Mathf.Abs(dify)) celdasExpandir.Add(new Vector2Int(celdaActual.x + ((int)(1 * Mathf.Sign(difx))), celdaActual.y));

                //en otro caso sabemos que puede estar en el lado de arriba o de abajo y habra que expandir una casilla en ese sentido es decir hacia arriba o abajo
                else celdasExpandir.Add(new Vector2Int(celdaActual.x, celdaActual.y + ((int)(1 * Mathf.Sign(dify)))));

            }


            if (valida) celdasGeneradas.Add(celdaActual); //se añade la celda SOLO si fue valida, lo ponemos aqui y no en el anterior if porque las celdas con prof = profDeseada son validas
            //pero no se van a expandir

        }

        return celdasGeneradas;

    }

    /*
     * En este caso se queda con la maxima diferencia quue hay en la distancia entre las 2 celdas en el eje x o y.
     */
    public float coste(Vector2Int celdaOrigen, Vector2Int celdaDestino)
    {
        return Mathf.Max(Mathf.Abs(celdaDestino.x - celdaOrigen.x), Mathf.Abs(celdaDestino.y - celdaOrigen.y));
    }
}
