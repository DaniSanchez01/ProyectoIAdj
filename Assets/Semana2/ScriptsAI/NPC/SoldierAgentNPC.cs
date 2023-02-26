using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAgentNPC : AgentNPC
{
    // Start is called before the first frame update
    void Start()
    {
        this.MaxSpeed = 16f;
        this.MaxAcceleration = 15f;
        this.MaxRotation = 90f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 70f;
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
