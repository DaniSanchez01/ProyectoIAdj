using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneracionGrafoAnchura : MonoBehaviour
{

    private Vector2Int origenGeneracion; //es el punto correspondiente del grafo NO DEL MUNDO que es el origen desde el que se ha generado el grafo
    private Grafo grafoGenerado;
    int L; //representa la longitud del lado del cuadrado del grid cyadrado
    int limite; //la profunidad escogida del grafo

    public GeneracionGrafoAnchura(Vector2Int origen, int L,int limite) 
        {
        this.origenGeneracion = origen;
        this.L = L;
        this.limite = limite;
        }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
