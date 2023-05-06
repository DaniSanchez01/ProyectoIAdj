using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatching : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    public Agent target;
    //tiempo hacia el objetivo, recordamos que a valores mas altos aceleraciones/desaceleraciones
    //mas suaves y a mas bajos mas bruscas
    public float timetoTarget = 0.1f;
    
    
    void Awake()
    {
        this.nameSteering = "VelocityMatching";
    }

    public override void NewTarget(Agent t)
    {
        target = t;
    }

    public override Steering GetSteering(AgentNPC agent)
    {

        Steering steer = new Steering();
        // Calcula el steering.
        steer.linear = target.Velocity - agent.Velocity;
        steer.linear = steer.linear / timetoTarget;

        if(steer.linear.magnitude > agent.MaxAcceleration)
        {
            steer.linear = steer.linear.normalized * agent.MaxAcceleration;
        }

        steer.angular = 0;
        // Retornamos el resultado final.
        return steer;
    }
}