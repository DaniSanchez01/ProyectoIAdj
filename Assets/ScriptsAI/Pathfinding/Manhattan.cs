using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Esta clase representa la distancia de Manhattan que es un tipo de heuristica y en este caso los vecinos representan las 4 direcciones a las que se puede ir desde la casilla actual
 */
public class Manhattan : Heuristica
{
    /*
     *  Metodo que devuelve las celdas generadas a partir de una celda origen y hasta una determinada profundidad. La implementaci�n permite no tratar celdas repetidas.
     *  Por tanto permite generar un subespacio local
     *  Pre: prof >=0
     */
    public List<Vector2Int> espacioLocal(Vector2Int celdaO,int prof,int filas,int cols,Nodo [,] nodosgrid)
    {
        List<Vector2Int> celdasExpandir = new List<Vector2Int>(); //representan las celdas que aun se tienen que obtener sus vecinos
        List<Vector2Int> celdasUsadas = new List<Vector2Int>(); //representan las celdas que aun se tienen que obtener sus vecinos
        List<Vector2Int> celdasGeneradas = new List<Vector2Int>(); //representan las celdas que ya han sido generadas y por tanto ya no se tratan

        celdasExpandir.Add(celdaO);
        celdasUsadas.Add(celdaO);

        while(celdasExpandir.Count != 0)
        {
            Vector2Int celdaActual = celdasExpandir[0]; //se obtiene la 1� celda 
            celdasExpandir.Remove(celdaActual); //se elimina la celda de la lista

            //hay que comprobar que la celda sea valida
            bool valida = (0 <= celdaActual.x && celdaActual.x < filas) && (0 <= celdaActual.y && celdaActual.y < cols) && nodosgrid[celdaActual.x, celdaActual.y].Transitable;
            //Para poder obtener los vecinos de una celda se debe cumplir que esta no este a una profundidad igual o mayor que el origen y tiene que ser valida esto es que sea transitable
            //y este dentro del grid
            if (valida && coste(celdaO,celdaActual) < prof) 
            {

                Vector2Int izq =new Vector2Int(celdaActual.x - 1, celdaActual.y);
                Vector2Int der =new Vector2Int(celdaActual.x + 1, celdaActual.y);
                Vector2Int up =new Vector2Int(celdaActual.x , celdaActual.y+ 1);
                Vector2Int under =new Vector2Int(celdaActual.x, celdaActual.y-1);

                if (!celdasUsadas.Contains(izq)) 
                {
                    celdasUsadas.Add(izq);
                    celdasExpandir.Add(izq);
                }
                if (!celdasUsadas.Contains(der)){
                    celdasUsadas.Add(der);
                    celdasExpandir.Add(der);
                }
                if (!celdasUsadas.Contains(up)){
                    celdasUsadas.Add(up);
                    celdasExpandir.Add(up);
                }
                if (!celdasUsadas.Contains(under)){
                    celdasUsadas.Add(under);
                    celdasExpandir.Add(under);
                }
            }
            if (valida) celdasGeneradas.Add(celdaActual);
        }
        
        //Debug.Log(celdasGeneradas.Count);
        return celdasGeneradas;
    }

    public List<Vector2Int> espacioLocal(Vector2Int celdaO,int prof,int filas,int cols,TypeTerrain [,] celdas)
    {
        List<Vector2Int> celdasExpandir = new List<Vector2Int>(); //representan las celdas que aun se tienen que obtener sus vecinos
        List<Vector2Int> celdasUsadas = new List<Vector2Int>(); //representan las celdas que aun se tienen que obtener sus vecinos
        List<Vector2Int> celdasGeneradas = new List<Vector2Int>(); //representan las celdas que ya han sido generadas y por tanto ya no se tratan

        celdasExpandir.Add(celdaO);
        celdasUsadas.Add(celdaO);

        while(celdasExpandir.Count != 0)
        {
            Vector2Int celdaActual = celdasExpandir[0]; //se obtiene la 1� celda 
            celdasExpandir.Remove(celdaActual); //se elimina la celda de la lista

            //hay que comprobar que la celda sea valida
            bool valida = (0 <= celdaActual.x && celdaActual.x < filas) && (0 <= celdaActual.y && celdaActual.y < cols) && celdas[celdaActual.x, celdaActual.y]!=TypeTerrain.invalido;
            //Para poder obtener los vecinos de una celda se debe cumplir que esta no este a una profundidad igual o mayor que el origen y tiene que ser valida esto es que sea transitable
            //y este dentro del grid
            if (valida && coste(celdaO,celdaActual) < prof) 
            {

                Vector2Int izq =new Vector2Int(celdaActual.x - 1, celdaActual.y);
                Vector2Int der =new Vector2Int(celdaActual.x + 1, celdaActual.y);
                Vector2Int up =new Vector2Int(celdaActual.x , celdaActual.y+ 1);
                Vector2Int under =new Vector2Int(celdaActual.x, celdaActual.y-1);

                if (!celdasUsadas.Contains(izq)) 
                {
                    celdasUsadas.Add(izq);
                    celdasExpandir.Add(izq);
                }
                if (!celdasUsadas.Contains(der)){
                    celdasUsadas.Add(der);
                    celdasExpandir.Add(der);
                }
                if (!celdasUsadas.Contains(up)){
                    celdasUsadas.Add(up);
                    celdasExpandir.Add(up);
                }
                if (!celdasUsadas.Contains(under)){
                    celdasUsadas.Add(under);
                    celdasExpandir.Add(under);
                }
            }
            if (valida) celdasGeneradas.Add(celdaActual);
        }
        
        //Debug.Log(celdasGeneradas.Count);
        return celdasGeneradas;
    }

    //Para manhattan la heuristica del coste entre 2 celdas son la suma de los  movimientos en el eje x e en el eje y que se deben realizar con el fin de llegar a la celda objetivo 
    public float coste(Vector2Int celdaOrigen, Vector2Int celdaDestino)
    {
        return Mathf.Abs(celdaOrigen.x - celdaDestino.x) + Mathf.Abs(celdaOrigen.y - celdaDestino.y);
    } 
}
