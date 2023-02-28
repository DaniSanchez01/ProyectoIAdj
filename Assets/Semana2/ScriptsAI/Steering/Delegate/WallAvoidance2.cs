using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAvoidance2 : SteeringBehaviour
{
    public bool gizmos = false;
    private Vector3 agentPos;
    private Vector3 collisionPos;

    // Start is called before the first frame update
    void Start()
    {
        this.nameSteering = "Wall Avoidance";

    }

    public override Steering GetSteering(Agent agent)
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
        collisionPos=collision.position;
        Vector3 rayoPenetrado = agentPos+rayVector - collisionPos;
        steer.linear = collision.normal*rayoPenetrado.magnitude*2;
        return steer;
    }

    private void OnDrawGizmos() {
        if (gizmos == true) {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(collisionPos,0.4f);
        }
    }
}
