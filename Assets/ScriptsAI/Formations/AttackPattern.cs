using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern: Pattern
{

    public AttackPattern() {        
        //Celda del lider
        this.leaderSlot = (2,1);
        //Celdas a usar por los npcs (no se incluye la del lider)
        //validSlots = new[] {(2,1),(1,1),(0,1),(2,0),(0,0)};
        validSlots = new[] {(1,3),(2,3),(3,2),(2,2),(1,2),(0,2),(1,1),(2,0),(1,0)};
        //Orientación en cada celda (se incluye la del lider)
        relativeAngles = new [] {0f,30f, -30f, -30f,0f,0f,30f,0f,60f,-60f};
        this.numAgents = 10;
    }
}
