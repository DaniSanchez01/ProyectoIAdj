public class MapaInfluencia : MonoBehaviour {
    public GameObject celdaPrefab;
    public Transform mapaParent; // Quizá se podría cambiar por alguno de nuestros grid

    private int[,] array_rojo;
    private int[,] array_azul;
    private int[,] array_influencias;
    private Color[,] array_colores;
    private int cellSize = 3;
    private int reduccion_influencia = 0.2;


    void Start() {

        // Inicializar los arrays de distancia y colores
        array_rojo = new int[30, 30]; // Inicializamos el array a 0
        array_azul = new int[30, 30]; // Inicializamos el array a 0
        array_colores = new Color[30, 30];

        // Calcular la distancia desde cada celda hasta los NPC de cada equipo
        CalcularDistancia("NPCazul"); //, 1);
        CalcularDistancia("NPCrojo"); //, -1);

        // Recorrer y rellenar
        // array_influencias = array_rojo + array_azul;
        // Generar los colores para cada celda del mapa en función de su distancia a los NPC
        for (int x = 0; x < 30; x++) {
            for (int y = 0; y < 30; y++) {
                int distancia = array_distancia[x, y];
                if (distancia > 0) {
                    float valor = (float)distancia / 30f;
                    if (valor > 1f) valor = 1f;
                    array_colores[x, y] = Color.Lerp(Color.blue, Color.white, valor);
                } else if (distancia < 0) {
                    float valor = (float)distancia / 30f;
                    if (valor < -1f) valor = -1f;
                    array_colores[x, y] = Color.Lerp(Color.red, Color.white, -valor);
                } else {
                    array_colores[x, y] = Color.white;
                }
            }
        }

        // Generar las celdas del mapa REVISAR (renderizar)
        for (int x = 0; x < 30; x++) {
            for (int y = 0; y < 30; y++) {
                Vector3 posicion = new Vector3(x, 0, y);
                GameObject celda = Instantiate(celdaPrefab, posicion, Quaternion.identity, mapaParent);
                Renderer rend = celda.GetComponent<Renderer>();
                rend.material.color = array_colores[x, y];
            }
        }
    }

    void CalcularDistancia(string tag) {
        // Obtener los NPC del equipo correspondiente
        GameObject[] npcs = GameObject.FindGameObjectsWithTag(tag);

        // Marcar todas las celdas adyacentes a los NPC como visitadas y meterlas en la cola
        foreach (GameObject npc in npcs) {
            Vector2Int coordenadas = getCeldaDePuntoPlano(npc.Position);
            int x = coordenadas.x;
            int y = coordenadas.y;

            if (array_distancia[x, y] < npc.influencia){    
                array_distancia[x, y] = npc.influencia;
                if (npc.influencia > reduccion_influencia) expandir(coordenadas, npc.influencia-reduccion_influencia);
            }
        }
    }

    // Coger con pinzas, ver nodos, listas_coordenadas... // Ver GridPathFinding
    void expandir(float influencia){
        // rellenar
        lista_coordenadas = getVecinosValidosProf(Nodo n, 1);
        foreach (GameObject (x,y) in lista_coordenadas) {
            // es mayor?
            if (array_distancia[x, y] < influencia){    
                array_distancia[x, y] = influencia;
                // paro?
                if (influencia > reduccion_influencia) expandir(coordenadas, influencia-reduccion_influencia);
            }
        }
    }

    public Vector2Int getCeldaDePuntoPlano(Vector3 puntoPlano)
    {
        //1.Se obtiene la cara origen correspondiente al punto
        Vector2Int caraPuntoPlano = new Vector2Int(Mathf.FloorToInt(puntoPlano.x / cellSize),Mathf.FloorToInt(puntoPlano.z / cellSize));

        //2. Se obtiene la posicion respecto a nuestra cara origen de la cara que contiene el punto del plano
        Vector2Int posCelda = new Vector2Int(caraPuntoPlano.x, caraPuntoPlano.y);
        return posCelda;
    }

}
