using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAgentNPC : AgentNPC
{
    // Start is called before the first frame update
    protected override void Start()
    {
        this.MaxSpeed = 8f;
        this.MaxAcceleration = 20f;
        this.MaxRotation = 180f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 300f;
        this.interiorAngle = 8f;
        if (team == Team.Blue){
            this.Orientation = 180f;
        }
        base.Start();
    }

    // Update is called once per frame

    public override void Update()
    {
        base.Update();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }
}
