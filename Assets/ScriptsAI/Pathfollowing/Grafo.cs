using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Como se observa se le da un diccionario y el grafo se crea guardadno ese diccionario
 * La he hecho MonoBehaviour con el fin de que se pueda dibujar el grafo y se pueda mostrar haciendo debug
 */
public class Grafo : MonoBehaviour
{

    private Dictionary<Vector3Int, List<Conexion>> grafo;
    private int L; //representa la longitud del lado del cuadrado del grid cyadrado
    private int limite; //la profunidad escogida del grafo
    private Vector3Int origenGeneracion; //es el punto correspondiente del grafo NO DEL MUNDO que es el origen desde el que se ha generado el grafo
    private Vector3 origenMundo; //quiere decir la posicion original desde la que se llamo al metodo de generar el grafo
    

    
    public List<Conexion> getConexiones(Vector3Int nodo)
    {
        
        return grafo.GetValueOrDefault(nodo,null);
        
    }

    /*
     * Para acceder al diccionario y poder modificarlo o cambiar sus datos
     */
    public Dictionary<Vector3Int, List<Conexion>> Grafoo
    {

        get { return grafo; }
    }


    public void Start()
    {

        L = 1;
        limite = 2;
        origenMundo = transform.position; //La posicion del mundo
        origenGeneracion = new Vector3Int(Mathf.RoundToInt(origenMundo.x), Mathf.RoundToInt(origenMundo.y), Mathf.RoundToInt(origenMundo.z));
        grafo = GeneracionGrafoAnchura.generarGrafo(origenGeneracion, limite);
        
    }

    public void Update()
    {
        //Debug.Log(grafo != null);
        //if (grafo == null) grafo = GeneracionGrafoAnchura.generarGrafo(origenGeneracion, 3);
    }

    private void OnDrawGizmos()
    {

       Debug.Log(grafo != null);
        grafo = GeneracionGrafoAnchura.generarGrafo(Vector3Int.zero, 4);
        if (grafo != null) //por si no hay grafo
        {
            
            Gizmos.color = Color.yellow;

            foreach (KeyValuePair<Vector3Int, List<Conexion>> punto in grafo) //se recorren las entradas par (clave,valor)
            {
                Gizmos.DrawSphere(punto.Key, 0.2f); //parece que deja hacerlo con Vector3Int aun cuando acepta Vector3

                Gizmos.color = Color.blue; //uso el color azul para las conexiones
                foreach (Conexion c in punto.Value)
                {
                    Gizmos.DrawLine(c.Origen, c.Destino);
                }
                Gizmos.color = Color.yellow; //para dibujar el siguiente nodo en amarillo
            }
        }
    }

}
