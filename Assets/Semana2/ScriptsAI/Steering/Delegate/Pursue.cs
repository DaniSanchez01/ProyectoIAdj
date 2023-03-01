using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : SeekCraig
{
    
    public Agent pursueTarget; 
    private Agent virt;
    public float maxPrediction = 0.6f;

    private Vector3 newPosition;

    public bool giz = false;

    void Start()
    {
        this.nameSteering = "Pursue";
        virt = Agent.CreateStaticVirtual(Vector3.zero,paint:giz);
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
         
        virt.Position = newPosition;
        virt.giz = this.giz;       
        target = virt;
        return base.GetSteering(agent);
    }
}
