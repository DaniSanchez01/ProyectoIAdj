using UnityEngine;

public class MapaDeInfluencias : MonoBehaviour
{
    public float updateInterval = 2f; // Intervalo de actualización del mapa de influencias
    public GameObject celdaPrefab; // Prefab de la celda del mapa
    public Transform mapaParent; // Padre de las celdas del mapa

    private int[,] array_azul; // Array de influencia azul
    private int[,] array_rojo; // Array de influencia rojo
    private float[,] array_influencia; // Array de influencia final
    private Color[,] array_colores; // Array de colores para las celdas del mapa

    private void Start()
    {
        // Inicializar los arrays con el tamaño del mapa
        array_azul = new int[30, 30];
        array_rojo = new int[30, 30];
        array_influencia = new float[30, 30];
        array_colores = new Color[30, 30];

        // Iniciar la actualización del mapa de influencias
        InvokeRepeating("ActualizarMapaDeInfluencias", 0f, updateInterval);
    }

    private void ActualizarMapaDeInfluencias()
    {
        // Reiniciar los valores de los arrays
        for (int x = 0; x < 30; x++) {
            for (int y = 0; y < 30; y++) {
                array_azul[x, y] = 0;
                array_rojo[x, y] = 0;
                array_influencia[x, y] = 0f;
                array_colores[x, y] = Color.black;
            }
        }

        // Obtener todos los NPCs de la escena con la etiqueta "NPCrojo" o "NPCazul"
        GameObject[] npcsAzules = GameObject.FindGameObjectsWithTag("NPCazul");
        GameObject[] npcsRojos = GameObject.FindGameObjectsWithTag("NPCrojo");

        // Calcular la posición en el mapa de cada NPC y asignarlos a un array de influencia correspondiente
        foreach (GameObject npc in npcsAzules) {
            int x = Mathf.RoundToInt(npc.Position.x);
            int y = Mathf.RoundToInt(npc.Position.z);
            if (x >= 0 && x < 30 && y >= 0 && y < 30) {
                array_azul[x, y] += 1;
            }
        }

        foreach (GameObject npc in npcsRojos) {
            int x = Mathf.RoundToInt(npc.Position.x);
            int y = Mathf.RoundToInt(npc.Position.z);
            if (
            x >= 0 && x < 30 && y >= 0 && y < 30) {
                array_rojo[x, y] += 1;
            }
        }

        // Normalizar los valores de cada array de influencia para que estén en el rango de -1 a 1
        for (int x = 0; x < 30; x++) {
            for (int y = 0; y < 30; y++) {
                array_azul[x, y] = Mathf.RoundToInt(Mathf.Clamp((float)array_azul[x, y] / npcsAzules.Length, -1f, 1f) * 10f);
                array_rojo[x, y] = Mathf.RoundToInt(Mathf.Clamp((float)array_rojo[x, y] / npcsRojos.Length, -1f, 1f) * 10f);
            }
        }

        // Restar los valores de array_rojo de array_azul para generar el array_influencia final
        for (int x = 0; x < 30; x++) {
            for (int y = 0; y < 30; y++) {
                array_influencia[x, y] = (float)array_azul[x, y] - (float)array_rojo[x, y];
            }
        }

        // Asignar un color a cada celda del mapa de influencias en función de su valor en el array_influencia final
        for (int x = 0; x < 30; x++) {
            for (int y = 0; y < 30; y++) {
                if (array_influencia[x, y] > 0f) {
                    array_colores[x, y] = Color.Lerp(Color.blue, Color.white, array_influencia[x, y] / 10f);
                } else if (array_influencia[x, y] < 0f) {
                    array_colores[x, y] = Color.Lerp(Color.red, Color.white, -array_influencia[x, y] / 10f);
                }
            }
        }

        // Crear las celdas del mapa con los colores correspondientes
        for (int x = 0; x < 30; x++) {
            for (int y = 0; y < 30; y++) {
                GameObject celda = Instantiate(celdaPrefab, new Vector3(x, 0f, y), Quaternion.identity, mapaParent);
                celda.GetComponent<Renderer>().material.color = array_colores[x, y];
            }
        }
    }
}
