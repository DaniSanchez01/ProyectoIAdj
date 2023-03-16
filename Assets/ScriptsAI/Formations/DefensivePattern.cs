using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensivePattern: Pattern
{
    public DefensivePattern() {
        //Celda del lider
        leaderSlot = (1,2);
        //Celdas a usar por los npcs (no se incluye la del lider)
        //validSlots = new[] {(2,1),(0,1),(1,0)};
        validSlots = new[] {(1,3),(3,2),(0,2),(3,1),(0,1),(2,0),(1,0)};
        //Orientación en cada celda (se incluye la del lider)
        //relativeAngles = new [] {0f,90f, -90f, 180f};
        relativeAngles = new [] {0f,0f, 45f, -45f,135f,-135f,180f,180f};
        this.numAgents = 8;
    }
    
}

