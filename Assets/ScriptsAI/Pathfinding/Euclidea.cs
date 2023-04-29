using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Euclidea : Heuristica
{

    public List<Vector2Int> espacioLocal(Vector2Int celdaO, int prof, int filas, int cols, Nodo[,] nodosgrid)
    {
        HashSet<Vector2Int> cjtoCerrados = new HashSet<Vector2Int>(); //son nodos que ya han sido tratados para expandir con exito o sin exito pero que estan dentro siempre del espacio local
        List<Vector2Int> celdasExpandir = new List<Vector2Int>(); //representan las pendientes de expandir aun no tratadas
        List<Vector2Int> celdasGeneradas = new List<Vector2Int>(); //representan las celdas que ya han sido generadas y que eran validas y que forman parte del espacio local

        //1. Se empieza a�adiendo el origen
        celdasExpandir.Add(celdaO);

        //2.Mientras aun hayan celdas por expandir
        while (celdasExpandir.Count != 0)
        {
            //2.1 se obtiene la 1� celda y se elimina de las celdas por expandir
            Vector2Int celdaActual = celdasExpandir[0];
            celdasExpandir.RemoveAt(0);



            //2.2 Hay que comprobar si esta celda ya se expandio pues si es asi ya se trato y ya generara hijos o no no tiene sentido tratarla de nuevo
            if (!cjtoCerrados.Contains(celdaActual))

            {
                //2.2.1 hay que comprobar que esa celda sea valida 
                bool valida = (0 <= celdaActual.x && celdaActual.x < filas) && (0 <= celdaActual.y && celdaActual.y < cols) && nodosgrid[celdaActual.x, celdaActual.y].Transitable;

                //2.2.1 si la celda es valida y se puede expandir se expande
                if (valida && coste(celdaO, celdaActual) < prof)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (i == 0 && j == 0) continue;
                            celdasExpandir.Add(new Vector2Int(celdaActual.x + i, celdaActual.y + j));
                        }
                    }
                }

                //2.2.2 se a�ade si es valida ya se haya expandido o no porque este en el limite de la frontera o no.
                if (valida) celdasGeneradas.Add(celdaActual);

                //2.2.3 siempre se a�ade a cerrados una vez tratada
                cjtoCerrados.Add(celdaActual); //la celda actual si era valida se ha expandido y si no lo era no se ha expandido
            }
        }

        return celdasGeneradas;
    }

    public List<Vector2Int> espacioLocal(Vector2Int celdaO, int prof, int filas, int cols, TypeTerrain[,] celdas)
    {
        HashSet<Vector2Int> cjtoCerrados = new HashSet<Vector2Int>(); //son nodos que ya han sido tratados para expandir con exito o sin exito pero que estan dentro siempre del espacio local
        List<Vector2Int> celdasExpandir = new List<Vector2Int>(); //representan las pendientes de expandir aun no tratadas
        List<Vector2Int> celdasGeneradas = new List<Vector2Int>(); //representan las celdas que ya han sido generadas y que eran validas y que forman parte del espacio local

        //1. Se empieza a�adiendo el origen
        celdasExpandir.Add(celdaO);

        //2.Mientras aun hayan celdas por expandir
        while (celdasExpandir.Count != 0)
        {
            //2.1 se obtiene la 1� celda y se elimina de las celdas por expandir
            Vector2Int celdaActual = celdasExpandir[0];
            celdasExpandir.RemoveAt(0);



            //2.2 Hay que comprobar si esta celda ya se expandio pues si es asi ya se trato y ya generara hijos o no no tiene sentido tratarla de nuevo
            if (!cjtoCerrados.Contains(celdaActual))

            {
                //2.2.1 hay que comprobar que esa celda sea valida 
                bool valida = (0 <= celdaActual.x && celdaActual.x < filas) && (0 <= celdaActual.y && celdaActual.y < cols) && celdas[celdaActual.x, celdaActual.y]!=TypeTerrain.invalido;

                //2.2.1 si la celda es valida y se puede expandir se expande
                if (valida && coste(celdaO, celdaActual) < prof)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (i == 0 && j == 0) continue;
                            celdasExpandir.Add(new Vector2Int(celdaActual.x + i, celdaActual.y + j));
                        }
                    }
                }

                //2.2.2 se a�ade si es valida ya se haya expandido o no porque este en el limite de la frontera o no.
                if (valida) celdasGeneradas.Add(celdaActual);

                //2.2.3 siempre se a�ade a cerrados una vez tratada
                cjtoCerrados.Add(celdaActual); //la celda actual si era valida se ha expandido y si no lo era no se ha expandido
            }
        }

        return celdasGeneradas;
    }
  
    public float coste(Vector2Int celdaOrigen, Vector2Int celdaDestino)
    {
        return Mathf.Sqrt(Mathf.Pow(celdaDestino.x - celdaOrigen.x, 2) + Mathf.Pow(celdaDestino.y - celdaOrigen.y, 2));
    }



}
