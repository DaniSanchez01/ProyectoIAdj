using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Face
{
    // Suponemos un circulo
    public float wanderRadius = 2f;
    public float wanderOffset = 5f;
    // Establece nuestro máximo, en que rango se va a ir moviendo
    public float wanderRate =5f;
    // Orientacion del target
    float wanderOrientation = 0f;
    private Agent virtWander; //agente virtual ficticio para llevar acabo el align


    Vector3 centerCircle = Vector3.zero;
    Vector3 offsetLine = Vector3.zero;
    private void Start()
    {
        this.nameSteering = "Wander";
        FaceTarget = Agent.CreateStaticVirtual(Vector3.zero,intRadius: 0,arrRadius: 1,paint: false);
    }

    // Valor aleatorio entre -1 y 1
    private float RandomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f); // Quizá se podría hacer entre 0 y 1
    }

    public override Steering GetSteering(Agent agent)
    {
        Steering steering = new Steering();

        // Actualizamos la dirección del Wander con un valor aleatorio
        wanderOrientation += RandomBinomial() * wanderRate;
        float targetOrientation = wanderOrientation + agent.Orientation;

        // Calculamos targetPosition
        offsetLine = wanderOffset * agent.OrientationToVector(agent.Orientation);
        centerCircle = agent.Position + offsetLine;
        Vector3 targetPosition = centerCircle + wanderRadius * agent.OrientationToVector(targetOrientation);

        // Face
        FaceTarget.Position = targetPosition; 
        steering = base.GetSteering(agent);

        // maxAcceleration en la orientación del agente
        steering.linear = agent.MaxAcceleration * agent.OrientationToVector(agent.Orientation);

        return steering;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(centerCircle, wanderRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(FaceTarget.Position, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(centerCircle,centerCircle-offsetLine);

    }
}
