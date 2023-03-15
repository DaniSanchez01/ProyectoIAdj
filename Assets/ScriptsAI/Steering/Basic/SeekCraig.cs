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


    public override Steering GetSteering(AgentNPC agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        Vector3 distance = target.Position - agent.Position;
        if (distance.magnitude<agent.interiorRadius) {
            steer.linear =-agent.Velocity/Time.deltaTime;
            steer.linear = Vector3.ClampMagnitude(steer.linear, agent.MaxAcceleration);
            return steer;
        }
        Vector3 desired_velocity = distance.normalized * agent.MaxSpeed;
        steer.linear = desired_velocity - agent.Velocity;
        steer.angular = 0.0f;
        // Retornamos el resultado final.
        return steer;
    }
}