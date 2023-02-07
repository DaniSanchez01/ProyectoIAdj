using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatching : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour

    
    void Start()
    {
        this.nameSteering = "Pon su nombre";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.

        // Retornamos el resultado final.
        return steer;
    }
}