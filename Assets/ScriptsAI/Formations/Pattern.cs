using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pattern
{

    //Numero de npcs que acepta la formación
    protected int numAgents;
    //Celdas que usa la formación
    protected (int,int)[] validSlots;
    //Celda del lider
    protected (int,int) leaderSlot;
    //Orientaciones que tienen que tener los npcs de cada celda
    protected float[] relativeAngles;

    public (int,int) getLeaderSlot() {
        return leaderSlot;
    }

    public (int,int) getSlot(int numSlot) {
        return validSlots[numSlot-1];
    }

    public (int,int)[] getValidSlots() {
        return validSlots;
    }

    public bool supportAgent(int slotCount) {
        return slotCount<numAgents;
    }

    public float getAngle(int numSlot){
        return relativeAngles[numSlot];
    }
}
