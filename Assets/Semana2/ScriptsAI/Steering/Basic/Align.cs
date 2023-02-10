using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    public Agent target;
    public float timeToTarget = 0.3f;
    
    void Start()
    {
        this.nameSteering = "Align";
    }


    public override Steering GetSteering(Agent agent)
    {
        float targetRotation = 0f;
        Steering steer = new Steering();
        
        // Calcula la diferencia entre la orientacion del target y el npc
        float rotation = target.Orientation - agent.Orientation;
        // Pasan esta diferencia al intervalo (-180,180)
        rotation = Bodi.MapToRange(rotation,Range.grados180);

        //El tamaño será este valor en positivo
        float rotationSize = Mathf.Abs(rotation);

        //Si la diferencia entre las orientaciones es menor que el angulo interior del target
        if (rotationSize<target.interiorAngle) {
            agent.Rotation = 0f;
            return steer;
        }

        //Si la diferencia entre las orientaciones es mayor que el angulo exterior del target
        if (rotationSize > target.exteriorAngle){
            //Rotar a la mayor velocidad posible
            targetRotation = agent.MaxRotation;
        }
        //Si estamos entre el angulo exterior y el interior
        else {
            //Calculas porcentualmente la velocidad (Maxima rotacion * tamaño de la diferencia / )
            targetRotation = agent.MaxRotation * rotationSize / target.exteriorAngle;
        }

        //Si el angulo de diferencia era negativo, rotar hacia el otro lado
        targetRotation *= rotation/rotationSize;

        //Calculamos la aceleracion angular que debemos imponer
        steer.angular = (targetRotation - agent.Rotation)/timeToTarget;

        //Comprobamos que la aceleracion no sea demasiado grande
        if (Mathf.Abs(steer.angular) > agent.MaxAngularAcc) {
            steer.angular /= Mathf.Abs(agent.AngularAcc);
            steer.angular *= agent.MaxAngularAcc;
        }

        steer.linear = Vector3.zero;
        // Retornamos el resultado final.
        return steer;
    }
}