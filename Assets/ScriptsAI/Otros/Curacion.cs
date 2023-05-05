using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Curacion : MonoBehaviour
{

    private List<AgentNPC> heridos;
    private List<AgentNPC> curados;
    // Start is called before the first frame update
    void Start()
    {
        heridos = new List<AgentNPC>();
        curados = new List<AgentNPC>();
        StartCoroutine(curar());
    }

    IEnumerator curar() {
        while(true) {
            foreach (var npc in heridos) {
                Debug.Log(npc.gameObject.name);
                npc.Vida+=20;
                if (npc.Vida >= npc.VidaMax) {
                    npc.Vida = npc.VidaMax;
                                    

                    curados.Add(npc);
                }
                npc.UpdateContador();
            }
            heridos = heridos.Except(curados).ToList();
            curados.Clear();
            yield return new WaitForSeconds(2);
        }
    }

    public void AnadirHerido(AgentNPC h) {
        heridos.Add(h);
    }
}
