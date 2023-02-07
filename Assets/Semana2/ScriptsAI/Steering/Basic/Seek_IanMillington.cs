using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek_IanMillington : SteeringBehaviour
{


    public virtual void Awake()
    {
        nameSteering = "Seek Ian Millingt.";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Determinar el vector de velocidad como el vector obtenido en los
        // siguientes pasos.
        //
        // 1. Calcula la diferencia de las posiciones
        // Vector3 direction = 

        // 2. Modifica el vector para que su módulo coincida con agente._maxAcceleration
        // Vector3 newAcceleration = 


        // Asignamos valores a la variable de salida
        // steer.linear = newAcceleration;  // Descomenta
        steer.angular = 0; // NO genera acleración angular


        return steer;
    }
}
