using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekCraig : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    public Agent target;
    
    void Start()
    {
        this.nameSteering = "Seek Craig";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        Vector3 distance = target.Position - agent.Position;
        if (distance.magnitude<agent.interiorRadius) {
            agent.Velocity = Vector3.zero;
            return steer;
        }
        Vector3 desired_velocity = distance.normalized * agent.MaxSpeed;
        steer.linear = desired_velocity - agent.Velocity;
        steer.angular = 0.0f;
        // Retornamos el resultado final.
        return steer;
    }
}