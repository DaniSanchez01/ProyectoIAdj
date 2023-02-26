using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAvoidance2
{
    public bool gizmos = false;
    private Vector3 agentPos;
    private Vector3 collisionPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();
        agentPos = agent.Position;
        Vector3 rayVector = agent.Velocity;
        rayVector = rayVector.normalized;
        rayVector *= agent.lookahead;

        CollisionDetector.Collision collision = CollisionDetector.getCollision(agentPos, rayVector);
        if (collision.normal == Vector3.zero) {
            return steer; 
        }
        Vector3 rayoPenetrado = agentPos+rayVector - collisionPos;
        steer.linear = collision.normal*rayoPenetrado.magnitude;
        collisionPos=collision.position;
        return steer;

    }
}
