using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAgentNPC : AgentNPC
{
    // Start is called before the first frame update
    void Start()
    {
        this.MaxSpeed = 40f;
        this.MaxAcceleration = 20f;
        this.MaxRotation = 180f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 180f;
        this.interiorAngle = 8f;
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
