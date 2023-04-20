using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAvoidance : SeekCraig
{
    public bool gizmos = false;
    Agent virt;
    private Vector3 agentPos;
    private Vector3 collisionPos;
    public int nBigotes = 3;

    Vector3 bigoteCentral;
    Vector3 bigoteIzq;
    Vector3 bigoteDer;
    private bool listoParaSteering = false; //variable que se usa para saber si el steering WallAvoidance esta listo para dar un valor o no
    // Start is called before the first frame update
    void Awake()
    {
        nameSteering = "WallAvoidance";
        virt = Agent.CreateStaticVirtual(Vector3.zero,intRadius: 0.2f,arrRadius: 0.2f,paint: false);
        listoParaSteering = true;
    }

    public override void DestroyVirtual(Agent first) {
        if (virt!=first) {
            Destroy(virt.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public override Steering GetSteering(AgentNPC agent)
    {
        if (listoParaSteering)
        {
            Steering steer = new Steering();
            agentPos = agent.Position;
            //El bigote central tendrá la direccion de la velocidad. Se le pone el tamano especificado por lookahead
            bigoteCentral = agent.Velocity;
            bigoteCentral = bigoteCentral.normalized;
            bigoteCentral *= agent.lookahead;
            //Si el npc está parado, no hacer nada
            if (bigoteCentral == Vector3.zero)
            {
                bigoteIzq = Vector3.zero;
                bigoteDer = Vector3.zero;
                return steer;
            }
            //Si está en movimiento
            else
            {
                if (nBigotes == 1)
                {
                    bigoteIzq = Vector3.zero;
                    bigoteDer = Vector3.zero;
                    steer = CollisionUnBigote(agent);

                }
                else if (nBigotes == 2)
                {
                    DosBigotes(agent);
                    steer = CollisionDosBigotes(agent);

                }
                else
                {
                    TresBigotes(agent);
                    steer = CollisionTresBigotes(agent);
                }
                return steer;
            }
        }
        else return new Steering();
    }

    private Steering CollisionUnBigote(AgentNPC agent) {
        //Calcula la colisión del bigote izquierdo
        CollisionDetector.Collision collision = CollisionDetector.getCollision(agentPos, bigoteCentral);
        //Si no detecta colision...
        if (collision.normal == Vector3.zero) {
            return new Steering();
        }
        else {
            collisionPos=collision.position;
            //Calculo de la nueva posicion que deberemos seguir
            virt.Position = collision.position + collision.normal * agent.avoidDistance;
            target = virt;
            //Calcular el seek a partir de este
            return base.GetSteering(agent);
        }
    }
    private Steering CollisionDosBigotes(AgentNPC agent) {
        //Calcula la colisión del bigote izquierdo
        CollisionDetector.Collision collision = CollisionDetector.getCollision(agentPos, bigoteIzq);
        //Si no detecta colision...
        if (collision.normal == Vector3.zero) {
            //Calcula la colisión del bigote derecho
            collision = CollisionDetector.getCollision(agentPos, bigoteDer);
            //Si no detecta colision...
            if (collision.normal == Vector3.zero) {
                return new Steering();
            }
            else {
                collisionPos=collision.position;
                //Calculo de la nueva posicion que deberemos seguir
                virt.Position = collision.position + collision.normal * agent.avoidDistance;
            }
        }
        else {
            collisionPos=collision.position;
            //Calculo de la nueva posicion que deberemos seguir
            virt.Position = collision.position + collision.normal * agent.avoidDistance;
        }
        target = virt;
        //Calcular el seek a partir de este
        return base.GetSteering(agent);
    }

    private Steering CollisionTresBigotes(AgentNPC agent) {
        //Calcula la colisión del bigote izquierdo
        CollisionDetector.Collision collision = CollisionDetector.getCollision(agentPos, bigoteIzq);
        //Si no detecta colision...
        if (collision.normal == Vector3.zero) {
            //Calcula la colisión del bigote derecho
            collision = CollisionDetector.getCollision(agentPos, bigoteDer);
            //Si no detecta colision...
            if (collision.normal == Vector3.zero) {
                //Calcula la colision del bigote central
                collision = CollisionDetector.getCollision(agentPos, bigoteCentral);
                //Si no detecta colision de ninguno de los tres bigotes se devuelve un steering vacio
                if (collision.normal == Vector3.zero) {
                    return new Steering();
                }
                else {
                    collisionPos=collision.position;
                    //Calculo de la nueva posicion que deberemos seguir
                    virt.Position = collision.position + collision.normal * agent.avoidDistance;
                }
            }
            else {
                collisionPos=collision.position;
                //Calculo de la nueva posicion que deberemos seguir
                virt.Position = collision.position + collision.normal * agent.avoidDistance;
            }
        }
        else {
            collisionPos=collision.position;
            //Calculo de la nueva posicion que deberemos seguir
            virt.Position = collision.position + collision.normal * agent.avoidDistance;
        }
        target = virt;
        //Calcular el seek a partir de este
        return base.GetSteering(agent);
    }
        
        

    

    private void DosBigotes(AgentNPC agent) {
        float velocityOrientation = agent.PositionToAngle(agent.Velocity); 
        if (agent.Velocity.x < 0) velocityOrientation = -velocityOrientation;
        float bigoteizqOrientation = Bodi.MapToRange(velocityOrientation-15,Range.grados180);
        float bigotederOrientation = Bodi.MapToRange(velocityOrientation+15,Range.grados180);
        bigoteIzq = agent.OrientationToVector(bigoteizqOrientation) * (agent.lookahead);
        bigoteDer = agent.OrientationToVector(bigotederOrientation) * (agent.lookahead);
        bigoteCentral = Vector3.zero;
    }

    private void TresBigotes(AgentNPC agent) {
        float velocityOrientation = agent.PositionToAngle(agent.Velocity); 
        if (agent.Velocity.x < 0) velocityOrientation = -velocityOrientation;
            float bigoteizqOrientation = Bodi.MapToRange(velocityOrientation-30,Range.grados180);
            float bigotederOrientation = Bodi.MapToRange(velocityOrientation+30,Range.grados180);
            bigoteIzq = agent.OrientationToVector(bigoteizqOrientation) * (agent.lookahead/2.5f);
            bigoteDer = agent.OrientationToVector(bigotederOrientation) * (agent.lookahead/2.5f);
    }

    private void OnDrawGizmos() {
        if (gizmos == true) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(agentPos,collisionPos);
            Gizmos.color = Color.yellow;
            if (target!= null) Gizmos.DrawLine(collisionPos,target.Position);
            Gizmos.color = Color.red;
            if (target!= null) Gizmos.DrawSphere(target.Position,0.2f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(agentPos,agentPos+bigoteCentral);
            Gizmos.DrawLine(agentPos,agentPos+bigoteIzq);
            Gizmos.DrawLine(agentPos,agentPos+bigoteDer);



        }
    }
}
