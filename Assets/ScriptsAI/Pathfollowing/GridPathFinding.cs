using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Supongo que el mapa su esquina inferior izquiera empieza en el (0,0,0)
 * porque si no la division del grid al mapa no va a ser correcta y sera mas dificil
 * 
 * Por otra parte esta clase la he creado con el fin de poder dividir el terreno pero que tambien cada celda tenga lo necesario para llevar a cabo el algoritmo de Pathfinding que se quiere
 */

public class GridPathFinding : MonoBehaviour
{
    [SerializeField] private int ancho; //ancho del mapa en unidades    
    [SerializeField] private int largo; //largo del mapa en unidades
    [SerializeField] private int celdasfilas; //numero de celdas que hay a lo largo del ancho notese que NO es lo mismo que filas porque esto representa las celdas de longitud "cellSize" a lo largo del ancho
    [SerializeField] private int celdasColumnas; //numero de celdas que hay para cada columna
    [SerializeField] private float cellSize; //longitud del cuadrado del grid
    private Heuristica heuristicagrid;
    //Aun falta como tal el array que guarda para cada grid un booleano
    //de si es transitable o no
   
    //Por otra parte e,l grid tambien podria tener informacion de un coste
    //heurisitco respecto a un destino pero pienso que eso tal vez seria mejor
    //hacerlo en la estructura de grafo que ya tenemos creada



    public void inicializarGrid(int ancho,int largo,int cellSize,string heuristicaDeseada)
    {
        this.ancho = ancho;
        this.largo = largo;
        this.cellSize = cellSize;
        heuristicagrid = FactoriaHeuristica.crearHeuristica(heuristicaDeseada);
        
    }


    /*
     * Dada una posicion de una celda retorna la posicion x,z del plano
     * OJO que es MUY importante que estos valores no sean negativos porque
     * solo se puede devolver a partir de que i>=0 e j>=0
     */
    public Vector3 getPuntoPlanoDePosicion(int i, int j)
    {

        

        //1. se obtiene la cara origen que toma como la cara (0,0)
        Vector2Int caraOrigen = Vector2Int.zero; //se parte dela 0,0

        //2. Obtiene la cara destino en base a un desplazamiento i,j respecto a la caraOrigen
        Vector2Int caraDestino = new Vector2Int(caraOrigen.x + i, caraOrigen.y + j);

        //3. Calcula el punto para esa cara destino
        return new Vector3(caraDestino.x * cellSize, 0f, caraDestino.y * cellSize);

        
    }

    /*
     * De la misma forma se supone que el punto indicado debe de estar en una celda i,j donde i>=0 y j>=0 y que claramente i<n j<m pues recordamos que las celdas empiezan a contar desde 0
     */

    public Vector2Int getPosicionDePuntoPlano(Vector3 puntoPlano)
    {
        //1. se obtiene la cara origen que toma como la cara (0,0)
        Vector2Int caraOrigen = Vector2Int.zero;

        //2.Se obtiene la cara origen correspondiente al punto
        Vector2Int caraPuntoPlano = new Vector2Int(Mathf.FloorToInt(puntoPlano.x / cellSize),Mathf.FloorToInt(puntoPlano.z / cellSize));

        //3. Se obtiene la posicion respecto a nuestra cara origen de la cara que contiene el punto del plano
        Vector2Int posCelda = new Vector2Int(caraPuntoPlano.x - caraOrigen.x, caraPuntoPlano.y - caraOrigen.y);
        return posCelda;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmos()
    {

        Vector2Int caraOrigen = Vector2Int.zero;

        Gizmos.color = Color.blue;

        Color antColor = Color.blue; //se usa para saber porque color se empezo en la fila anterior
        
        celdasColumnas = Mathf.FloorToInt((float)  largo / cellSize);
        celdasfilas = Mathf.FloorToInt((float) ancho / cellSize);

        //para cada fila que tendremos mas o menos segun el tamaño del lado del cudrado
        for (int i=0;i<celdasfilas;i++)
        {
            
            //para una columna determinada se colorean las columnas que tendremos mas o menos segun el lado del cuadrado
            for(int j = 0;j<celdasColumnas;j++)
            {
                //se van dibujando la celda
                Gizmos.DrawCube(new Vector3((caraOrigen.x + i) * cellSize + cellSize / 2f, 0f, (caraOrigen.y + j) * cellSize + cellSize / 2f), new Vector3(cellSize, 0, cellSize));
                if (Gizmos.color == Color.blue) Gizmos.color = Color.red;
                else Gizmos.color = Color.blue; //para ir cambiando el color
            }

            //se comprueba el color anterior para empezar la siguiente fila por el color contrario
            if (antColor == Color.blue) {
                antColor = Color.red;
                    Gizmos.color = Color.red;
                    }
            else //si es rojo
            {
                antColor = Color.blue;
                Gizmos.color = Color.blue;
            }
        }

    }

}
