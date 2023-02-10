using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAgentNPC : AgentNPC
{
    // Start is called before the first frame update
    void Start()
    {
        this.MaxSpeed = 16f;
        this.MaxAcceleration = 7f;
        this.MaxRotation = 50f;
        this.MaxForce = 30f;
        this._maxAngularAcc = 40f;
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
