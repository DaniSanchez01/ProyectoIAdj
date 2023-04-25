using UnityEngine;
using System.Collections.Generic;

public class GeneradorMapaInfluencias : MonoBehaviour {

    public GameObject celdaPrefab;
    public Transform mapaParent;

    private float[,] array_influencia;
    private Color[,] array_colores;

    private List<TankAgent> tanks;
    private List<SoldierAgent> soldiers;
    private List<ArcherAgent> archers;

    private void Start() {
        InvokeRepeating("GenerarMapaInfluencias", 0f, 2f);
    }

    private void GenerarMapaInfluencias() {
        // Obtener los NPCs de la escena
        tanks = new List<TankAgent>(FindObjectsOfType<TankAgent>());
        soldiers = new List<SoldierAgent>(FindObjectsOfType<SoldierAgent>());
        archers = new List<ArcherAgent>(FindObjectsOfType<ArcherAgent>());

        // Inicializar los arrays de influencia y colores
        array_influencia = new float[30, 30];
        array_colores = new Color[30, 30];

        // Calcular el valor de array_azul para cada NPC azul
        foreach (var npc in tanks) {
            int x = Mathf.RoundToInt(npc.Position.x);
            int y = Mathf.RoundToInt(npc.Position.z);

            if (npc.CompareTag("NPCazul") && 
                x >= 0 && x < 30 && y >= 0 && y < 30) {
                array_influencia[x, y] += npc.team * 1f;
            }
        }

        foreach (var npc in soldiers) {
            int x = Mathf.RoundToInt(npc.Position.x);
            int y = Mathf.RoundToInt(npc.Position.z);

            if (npc.CompareTag("NPCazul") &&
                x >= 0 && x < 30 && y >= 0 && y < 30) {
                array_influencia[x, y] += npc.team * 0.4f;
            }
        }

        foreach (var npc in archers) {
            int x = Mathf.RoundToInt(npc.Position.x);
            int y = Mathf.RoundToInt(npc.Position.z);

            if (npc.CompareTag("NPCazul") &&
                x >= 0 && x < 30 && y >= 0 && y < 30) {
                array_influencia[x, y] += npc.team * 0.3f;
            }
        }

        // Calcular el valor de array_rojo para cada NPC rojo
        foreach (var npc in tanks) {
            int x = Mathf.RoundToInt(npc.Position.x);
            int y = Mathf.RoundToInt(npc.Position.z);

            if (npc.CompareTag("NPCrojo") &&
                x >= 0 && x < 30 && y >= 0 && y < 30) {
                array_influencia[x, y] -= npc.team * 1f;
            }
        }

        foreach (var npc in soldiers) {
            int x = Mathf.RoundToInt(npc.Position.x);
            int y = Mathf.RoundToInt(npc.Position.z);

            if (npc.CompareTag("NPCrojo") &&
                x >= 0 && x < 30 && y >= 0 && y < 30) {
                array_influencia[x, y] -= npc.team * 0.4f;
            }
        }

        foreach (var npc in archers) {
            int x = Mathf.RoundToInt(npc.Position.x);
            int y = Mathf.RoundToInt(npc.Position.z);

            if (npc.CompareTag("NPCrojo") &&
                x >= 0 && x < 30 && y >= 0 && y < 30) {
                array_influencia[x, y] -= npc.team * 0.3f;
            }
        }

        // Calcular los colores para cada celda del mapa
        for (int x = 0; x < 30; x++) {
            for (int y = 0; y < 30; y++) {
                float valor = array_influencia[x, y];
                if (valor > 0) {
                    array_colores[x, y] = Color.Lerp(Color.blue, Color.white, valor);
                } else if (valor < 0) {
                    array_colores[x, y] = Color.Lerp(Color.red, Color.white, -valor);
                } else {
                    array_colores[x, y] = Color.black;
                }
            }
        }

        // Generar las celdas del mapa
        for (int x = 0; x < 30; x++) {
            for (int y = 0; y < 30; y++) {
                Vector3 posicion = new Vector3(x, 0, y);
                GameObject celda = Instantiate(celdaPrefab, posicion, Quaternion.identity, mapaParent);
                Renderer rend = celda.GetComponent<Renderer>();
                rend.material.color = array_colores[x, y];
            }
        }
    }
}
