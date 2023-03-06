using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Como se observa se le da un diccionario y el grafo se crea guardadno ese diccionario
 */
public class Grafo : MonoBehaviour
{

    private Dictionary<Vector3Int, List<Conexion>> grafo;
   public Grafo(Dictionary<Vector3Int,List<Conexion>> contenidoGrafo)
    {
        grafo = contenidoGrafo;
    }
    
    public List<Conexion> getConexiones(Vector3Int nodo)
    {
        
        return grafo.GetValueOrDefault(nodo,null);
        
    }


    public void Update()
    {
        //aqui no hace falta actualizar nada
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        foreach( KeyValuePair<Vector3Int,List<Conexion>>  punto in grafo ) //se recorren las entradas par (clave,valor)
        {
            Gizmos.DrawSphere(punto.Key, 1); //parece que deja hacerlo con Vector3Int aun cuando acepta Vector3

            Gizmos.color = Color.blue; //uso el color azul para las conexiones
            foreach (Conexion c in punto.Value)
            {
                Gizmos.DrawLine(c.Origen, c.Destino);
            }
            Gizmos.color = Color.yellow; //para dibujar el siguiente nodo en amarillo
        }
    }

}
