﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBehaviour
{

    public Agent target;
    public float timeToTarget = 0.1f; // Tiempo para llegar al objetivo ?

    void Start()
    {
        this.nameSteering = "Arrive";
    }

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Calcular la dirección hacia el objetivo
        Vector3 direction = target.Position - agent.transform.position; // podríamos usar la Transform del target?
        float distance = direction.magnitude;

        // Calcular la velocidad dentro del radio de llegada
        float targetSpeed = target.MaxSpeed;
        if (distance < target.arrivalRadius)
        {
            targetSpeed = target.MaxSpeed * distance / target.arrivalRadius;
        }

        // Calcular la velocidad deseada
        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        steer.linear = targetVelocity - agent.Velocity;
        steer.linear /= timeToTarget;

        if (steer.linear.magnitude > target.MaxAcceleration)
        {
            steer.linear.Normalize();
            steer.linear *= target.MaxAcceleration;
        }

        // Devolver el resultado final
        steer.angular = 0;
        return steer;
    }
}


