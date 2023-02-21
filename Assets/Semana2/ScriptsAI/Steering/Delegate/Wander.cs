using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Face
{
    // Suponemos un circulo
    public float wanderRadius = 5.0f;
    public Vector3 wanderOffset = new Vector3(1.0f,0f,1.0f);
    // Establece nuestro máximo, en que rango se va a ir moviendo
    public float wanderRate = 0.05f;
    // Orientacion del target
    private float wanderOrientation;
    public float maxAcceleration = 0.01f;

    private void Start()
    {
        this.nameSteering = "Wander";
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
        Vector3 targetPosition = agent.Position + wanderOffset; // * new Vector3(Mathf.Cos(agent.Orientation), 0f, Mathf.Sin(agent.Orientation));
        targetPosition += wanderRadius * new Vector3(Mathf.Cos(targetOrientation), 0f, Mathf.Sin(targetOrientation));

        // Face
        FaceTarget.Position = targetPosition;
        steering = base.GetSteering(agent);

        // maxAcceleration en la orientación del agente
        steering.linear = new Vector3(maxAcceleration * Mathf.Cos(agent.Orientation), 0f, maxAcceleration * Mathf.Sin(agent.Orientation));

        return steering;
    }
}
