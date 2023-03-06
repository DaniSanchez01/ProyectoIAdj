using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Esta clase representa la conexion de un nodo con otro, como se observa se tiene un coste. Como se observa he usado enteros para representar valores discretos dentro de un grafo.
 * La clase "Vector3Int" solo es un Vector3 que en vez de floats tiene Int.
 */
public class Conexion
{

    private Vector3Int origen;
    private Vector3Int destino;
    int coste;

    public Conexion(Vector3Int origen,Vector3Int destino,int coste)
    {
        this.origen = origen;
        this.destino = destino;
        this.coste = coste;

    }

    //declaracion de propiedades (metodos get y set)

    public Vector3Int Origen //obtiene el nodo origen
    {
        get{ return origen; }
    }

    public Vector3Int Destino //obtiene el destino
    {
        get { return destino;  }
    }

    public int Coste
    {
        get { return coste;  }
    }

}
