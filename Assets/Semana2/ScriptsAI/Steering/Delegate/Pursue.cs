using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : SeekCraig
{
    
    public Agent pursueTarget; 
    private Agent virt;
    public float maxPrediction;

    private Vector3 newPosition;

    void Start()
    {
        this.nameSteering = "Pursue";
    }

    public override Steering GetSteering(Agent agent)
    {
        
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
        
        newPosition = pursueTarget.Position + pursueTarget.Velocity * prediction;
        if (virt == null) {
            virt = pursueTarget.CreateVirtual(newPosition);
        }
        else {
            pursueTarget.UpdateVirtual(virt,newPosition);
        }
        
        target = virt;
        return base.GetSteering(agent);
    }
}
