using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeMap {
        Ninguno,
        Influencia,
        Tension,
        Vulnerabilidad
    }

public class MapasTacticos : MonoBehaviour {
    private float[,] array_rojo;
    private float[,] array_azul;
    private float[,] array_final;
    private int cellSize = 3;
    private float reduccion_influencia = 0.2f;

    public GameObject minim;

    public TypeTerrain[,] mapaTerreno;
    public typeHeuristica heuristica;
    private Heuristica h;

    private MinimapaQuad minimapa;

    private typeMap tipoMapa;
    
    public typeMap TipoMapa{
        get{return tipoMapa;}
    }

    void Start() {

        // Inicializar los arrays de distancia y colores
        array_rojo = new float[30, 30]; // Inicializamos el array a 0
        array_azul = new float[30, 30]; // Inicializamos el array a 0
        array_final = new float[30, 30]; // Inicializamos el array a 0                
        minimapa= FindObjectOfType<MinimapaQuad>();
        TerrainMap scriptMapa= FindObjectOfType<TerrainMap>();
        mapaTerreno = scriptMapa.MapaTerreno;
        h = FactoriaHeuristica.crearHeuristica(heuristica);
        StartCoroutine(decrementarMapa());

        
    }

    void Update() {
        // Calcular la distancia desde cada celda hasta los NPC de cada equipo
        CalcularDistancia("NPCazul"); //, 1);
        CalcularDistancia("NPCrojo"); //, -1);
        switch (tipoMapa) {
            case (typeMap.Influencia):
                array_final = calculaInfluencia();
                minimapa.ChangeColor(array_final);
                break;
            case (typeMap.Tension):
                array_final = calculaTension();
                minimapa.ChangeColorTension(array_final);
                break;
            case (typeMap.Vulnerabilidad):
                array_final = calculaVulnerabilidad();
                minimapa.ChangeColorTension(array_final);
                break;
            default:
                break;
        }
    }

    public void nextMap() {
        int num = (int)tipoMapa+1;
        num = num % 4;
        tipoMapa = (typeMap) num;
        if (tipoMapa == typeMap.Ninguno) {
            minim.SetActive(false);
        }
        else if (tipoMapa == typeMap.Influencia) {
            minim.SetActive(true);
        }
    }
    public IEnumerator decrementarMapa()
    {
        while (true) {
            yield return new WaitForSeconds(1); //Esperate 2 segundos quieto
            for (int i=0;i<30;i++) {
                for (int j=0;j<30;j++) {
                    array_azul[i,j] = Mathf.Max(0, array_azul[i,j]-0.05f);
                    array_rojo[i,j] = Mathf.Max(0, array_rojo[i,j]-0.05f);
                }
            }
        }
    }
    

    void CalcularDistancia(string tag) {
        float[,] mapa;
        // Obtener los NPC del equipo correspondiente
        if (tag == "NPCazul") mapa = array_azul;
        else mapa = array_rojo;
        GameObject[] npcs = GameObject.FindGameObjectsWithTag(tag);

        // Marcar todas las celdas adyacentes a los NPC como visitadas y meterlas en la cola
        foreach (GameObject gObject in npcs) {
            AgentNPC npc = gObject.GetComponent<AgentNPC>();
            Vector2Int celda = getCeldaDePuntoPlano(npc.Position);
            int x = celda.x;
            int y = celda.y;

            if (mapa[x, y] < npc.influencia){    
                mapa[x, y] = npc.influencia;
                if (npc.influencia > reduccion_influencia) expandir(mapa,celda, npc.influencia-reduccion_influencia);
            }
        }
    }
    float[,] calculaInfluencia() {
        float[,] array = new float[30,30];
        for (int i=0;i<30;i++) {
            for (int j=0;j<30;j++) {
                array[i,j] = array_azul[i,j]-array_rojo[i,j]; 
            }
        }
        return array;
    }

    float[,] calculaTension() {
        float[,] array = new float[30,30];
        for (int i=0;i<30;i++) {
            for (int j=0;j<30;j++) {
                array[i,j] = array_azul[i,j]+array_rojo[i,j]; 
            }
        }
        return array;
    }

    float[,] calculaVulnerabilidad() {
        float[,] array = new float[30,30];
        for (int i=0;i<30;i++) {
            for (int j=0;j<30;j++) {
                float tension = array_azul[i,j]+array_rojo[i,j];
                float influencia = Mathf.Abs(array_azul[i,j]-array_rojo[i,j]);
                array[i,j] = tension-influencia;
            }
        }
        return array;
    }

    
    // Coger con pinzas, ver nodos, listas_coordenadas... // Ver GridPathFinding
    void expandir(float[,] mapa,Vector2Int celda, float influencia){
        // rellenar
        List<Vector2Int> lista_vecinos = getVecinosValidos(celda);
        foreach (var vecino in lista_vecinos) {
            // es mayor?
            if (mapa[vecino.x, vecino.y] < influencia){    
                mapa[vecino.x, vecino.y] = influencia;
                // paro?
                if (influencia > reduccion_influencia) expandir(mapa,vecino, influencia-reduccion_influencia);
            }
        }
    }


    public List<Vector2Int> getVecinosValidos(Vector2Int n)
    {
        //2. Se obtienen las  celdas correspondientes al espacio local y que son validas
        List<Vector2Int> vecinos = h.espacioLocal(n, 1,30,30,mapaTerreno);
        return vecinos;
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
