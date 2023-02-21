using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBehaviour
{
    public Vector3 targetPosition; // El objetivo hacia el que nos estamos acercando
    public float maxSpeed; // Velocidad máxima que puede tener el agente
    public float arrivalRadius; // Radio de satisfacción
    public float timeToTarget = 0.1f; // Tiempo para llegar al objetivo ?
    public float maxAcceleration; // Aceleración máxima que puede tener el agente

    public Agent target;
    public float timeToTarget = 0.1f; // Tiempo para llegar al objetivo ?


    void Start()
    {
        this.nameSteering = "Arrive";
    }

    public void NewTarget(Agent t) {
        target = t;
    }

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Calcular la dirección hacia el objetivo
        Vector3 direction = target.Position - agent.Position; // podríamos usar la Transform del target?
        float distance = direction.magnitude;

        // Calcular la velocidad dentro del radio de llegada
        if (distance < target.interiorRadius){
            steer.linear =-agent.Velocity/Time.deltaTime;
            return steer;

        }
        float targetSpeed;
        
        if (distance > target.arrivalRadius){
            targetSpeed = agent.MaxSpeed;
        }
        else {
            targetSpeed = agent.MaxSpeed * distance / (target.arrivalRadius);
        }
        

        // Calcular la velocidad deseada
        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        steer.linear = targetVelocity - agent.Velocity;
        steer.linear /= timeToTarget;

        

        if (steer.linear.magnitude > agent.MaxAcceleration)
        {
            steer.linear.Normalize();
            steer.linear *= agent.MaxAcceleration;

        }

        // Devolver el resultado final
        steer.angular = 0;
        return steer;
    }
}


