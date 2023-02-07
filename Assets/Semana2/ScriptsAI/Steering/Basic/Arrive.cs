using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    public Agent target;
    public float timeToTarget = 0.2f;
    
    void Start()
    {
        this.nameSteering = "Arrive";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        Vector3 direction = target.Position - agent.Position;
        float distance = direction.magnitude;

        if (distance < target.interiorRadius) {
            return steer;
        }
        if (distance > target.interiorRadius)
        {
            float agentSpeeed = agent.MaxSpeed; 
        }
        else
        {

        } 
        // Retornamos el resultado final.
        return steer;
    }
}