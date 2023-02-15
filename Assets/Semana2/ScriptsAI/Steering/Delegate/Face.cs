using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{
    public Agent FaceTarget;
    private Agent virt; //agente virtual ficticio para llevar acabo el align
    private float newOrientation; //nueva orientacion del NPC virtual

    private void Start()
    {
        base.nameSteering = "Face";
    }

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();
        //calculo el vector direccion hacia el objetivo
        Vector3 direccion = FaceTarget.Position - agent.Position;

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

        if(virt==null) //no tenemos el objetivo virtual
        {
           
            virt = FaceTarget.CreateVirtual(FaceTarget.Position); //toma radio por defecto -1
            virt.Orientation = newOrientation; //nueva orientacion del agente virtual creado
        }
        else //lo tenemos ya creado
        {
            //virt.UpdateVirtual(virt,FaceTarget.Position,newOrientation);
            virt.Position = FaceTarget.Position; //se pueden usar los metodos set directamente para actualizar
            virt.Orientation = newOrientation; //nueva orientacion
        }

        target = virt;
        return base.GetSteering(agent);

    }

   
}
