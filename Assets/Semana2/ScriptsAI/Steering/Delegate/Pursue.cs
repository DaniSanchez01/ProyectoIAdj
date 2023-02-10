using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : SeekCraig
{
    
    public Agent pursueTarget; 
    public float maxPrediction;

    void Start()
    {
        this.nameSteering = "Pursue";
    }

    public override Steering GetSteering(Agent agent)
    {
        
        Steering steer = new Steering();

        // Calcula la distancia al target
        Vector3 direction = pursueTarget.Position - agent.Position;
        float distance = direction.magnitude;

        //Coge nuestra velocidad.
        float speed = agent.Velocity.magnitude;
        float prediction = 0;
        if (speed<= (distance/maxPrediction)) {
            prediction = maxPrediction;
        }
        else {
            prediction = distance / speed;
        }
        target = pursueTarget;
        target.Position += target.Velocity * prediction;

        return base.GetSteering(agent);
    }
}
