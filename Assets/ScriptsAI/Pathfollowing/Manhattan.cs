using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Esta clase representa la distancia de Manhattan que es un tipo de heuristica y en este caso los vecinos representan las 4 direcciones a las que se puede ir desde la casilla actual
 */
public class Manhattan : Heuristica
{
    /*
     *  Metodo que devuelve las celdas generadas a partir de una celda origen y hasta una determinada profundidad. La implementación permite no tratar celdas repetidas.
     *  Por tanto permite generar un subespacio local
     *  Pre: prof >=0
     */
    public List<Vector2Int> espacioLocal(Vector2Int celdaO,int prof)
    {
        List<Vector2Int> celdasExpandir = new List<Vector2Int>(); //representan las celdas que aun se tienen que obtener sus vecinos
        List<Vector2Int> celdasGeneradas = new List<Vector2Int>(); //representan las celdas que ya han sido generadas y por tanto ya no se tratan

        celdasExpandir.Add(celdaO);

        while(celdasExpandir.Count == 0)
        {
            Vector2Int celdaActual = celdasExpandir[0]; //se obtiene la 1º celda 
            celdasExpandir.RemoveAt(0); //se elimina la celda de la lista

            //Para poder obtener los vecinos de una celda se debe cumplir que esta no este a una profundidad igual o mayor que el origen
            if(coste(celdaO,celdaActual) < prof) 
            {

                if (celdaActual.Equals(celdaO)) //Si estamos en la celda que es el origen del espacio local se generan las primeras celdas en la 4 direcciones
                {
                    celdasExpandir.Add(new Vector2Int(celdaActual.x - 1, celdaActual.y));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x + 1, celdaActual.y));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x, celdaActual.y + 1));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x, celdaActual.y - 1));
                }

                //si la celda esta en la misma columna que la celda origen, entonces esta generara la de su izquierda, derecha y la siguiente en su eje y
                else if (celdaActual.x == celdaO.x)
                { 

                    celdasExpandir.Add(new Vector2Int(celdaActual.x, celdaActual.y + (int)(1 * Mathf.Sign(celdaActual.y))));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x + 1, celdaActual.y));
                    celdasExpandir.Add(new Vector2Int(celdaActual.x - 1, celdaActual.y));
                }

                //en otro caso ni es la celda origen ni esta en la misma columna
                else celdasExpandir.Add(new Vector2Int(celdaActual.x + (int)(1 * Mathf.Sign(celdaActual.x)), celdaActual.y));

            }


            celdasGeneradas.Add(celdaActual); //siempres se añade la celda ya se haya podido obtener sus vecinos o no

        }

        return celdasGeneradas;
    }

    //Para manhattan la heuristica del coste entre 2 celdas son la suma de los  movimientos en el eje x e en el eje y que se deben realizar con el fin de llegar a la celda objetivo 
    public float coste(Vector2Int celdaOrigen, Vector2Int celdaDestino)
    {
        return Mathf.Abs(celdaOrigen.x - celdaDestino.y) + Mathf.Abs(celdaOrigen.y - celdaDestino.y);
    } 
}
