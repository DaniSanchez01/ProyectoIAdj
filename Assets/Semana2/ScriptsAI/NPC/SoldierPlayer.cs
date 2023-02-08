using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierPlayer : AgentPlayer
{
    // Start is called before the first frame update
    void Start()
    {
        this.MaxSpeed = 30f;
        this.MaxAcceleration = 5f;
        this.MaxRotation = 20f;
        this.MaxForce = 30f;
        this._maxAngularAcc = 20f;
        this.Orientation = 240f;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
