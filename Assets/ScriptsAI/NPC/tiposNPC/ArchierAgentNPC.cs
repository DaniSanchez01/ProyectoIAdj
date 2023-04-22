using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchierAgentNPC : AgentNPC
{
    // Start is called before the first frame update
    protected override void Start()
    {
        this.MaxSpeed = 10f;
        this.MaxAcceleration = 30f;
        this.MaxRotation = 180f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 300f;
        this.interiorAngle = 8f;
        if (team == Team.Blue){
            this.Orientation = 180f;
        }
        base.Start();
    }

    public override float getTerrainCost(Nodo a) {
            
            TypeTerrain t = GameObject.FindObjectOfType<TerrainMap>().getTerrenoCasilla(a.Celda.x,a.Celda.y);
            switch (t) {
                case (TypeTerrain.camino):
                    return 1;
                case (TypeTerrain.llanura):
                    return 2;
                case (TypeTerrain.bosque):
                    return 1;
                case (TypeTerrain.desierto):
                    return 5;
                default:
                    return 9999;             
            }

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
