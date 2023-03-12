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
    private Vector3Int origenGeneracion; //es la cara origen del grafo
    private Vector3 origenMundo; //quiere decir la posicion original desde la que se llamo al metodo de generar el grafo y por tanto que contiene la cara origen
    

    
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
        
        //se realiza a la cara mas cercana
        origenGeneracion = new Vector3Int(Mathf.FloorToInt(origenMundo.x / l), Mathf.FloorToInt(0f), Mathf.FloorToInt(origenMundo.z / l)); 

        grafo = GeneracionGrafoAnchura.generarGrafo(origenGeneracion, limite);
        
    }

    public void Update()
    {
        // no lo hago en Update porque si no mostraria el grafo solo en tiempo de ejecucon
    }

    float retornaSigno(float f)
    {
        if (f == 0f) return 1;
        else return Mathf.Sign(f);
    }


    private void OnDrawGizmos()
    {

        origenMundo = transform.position; //la posicion actual donde esta el objeto en el mundo
        origenGeneracion = new Vector3Int(Mathf.FloorToInt(origenMundo.x / l), Mathf.FloorToInt(0f), Mathf.FloorToInt(origenMundo.z / l)); //a la cara mas cercana

        //se genera un grafo a partir de las caras las esferas son las caras
        grafo = GeneracionGrafoAnchura.generarGrafo(origenGeneracion, limite);
        if (grafo != null) //por si no hay grafo
        {
            
            foreach(KeyValuePair<Vector3Int, List<Conexion>> punto in grafo)
            {
                //se colorean los cuadrados
                if ((Mathf.Abs(origenGeneracion.x - punto.Key.x) + Mathf.Abs(origenGeneracion.z - punto.Key.z)) % 2 == 0) Gizmos.color = Color.red; //para las celdas de profunidad par respecto al orige
                else Gizmos.color = Color.yellow;
                Vector3 CentroCaraActual;
                //if(punto.Key.x < 0 || punto.Key.z < 0) CentroCaraActual = new Vector3(punto.Key.x)
                CentroCaraActual = new Vector3(punto.Key.x *l +  l/2f, 0f, punto.Key.z *l +  l/2f);
                Debug.Log(CentroCaraActual);
                Gizmos.DrawCube(CentroCaraActual, new Vector3(l,0,l));
            }

            
            /* Representacion anterior con esferas
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
            */
        }
    }

}
