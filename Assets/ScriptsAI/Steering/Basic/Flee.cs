﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    public Agent target;

    void Awake()
    {
        this.nameSteering = "Flee";
    }

    public override void NewTarget(Agent t) {
        target = t;
    }

    public override Steering GetSteering(AgentNPC agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        Vector3 desired_velocity = ( agent.Position - target.Position).normalized * agent.MaxSpeed;
        steer.linear = desired_velocity - agent.Velocity;
        steer.angular = 0.0f;
        // Retornamos el resultado final.
        return steer;
    }
}