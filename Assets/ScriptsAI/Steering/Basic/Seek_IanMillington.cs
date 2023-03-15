using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek_IanMillington : SteeringBehaviour
{
    public Agent target;

    public virtual void Awake()
    {
        nameSteering = "Seek Ian Millingt.";
    }


    public override Steering GetSteering(AgentNPC agent)
    {
        Steering steer = new Steering();

        // Determinar el vector de velocidad como el vector obtenido en los
        // siguientes pasos.
        //
        // 1. Calcula la diferencia de las posiciones
        Vector3 distance = target.Position - agent.Position;
        if (distance.magnitude<agent.interiorRadius) {
            steer.linear =-agent.Velocity/Time.deltaTime;
            steer.linear = Vector3.ClampMagnitude(steer.linear, agent.MaxAcceleration);
            return steer;
        }
        // 2. Modifica el vector para que su módulo coincida con agente._maxAcceleration
        steer.linear = distance.normalized;
        steer.linear *= agent.MaxAcceleration;
        steer.angular = 0.0f;
        // Retornamos el resultado final.
        return steer;
    }
}
