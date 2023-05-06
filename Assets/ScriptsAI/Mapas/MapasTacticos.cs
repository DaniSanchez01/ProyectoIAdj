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

    private float[,] influencia;
    private float[,] tension;
    private float[,] vulnerabilidad;


    private int cellSize = 3;
    private float reduccion_influencia = 0.15f;

    public GameObject minim;

    public TypeTerrain[,] mapaTerreno;
    public typeHeuristica heuristica;
    private Heuristica h;

    private MinimapaQuad minimapa;

    private typeMap tipoMapa;
    
    public typeMap TipoMapa{
        get{return tipoMapa;}
    }

    public float[,] mapaInfluencia {
        get {return influencia;}
    }

    public float[,] mapaTension {
        get {return tension;}
    }

    public float[,] mapaVulnerabilidad {
        get {return vulnerabilidad;}
    }

    public float getInfluencia(int x, int y) {
        return influencia[x,y];
    }

    public float getTension(int x, int y) {
        return tension[x,y];
    }

    public float getVulnerabilidad(int x, int y) {
        return vulnerabilidad[x,y];
    }
    
    void Awake() {

        // Inicializar los arrays de distancia y colores
        array_rojo = new float[30, 30]; // Inicializamos el array a 0
        array_azul = new float[30, 30]; // Inicializamos el array a 0
        array_final = new float[30, 30]; // Inicializamos el array a 0
        influencia = new float[30, 30]; // Inicializamos el array a 0                
        tension = new float[30, 30]; // Inicializamos el array a 0                
        vulnerabilidad = new float[30, 30]; // Inicializamos el array a 0                
        minimapa= FindObjectOfType<MinimapaQuad>();
        TerrainMap scriptMapa= FindObjectOfType<TerrainMap>();
        mapaTerreno = scriptMapa.MapaTerreno;
        h = FactoriaHeuristica.crearHeuristica(heuristica);
        StartCoroutine(calcularMapas());
        StartCoroutine(decrementarMapa());


        
    }

    void Update() {
        // Calcular la distancia desde cada celda hasta los NPC de cada equipo
        
        switch (tipoMapa) {
            case (typeMap.Influencia):
                array_final = influencia;
                minimapa.ChangeColor(array_final);
                break;
            case (typeMap.Tension):
                array_final = tension;
                minimapa.ChangeColorTension(array_final);
                break;
            case (typeMap.Vulnerabilidad):
                array_final = vulnerabilidad;
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
            yield return new WaitForSeconds(1);
            for (int i=0;i<30;i++) {
                for (int j=0;j<30;j++) {
                    array_azul[i,j] = Mathf.Max(0, array_azul[i,j]-0.05f);
                    array_rojo[i,j] = Mathf.Max(0, array_rojo[i,j]-0.05f);
                }
            }
        }
    }

    public IEnumerator calcularMapas()
    {
        while (true) {
            algoritmoInundacion("NPCazul");
            algoritmoInundacion("NPCrojo"); 
            for (int i=0;i<30;i++) {
                for (int j=0;j<30;j++) {
                    influencia[i,j] = array_azul[i,j]-array_rojo[i,j];
                    tension[i,j] = array_azul[i,j]+array_rojo[i,j];
                    vulnerabilidad[i,j] = tension[i,j]-Mathf.Abs(influencia[i,j]);
                }
            }
            yield return new WaitForSeconds(0.5f); //Esperate 2 segundos quieto                
        }
    }

    void algoritmoInundacion(string tag) {
        float[,] mapa;
        // Obtener los NPC del equipo correspondiente
        if (tag == "NPCazul") mapa = array_azul;
        else mapa = array_rojo;
        GameObject[] npcs = GameObject.FindGameObjectsWithTag(tag);

        // Marcar todas las celdas adyacentes a los NPC como visitadas y meterlas en la cola
        foreach (GameObject gObject in npcs) {
            AgentNPC npc = gObject.GetComponent<AgentNPC>();
            if (npc.agentState == State.Muerto) {
                continue;
            }
            Vector2Int celda = getCeldaDePuntoPlano(npc.Position);
            int x = celda.x;
            int y = celda.y;
            x = System.Math.Clamp(x,0,29);
            y = System.Math.Clamp(y,0,29);
            if (mapa[x, y] < npc.influencia){    
                mapa[x, y] = npc.influencia;
                if (npc.influencia > reduccion_influencia) expandir(mapa,celda, npc.influencia-reduccion_influencia);
            }
        }
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
