using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{
    [SerializeField] public Agent FaceTarget;
    [SerializeField] protected Agent virt; //agente virtual ficticio para llevar acabo el align
    [SerializeField] private float newOrientation; //nueva orientacion del NPC virtual
    [SerializeField] public PathFollowingNoOffset path = null;
    [SerializeField] private bool listoParaSteering = false; //variable que se usa para saber si el steering Face esta listo para dar un valor o no
    [SerializeField] public bool giz = false;

    Vector3 agentPos;
    Vector3 direction;


    private void Awake()
    {
        base.nameSteering = "Face";
        virt = Agent.CreateStaticVirtual(Vector3.zero,paint:giz); //toma radio por defecto -1
        virt.Orientation = newOrientation; //nueva orientacion del agente virtual creado
        listoParaSteering = true; //hemos hecho la inicialización del Face y ahora esta listo para dar un steering.
    }

    public override void DestroyVirtual(Agent first) {
        if (virt!=first) {
            Destroy(virt.gameObject);
        }
    }

    public void Update() {
        if (path!=null) {
            if (path.getTarget()!=null) {
                this.FaceTarget = path.getTarget();
            }
        }
    }
    public void FaceNewTarget(Agent t) {
        this.FaceTarget = t;
    }

    public override Steering GetSteering(AgentNPC agent)
    {
        if (listoParaSteering) //solo ejecutamos el steering si sabemos que ha ejecutado awake() 
        {
            Steering steer = new Steering();
            //calculo el vector direccion hacia el objetivo
            Vector3 direccion = FaceTarget.Position - agent.Position;
            direction = direccion;
            agentPos = agent.Position;

            //pendiente no estoy seguro de si seria esto
            if (direccion.magnitude == 0)
            {
                steer.linear = Vector3.zero;
                steer.angular = 0f;
                return steer;
            }

            //positionToAngle deberia ser mejor un metodo estatico
            newOrientation = agent.PositionToAngle(direccion); //se obtiene el angulo respecto
                                                               //al ejeZ del vector va del agente al target
            if (direccion.x < 0) newOrientation = -newOrientation;
            
            virt.Position = agent.Position; //se pueden usar los metodos set directamente para actualizar
            virt.Orientation = newOrientation; //nueva orientacion
            virt.giz = this.giz;


            target = virt;
            return base.GetSteering(agent);
        }
        else return new Steering(); //devuelve un steering que no va a hacer cambios en el movimiento en el frame actual pues el face aun no esta listo pero lo estara en el siguiente frame

    }

    public void OnDrawGizmos() {

    if (giz == true) {        
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(agentPos, agentPos+direction);
            
    }
    }

   
}
