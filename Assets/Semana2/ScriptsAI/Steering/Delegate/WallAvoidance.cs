using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAvoidance : SeekCraig
{
    public bool gizmos = false;
    Agent virt;
    public float lookahead = 4f;
    public float avoidDistance = 2f;

    private Vector3 agentPos;
    private Vector3 collisionPos;

    // Start is called before the first frame update
    void Start()
    {
        virt = Agent.CreateStaticVirtual(Vector3.zero,intRadius: 0,arrRadius: 0,paint: false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override Steering GetSteering(Agent agent)
    {
        agentPos = agent.Position;
        Vector3 rayVector = agent.Velocity;
        rayVector = rayVector.normalized;
        rayVector *= lookahead;

        CollisionDetector.Collision collision = CollisionDetector.getCollision(agentPos, rayVector);
        if (collision.normal == Vector3.zero) {
            Debug.Log("Sigue");
            return base.GetSteering(agent); 
        }
        Debug.Log("Choque");
        collisionPos=collision.position;
        virt.Position = collision.position + collision.normal * avoidDistance;
        target = virt;
        return base.GetSteering(agent);

    }

    private void OnDrawGizmos() {
        if (gizmos == true) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(agentPos,collisionPos);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(collisionPos,target.Position);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target.Position,0.2f);
        }
    }
}
