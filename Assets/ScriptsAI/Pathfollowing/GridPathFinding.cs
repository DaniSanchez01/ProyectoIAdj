using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Supongo que el mapa su esquina inferior izquiera empieza en el (0,0,0)
 * porque si no la division del grid al mapa no va a ser correcta
 */

public class GridPathFinding : MonoBehaviour
{
    [SerializeField] private int filas;
    [SerializeField] private int columnas;
    [SerializeField] private float cellSize; //longitud del cuadrado del grid
    [SerializeField] private Vector3 origen; //Representa el punto origen de la celda
    //--> no se si  esto funcionaria lo anterior
    
    //Aun falta como tal el array que guarda para cada grid un booleano
    //de si es transitable o no
   
    //Por otra parte e,l grid tambien podria tener informacion de un coste
    //heurisitco respecto a un destino pero pienso que eso tal vez seria mejor
    //hacerlo en la estructura de grafo que ya tenemos creada

    /*
     * Dada una posicion de una celda retorna la posicion x,z del plano
     * OJO que es MUY importante que estos valores no sean negativos porque
     * solo se puede devolver a partir de que i>=0 e j>=0
     */
    public Vector3 getPuntoPlanoDePosicion(int i, int j)
    {
        //1. se obtiene la cara origen que toma como la cara (0,0)
        Vector2Int caraOrigen = new Vector2Int (Mathf.FloorToInt(origen.x / cellSize),
        Mathf.FloorToInt(origen.z / cellSize));

        //2. Obtiene la cara destino en base a un desplazamiento i,j respecto a la caraOrigen
        Vector2Int caraDestino = new Vector2Int(caraOrigen.x + i, caraOrigen.y + j);

        //3. Calcula el punto para esa cara destino
        return new Vector3(caraDestino.x * cellSize, 0f, caraDestino.y * cellSize);

        
    }

    /*
     * De la misma forma se supone que el punto indicado debe estar
     * Debe ser un punto que contenga nuestro grid porque si no lo contiene
     * seria un error
     */

    public Vector2Int getPosicionDePuntoPlano(Vector3 puntoPlano)
    {
        //1. se obtiene la cara origen que toma como la cara (0,0)
        Vector2Int caraOrigen = new Vector2Int(Mathf.FloorToInt(origen.x / cellSize),
        Mathf.FloorToInt(origen.z / cellSize));

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
        Vector3 posicionActual = transform.position; //pos actual del game object
        Vector2Int caraOrigen = new Vector2Int(Mathf.FloorToInt(posicionActual.x / cellSize),
        Mathf.FloorToInt(posicionActual.z / cellSize));

        Gizmos.color = Color.blue;

        Color antColor = Color.blue; //se usa para saber porque color se empezo en la fila anterior
        //para cada fila
        for (int i=0;i<filas;i++)
        {
            
            //para una fila determinada se colorean las columnas
            for(int j = 0;j<columnas;j++)
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
