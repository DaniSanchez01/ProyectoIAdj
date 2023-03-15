using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern: Pattern
{

    public AttackPattern() {        
        //Celda del lider
        this.leaderSlot = (1,2);
        //Celdas a usar por los npcs (no se incluye la del lider)
        validSlots = new[] {(2,1),(1,1),(0,1),(2,0),(0,0)};
        //Orientación en cada celda (se incluye la del lider)
        relativeAngles = new [] {0f,45f, 0f, -45f,45f,-45f};
        this.numAgents = 5;
    }
}
