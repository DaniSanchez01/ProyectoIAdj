using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAgentNPC : AgentNPC
{
    // Start is called before the first frame update
    protected override void Start()
    {
        this.MaxSpeed = 40f;
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
    public override void determineMaxSpeedTerrain() {
    }
    public override float getTerrainCost(Nodo a) {
        return 1;
    }
    // Update is called once per frame

protected override int calculateDamage() {
        return baseDamage;
    }
    public override void Update()
    {
        base.Update();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }
}
