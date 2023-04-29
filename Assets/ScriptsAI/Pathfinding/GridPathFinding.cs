using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Suposiciones: que el mapa su esquina inferior izquiera empieza en el (0,0,0),
 * 
 */


public class GridPathFinding : MonoBehaviour
{
    [SerializeField] private int filas; //numero de celdas que hay a lo largo del ancho notese que NO es lo mismo que filas porque esto representa las celdas de longitud "cellSize" a lo largo del ancho
    [SerializeField] private int columnas; //numero de celdas que hay para cada columna
    [SerializeField] private float cellSize; //longitud del cuadrado del grid
    [SerializeField] private bool tactics = true; 

    private Nodo[,] celdasGrid; //sera un array bidimensional donde se accede a la celda i,j-esima
    private Heuristica heuristicagrid;
    [SerializeField] bool activarGizmos; //se usa para activar los gizmos o no.


    //propiedades

    public Nodo[,] CeldasGrid
    {
        get { return celdasGrid; }
    }

    public Heuristica HeuristicaGrid {
        get {return heuristicagrid;}
    }
    
    public bool Tactics {
        get {return tactics;}
        set {
            tactics = value;}
    }

    public int Filas 
    {
        get {return filas;}
    }

    public int Columnas 
    {
        get {return columnas;}
    }

    //metodos

    public void inicializarGrid(int ancho, int largo, float cellSize, typeHeuristica heuristicaDeseada, bool giz)
    {
        //1. Se introducen loas propiedades del grid
        filas = ancho;
        columnas = largo;
        this.cellSize = cellSize;
        heuristicagrid = FactoriaHeuristica.crearHeuristica(heuristicaDeseada);
        celdasGrid = new Nodo[filas, Columnas]; //se crea el grid acorde a las celdas y columnas
        activarGizmos = giz;

        //2. Se averiguan que nodos son validos
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < Columnas; j++)
            {
                //1. Se comprueba si la celda es valida y tambien se establece el coste de la zona a 1 
                bool valida = hayColisionEnCelda(i, j);
                celdasGrid[i, j] = new Nodo(i, j, valida, 1); //El coste de cada celda se supone que es 1
            }
        }
    }

    public Nodo GetNodo(int i, int j) {
        if ((filas > i && i >= 0) && (Columnas > j && j >= 0)) {
            return CeldasGrid[i,j];
        }
        else return null;
    }

    /*
     * Dada una celda del grid marcada por su fila y columna comprueba si en esta hay objetos del escenario y por tanto si es transitable o no lo es.
     * Si se encuentra un objeto NO es valida, en caso contrario si lo es
     * Pre: filas>fila>=0 y columnas>columna>=0, no debe ser una fila y columna que no abarquemos ( negativas o fuera del array) 
     */
    private bool hayColisionEnCelda(int fila, int columna)
    {
        //1. hallar punto medio del cubo en la celda correspondiente usando para ello el cellsize. Para esto se obtiene el punto que corresponde a la celda y sumando la mitad del lado
        //se obtiene el punto medio del cubo
        Vector3 puntoMedioCubo = getPuntoPlanoDeCelda(fila, columna);
        puntoMedioCubo = new Vector3(puntoMedioCubo.x, 0f, puntoMedioCubo.z); //ahora tenemos el punto medio real donde colocar el cubo

        //2. Se obtienen los objetos que colisionan con el cubo de lado cellSize (tamano de la celda) y cuyo punto medio es el calculado anteriormente. Y esta alineado con los ejes
        Collider[] objetoscolisionados = Physics.OverlapBox(puntoMedioCubo, new Vector3(cellSize / 2, 0, cellSize / 2), Quaternion.identity);

        //3. Se busca si ha colisionado con algun objeto del escenario.
        foreach(Collider obj in objetoscolisionados)
        {
            // Vector3 puntomasCercano = Physics.ClosestPoint(puntoMedioCubo, obj, obj.gameObject.transform.position, obj.gameObject.transform.rotation);
            //((puntomasCercano - puntoMedioCubo).magnitude < cellSize /2 )
            if (obj.CompareTag("Wall")) return false; //si choca con algun objeto del escenario NO es valida
        }
        return true; //en caso contrario si lo es
    }

    /*
     * Comprueba que la celda es valida al ver que no se sale del grid y que el nodo que la representa es transitable. Es decir que la celda es transitable y esta dentro del mapa
     * Pre: se debe haber inicializado todos los nodos del grid
     */
    public bool esValidaCelda(int fila,int columna)
    {
        return ((filas > fila && fila >= 0) && (Columnas > columna && columna >= 0)) && celdasGrid[fila, columna].Transitable;
    }
    /*
     * Indica si el punto es valido o no
     * Pre: deben haberse validado las celdas del grid
     */
    public bool esValidoPunto(Vector3 puntoDestino)
    {
        //1.Obtiene la posicion de la celda correspondiente al punto pasado como parametro.
        Vector2Int posCelda = getCeldaDePuntoPlano(puntoDestino);
        //2. Comprueba si el punto cae dentro de una celda valida
        return esValidaCelda(posCelda.x, posCelda.y);
    }

    /*
     * Dada una posicion de una celda retorna la posicion x,z del plano
     * OJO que es MUY importante que estos valores no sean negativos porque
     * solo se puede devolver a partir de que i>=0 e j>=0
     */
    public Vector3 getPuntoPlanoDeCelda(int i, int j)
    {
        //1. Calcula el punto para esa cara destino en base al tamaño del lado
        return new Vector3(i * cellSize+cellSize/2, 0f, j * cellSize+cellSize/2);
    }

    /*
     * De la misma forma se supone que el punto indicado debe de estar en una celda i,j donde i>=0 y j>=0 y que claramente i<n j<m pues recordamos que las celdas empiezan a contar desde 0
     */
    public Vector2Int getCeldaDePuntoPlano(Vector3 puntoPlano)
    {
        //1.Se obtiene la cara origen correspondiente al punto
        Vector2Int caraPuntoPlano = new Vector2Int(Mathf.FloorToInt(puntoPlano.x / cellSize),Mathf.FloorToInt(puntoPlano.z / cellSize));

        //2. Se obtiene la posicion respecto a nuestra cara origen de la cara que contiene el punto del plano
        Vector2Int posCelda = new Vector2Int(caraPuntoPlano.x, caraPuntoPlano.y);
        return posCelda;
    }


    /*
     * Establece la heuristica de cada nodo de cada celda respecto a un punto destino que caera en una celda determinada
     * Pre: el grid se debe haber inicializado
     */
    public void setValoresHeuristicos(Vector3 puntoDestino) {

        // 1. si es valido es decir cae en una zona transitable y que no esta fuera del grid
        if (esValidoPunto(puntoDestino)) { 

            //2. Se calcula la celda del grid donde cae el punto
            Vector2Int celdaDestino = getCeldaDePuntoPlano(puntoDestino);

                for (int i = 0; i < filas; i++) {

                    for (int j = 0; j < Columnas; j++)
                    {
                    //3. Si el grid es transitable se calcula la heuristica entre el nodo origen y destino
                    if (celdasGrid[i, j].Transitable) celdasGrid[i, j].CosteHeuristica = heuristicagrid.coste(celdasGrid[i, j].Celda, celdaDestino);
                    }

                }
        }
        else
        {
            Debug.LogError("El punto de destino no es valido");
            return; //no se hace nada
        }
    }

    public void setValoresHeuristicos(Nodo nodoDestino) {

        // 1. si es valido es decir cae en una zona transitable y que no esta fuera del grid
        Vector2Int celdaDestino = nodoDestino.Celda;
        if (esValidaCelda(celdaDestino.x,celdaDestino.y)) { 
                for (int i = 0; i < filas; i++) {

                    for (int j = 0; j < Columnas; j++)
                    {
                    //3. Si el grid es transitable se calcula la heuristica entre el nodo origen y destino
                    if (celdasGrid[i, j].Transitable) celdasGrid[i, j].CosteHeuristica = heuristicagrid.coste(celdasGrid[i, j].Celda, celdaDestino);
                    }

                }
        }
        else
        {
            Debug.LogError("El punto de destino no es valido");
            return; //no se hace nada
        }
    }



    public List<Nodo> getVecinosValidosProf(Nodo n, int prof)
    {
        //1.Se crea la lista de nodos
        List<Nodo> nodosEspacioLocal = new List<Nodo>();

        //2. Se obtienen las  celdas correspondientes al espacio local y que son validas
        List<Vector2Int> celdasEspacioLocal = heuristicagrid.espacioLocal(n.Celda, prof,filas,Columnas,celdasGrid);


        //3. Para cada celda se añade su nodo directamente porque ya sabemos que son validas es decir estan dentro del grid. De esta forma se obtienen los nodos validos
        //hasta cierta prof
        foreach(Vector2Int celda in celdasEspacioLocal)
        {
           nodosEspacioLocal.Add(celdasGrid[celda.x, celda.y]);
        }

        return nodosEspacioLocal;
    }

    /*public List<Nodo> eliminarNoTransitables(List<Nodo> listaNodos) {
        List<Nodo> answer = new List<Nodo>();
        foreach (var nodo in listaNodos) {
            if (esValidaCelda(nodo.Celda.x, nodo.Celda.y)) {
                answer.Add(nodo);
            }
        }
        return answer;

    }*/

    //se dibuja respecto a la coordenada origen 0,0
    private void OnDrawGizmos()
    {
        
        if (activarGizmos) //si estan activados los gizmos
        {
            //para cada fila que tendremos mas o menos segun el tama�o del lado del cudrado
            for (int i = 0; i < filas; i++)
            {

                //para una columna determinada se colorean las columnas que tendremos mas o menos segun el lado del cuadrado
                for (int j = 0; j < Columnas; j++)
                {
                    if (hayColisionEnCelda(i,j))
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
