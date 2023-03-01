using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierPlayer : AgentPlayer
{
    // Start is called before the first frame update
    void Start()
    {
        this.MaxSpeed = 23f;
        this.MaxAcceleration = 7f;
        this.MaxRotation = 20f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 20f;
        this.Orientation = 240f;
        this.arrivalRadius = 6f;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
