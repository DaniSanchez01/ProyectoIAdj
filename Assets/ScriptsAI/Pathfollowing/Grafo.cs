using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Como se observa se le da un diccionario y el grafo se crea guardadno ese diccionario
 * La he hecho MonoBehaviour con el fin de que se pueda dibujar el grafo y se pueda mostrar haciendo debug
 */
public class Grafo : MonoBehaviour
{
    //los campos con SerializedField son usados para mostrarse en el editor
    private Dictionary<Vector3Int, List<Conexion>> grafo;
     [SerializeField] private int l; //representa la longitud del lado del cuadrado del grid cyadrado
     [SerializeField] private int limite; //la profunidad escogida del grafo
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

    /*
     * Funcion que se ejecutara con unos valores por defecto en tiempo de ejecucion y genera un grafo correspondiente
     */
    public void Start()
    {

        l = 1;
        limite = 2;
        origenMundo = transform.position; //La posicion del mundo
        
        origenGeneracion = new Vector3Int(Mathf.RoundToInt(origenMundo.x / l), Mathf.RoundToInt(0f), Mathf.RoundToInt(origenMundo.z / l)); //origen de la generacion simboliza el
                                                                                                                                           //vertice mas cercano al punto del mundo donde se comenzara a generar el grafo
        grafo = GeneracionGrafoAnchura.generarGrafo(origenGeneracion, limite);
        
    }

    public void Update()
    {
        //Debug.Log(grafo != null);
        //if (grafo == null) grafo = GeneracionGrafoAnchura.generarGrafo(origenGeneracion, 3);
    }


    private void OnDrawGizmos()
    {

        origenMundo = transform.position;
        origenGeneracion = new Vector3Int(Mathf.RoundToInt(origenMundo.x / l), Mathf.RoundToInt(0f), Mathf.RoundToInt(origenMundo.z / l));

        grafo = GeneracionGrafoAnchura.generarGrafo(origenGeneracion, limite);
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
