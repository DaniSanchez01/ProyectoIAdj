using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Suposiciones: que el mapa su esquina inferior izquiera empieza en el (0,0,0), 
 * 
 */



/*
 * Representa una celda que tendra un booleano para indicar si es transitable o no y un valor real que indica el coste heuristico hacia algun objetivo
 * una estructura siempre tiene de forma prederminada un constructor que pone todos los valores a cero
 */
public struct Celda 
{
    bool transitable;
    float valorheuristico;
    //todos los costes de las conexiones con los nodos transitables son 1.


    public bool Transitable { 

        get { return transitable;  }
        set { transitable = value;  }
    }

    public float Heuristica
    {

        get { return valorheuristico; }
        set { valorheuristico = value; }
    }
}




public class GridPathFinding : MonoBehaviour
{
    [SerializeField] private int filas; //numero de celdas que hay a lo largo del ancho notese que NO es lo mismo que filas porque esto representa las celdas de longitud "cellSize" a lo largo del ancho
    [SerializeField] private int Columnas; //numero de celdas que hay para cada columna
    [SerializeField] private float cellSize; //longitud del cuadrado del grid
    private Celda[,] celdasGrid; //sera un array bidimensional donde se accede a la celda i,j-esima
    private Heuristica heuristicagrid;
    [SerializeField] bool activarGizmos; //se usa para activar los gizmos o no.

    public void inicializarGrid(int ancho,int largo,int cellSize,string heuristicaDeseada)
    {
        //1. Se introducen los valores al grid
        filas = ancho;
        Columnas = largo;
        this.cellSize = cellSize;
        heuristicagrid = FactoriaHeuristica.crearHeuristica(heuristicaDeseada);
        celdasGrid = new Celda[filas, Columnas]; //se crea el grid acorde a las celdas y columnas


        validarNodos();
        
    }

    /*
     * Esta funcion se encarga de comprobar para cada una de las celdas que tiene el grid si en esta celda no hay ningun objeto y por tanto es valida.
     * Pre: Debe haberse calculado anteriormente la variable celdasFila y celdasColumna con el cellSize del grid.
     */
    private void validarNodos()
    {
        
        for(int i=0;i<filas;i++)
        {
            for(int j=0;j<Columnas;j++)
            {
                //1. Se comprueba si hay colision
                bool haycolision = comprobarColisionesConobjetosEscenario(i, j);
                //2. Si la hay con algun objeto del escenario entonces no es transitable en caso contrario si lo es
                celdasGrid[i, j].Transitable = !haycolision;

                
            }
        }

        
    }

    /*
     * Dada una celda del grid marcada por su fila y columna comprueba si en esta hay objetos del escenario y por tanto si es transitable o no lo es.
     * Pre: filas>fila>=0 y columnas>columna>=0, no debe ser una fila y columna que no abarquemos ( negativas o fuera del array) 
     */
    private bool comprobarColisionesConobjetosEscenario(int fila, int columna)
    {
        //1. hallar punto medio del cubo en la celda correspondiente usando para ello el cellsize. Para esto se obtiene el punto que corresponde a la celda y sumando la mitad del lado
        //se obtiene el punto medio del cubo
        Vector3 puntoMedioCubo = getPuntoPlanoDeCelda(fila, columna);
        puntoMedioCubo = new Vector3(puntoMedioCubo.x + cellSize / 2f, 0f, puntoMedioCubo.z + cellSize / 2f); //ahora tenemos el punto medio real donde colocar el cubo

        //2. Se obtienen los objetos que colisionan con el cubo de lado cellSize (tamaño de la celda) y cuyo punto medio es el calculado anteriormente. Y esta alineado con los ejes
        Collider[] objetoscolisionados = Physics.OverlapBox(puntoMedioCubo, new Vector3(cellSize / 2, 0, cellSize / 2), Quaternion.identity);

        //3. Se busca si ha colisionado con algun objeto del escenario.
        foreach(Collider obj in objetoscolisionados)
        {
            // Vector3 puntomasCercano = Physics.ClosestPoint(puntoMedioCubo, obj, obj.gameObject.transform.position, obj.gameObject.transform.rotation);
            //((puntomasCercano - puntoMedioCubo).magnitude < cellSize /2 )
            if (obj.tag == "ObjetoEscenario"  ) return true;
        }
        return false;
    }


    /*
     * Dada una posicion de una celda retorna la posicion x,z del plano
     * OJO que es MUY importante que estos valores no sean negativos porque
     * solo se puede devolver a partir de que i>=0 e j>=0
     */
    public Vector3 getPuntoPlanoDeCelda(int i, int j)
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

    public Vector2Int getCeldaDePuntoPlano(Vector3 puntoPlano)
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

  


    //se dibuja respecto a la coordenada origen 0,0
    private void OnDrawGizmos()
    {
        
        if (activarGizmos) //si estan activados los gizmos
        {
            //para cada fila que tendremos mas o menos segun el tamaño del lado del cudrado
            for (int i = 0; i < filas; i++)
            {

                //para una columna determinada se colorean las columnas que tendremos mas o menos segun el lado del cuadrado
                for (int j = 0; j < Columnas; j++)
                {
                    if (!comprobarColisionesConobjetosEscenario(i, j))
                    {
                        Gizmos.color = Color.blue;
                        //de la cara obtenemos las esquinas que definen la cara y lo pasamos al mundo real
                        Vector3 esquina1 = new Vector3(i * cellSize, 0f, j * cellSize);
                        Vector3 esquina2 = new Vector3((i + 1) * cellSize, 0f, j * cellSize);
                        Vector3 esquina3 = new Vector3(i * cellSize, 0f, (j + 1) * cellSize);
                        Vector3 esquina4 = new Vector3((i + 1) * cellSize, 0f, (j + 1) * cellSize);
                        Gizmos.DrawSphere(esquina1, cellSize / 12);
                        Gizmos.DrawSphere(esquina2, cellSize / 12);
                        Gizmos.DrawSphere(esquina3, cellSize / 12);
                        Gizmos.DrawSphere(esquina4, cellSize / 12);
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine(esquina1, esquina2);
                        Gizmos.DrawLine(esquina1, esquina3);
                        Gizmos.DrawLine(esquina3, esquina4);
                        Gizmos.DrawLine(esquina2, esquina4);

                    }
                }



            }
        }

    }

}
