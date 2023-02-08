using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    public Agent target;
    public float timeToTarget;
    
    void Start()
    {
        this.nameSteering = "Align";
    }


    public override Steering GetSteering(Agent agent)
    {
        float targetRotation = 0f;
        Steering steer = new Steering();
        targetRotation = target.PositionToAngle(target.Position-agent.Position);
        Debug.Log(targetRotation);

        // Calcula el steering.
        /*float rotation = target.Orientation - agent.Orientation;
        rotation = Bodi.MapToRange(rotation,Range.grados180);

        float rotationSize = Mathf.Abs(rotation);

        if (rotationSize<target.interiorAngle) {
            return steer;
        }

        if (rotationSize > target.exteriorAngle){
            targetRotation = agent.MaxRotation;
        }
        else {
            targetRotation = agent.MaxRotation * rotationSize / target.arrivalRadius;
        }

        //Se multipla por +-1
        targetRotation *= rotation/rotationSize;

        steer.angular = (targetRotation - agent.Rotation)/timeToTarget;

        if (Mathf.Abs(steer.angular) > agent.MaxAngularAcc) {
            steer.angular /= Mathf.Abs(steer.angular);
            steer.angular += agent.MaxAngularAcc;
        }

        steer.linear = Vector3.zero;
        // Retornamos el resultado final.*/
        return steer;
    }
}