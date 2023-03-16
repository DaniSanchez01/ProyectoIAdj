using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAgentNPC : AgentNPC
{


    /*
     * Este personaje representaria un personaje pesado y que por tanto tiene mucha masa y poca aceleraci�n aunque se puede seguir moviendo
     */

    // Start is called before the first frame update
    protected override void Start()
    {
        this.Mass = 5; //tiene mas masa
        this.MaxSpeed = 0.8f; // la maxima velocidad tambien se disminuye
        this.MaxRotation = 0.8f; //la maxima velocidad angular que puede tener al ser mas pesado tienes menos velocidad angular
        this.MaxAcceleration = 0.8f; //la maxima aceleracion tambien la he reducido
        this.MaxAngularAcc = 0.6f; //la maxima aceleracion agular tambien se ha reducido
        this.MaxForce = 5; //la maxima fuerza
        
    }

    // Update is called once per frame
    public  override void Update()
    {
        base.Update();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }
}
