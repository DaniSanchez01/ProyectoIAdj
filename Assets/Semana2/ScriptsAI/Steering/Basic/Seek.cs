using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    public Agent target;
    
    void Start()
    {
        this.nameSteering = "Seek";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        steer.linear = target.Position - agent.Position;
        steer.linear = steer.linear.normalized;
        steer.linear *= agent.MaxAcceleration;
        steer.angular = 0.0f;
        // Retornamos el resultado final.
        return steer;
    }
}