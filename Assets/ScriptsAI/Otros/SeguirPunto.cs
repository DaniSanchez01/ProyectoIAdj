﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * L. Daniel Hernández. 2018. Copyleft
 * 
 * Una propuesta para dar órdenes a un grupo de agentes sin formación.
 * 
 * Recursos:
 * Los rayos de Cámara: https://docs.unity3d.com/es/current/Manual/CameraRays.html
 * "Percepción" mediante Physics.Raycast: https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
 * SendMessage to external functions: https://www.youtube.com/watch?v=4j-lh3C_w1Q
 * 
 * */

public class SeguirPunto : MonoBehaviour
{
    List<GameObject> selectedUnits = new List<GameObject>();
    private Agent virt;

    public bool giz = true;

    void Start() {
        virt = Agent.CreateStaticVirtual(Vector3.zero,paint:false);
    }
    // Update is called once per frame
    void Update()
    {

        //Si mantenemos pulsado el shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //Al pulsar con el raton
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                //Si da en algo
                if (Physics.Raycast(ray, out hitInfo))
                {
                    AgentNPC a;
                    //Si es un NPC
                    if (hitInfo.transform.gameObject.TryGetComponent<AgentNPC>(out a))
                    {
                        //Si este NPC todavia no se encuentra en la lista
                        if (!selectedUnits.Contains(hitInfo.transform.gameObject))
                        {
                            //Añadirlos a la lista
                            selectedUnits.Add(hitInfo.transform.gameObject);
                            hitInfo.transform.gameObject.GetComponent<Cubo>().enable();
                        }
                    }
                }
            }
        }
        //Si estamos pulsando el espacio
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //Hacer desaparecer los cubos y borrar la lista
            foreach (var npc in selectedUnits)
            {
                npc.GetComponent<Cubo>().enable();
                AgentNPC a = npc.GetComponent<AgentNPC>();
                npc.GetComponent<Arrive>().NewTarget(a);
                
            }
            selectedUnits.Clear();
        }
        // Damos una orden cuando levantemos el botón del ratón.
        else if (Input.GetMouseButtonUp(0))
        {

            // Comprobamos si el ratón golpea a algo en el escenario.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                // Si lo que golpea es un punto del terreno entonces da la orden a todas las unidades NPC
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Floor"))
                {
                    Vector3 newTarget = hitInfo.point;
                    Agent target = virt;
                    target.Position = newTarget;
                    target.giz = this.giz;


                    /**
                     * Otra alternativa es recurrir a una lista pública de todas las unidades seleccionadas
                     *              public List<GameObject> selectedUnits;
                     * En este caso se cambiaría "listNPC" por "selectedUnits"
                     * 
                     * Dicha lista pertenecería a una clase encarga de controlar eventos generales del juego,
                     * como por ejemplo la selección de unidades. La ventaja de mantener una lista en tiempo
                     * de ejecución es obvia: Si el número de unidades es pequeño (p.e. dos) en relación con
                     * el número total de NPC (p.e. miles), pues no sería necesario que Unity busque en todos los
                     * objetos del escenario con la marca de haber sido seleccionado, 
                     * lo que facilita y agiliza algunas tareas. P.e. para realizar formaciones.
                     */

                    foreach (var npc in selectedUnits)
                    {
                        // Llama al método denominado "NewTarget" en TODOS y cada uno de los MonoBehaviour de este game object (npc)
                        //npc.SendMessage("NewTarget", newTarget);
                        npc.GetComponentInParent<Arrive>().NewTarget(target);
                        // Se asume que cada NPC tiene varias componentes scripts (es decir, varios MonoBehaviour).
                        // En algunos de esos scripts está la función "NewTarget(Vector3 target)"
                        // Dicha función contendrá las instrucciones necesarias para ir o no al nuevo destino.
                        // P.e. Dejar lo que esté haciendo y  disparar a target.
                        // P.e. Si no tengo vida suficiente huir de target.
                        // P.e. Si fui seleccionado en una acción anterio y estoy a la espera de nuevas órdenes, entonces hacer un Arrive a target.

                        // Nota1: En el caso de que tu objeto tenga una estructura jerárquica, 
                        // y se quiera invocar a NewTarget de todos sus hijos, deberás usar BroadcastMessage.

                        // Nota 2: En el caso de que solo se tenga una función "NewTarget" para cada NPC, entonces 
                        // puede ser más eficiente algo como:
                        //                  npc.GetComponent<ComponenteScriptConteniendoLaFuncion>().NewTarget(newTarget);
                        // que obtiene la componente del NPC que yo sé que contiene a la función NewTarget(), y la invoca.
                    }
                }
            }
        }
    }
}
