﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek_IanMillington : SteeringBehaviour
{
    public Agent target;

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
        steer.linear = target.Position - agent.Position;

        // 2. Modifica el vector para que su módulo coincida con agente._maxAcceleration
        steer.linear = steer.linear.normalized;
        steer.linear *= agent.MaxAcceleration;
        steer.angular = 0.0f;
        // Retornamos el resultado final.
        return steer;
    }
}
