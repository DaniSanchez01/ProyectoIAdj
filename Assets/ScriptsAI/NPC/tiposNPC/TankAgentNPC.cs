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
        this.MaxSpeed = 5f;
        this.MaxAcceleration = 15f;
        this.MaxRotation = 140f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 180f;
        this.interiorAngle = 8f;
        if (team == Team.Blue){
            this.Orientation = 180f;
        }
        base.Start();
        
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
