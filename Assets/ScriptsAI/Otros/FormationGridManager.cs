using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Slot {
    public Agent npc;
    public Vector3 relativePosition;
    public float relativeOrientation;
}

public class FormationGridManager : MonoBehaviour {

    public int numColumns;
    public int numRows;
    public float cellSize;
    public Agent leader; // leader.Position será (0,0) es decir el origin del grid
    public Slot[,] slots; // matriz de ranuras

    public FormationGridManager(float cellSize, Agent leader, AgentNPC[] allAgents){
        this.slots = new Slot[numColumns, numRows];
        this.cellSize = cellSize;
        this.leader = leader;
        int numElements = allAgents.Length;
        this.numColumns = (int)Math.Ceiling(Math.Sqrt(numElements));
        this.numRows = (int)Math.Ceiling((double)numElements / numColumns);
        int index = 0;
        for (int i = 0; i < numColumns; i++) {
            for (int j = 0; j < numRows; j++) {
                
                if (index < numElements){
                    this.slots[i, j] = new Slot();
                    this.slots[i, j].relativePosition = new Vector3(i * cellSize, 0f, j * cellSize);
                    this.slots[i, j].npc = allAgents[index];
                    this.slots[i, j].relativeOrientation = leader.Orientation + slots[i, j].npc.Orientation; // como se calcula relative Orientation?
                }
                else {
                    this.slots[i, j] = new Slot();
                    this.slots[i, j].relativePosition = new Vector3(0f, 0f, 0f);
                    this.slots[i, j].npc = null;
                    this.slots[i, j].relativeOrientation = 0f; // como se calcula relative Orientation?
                }
            }
            index++;
        }
    }
    // No se si es necesario usar slots[i, j] en las siguientes funciones?
    // Dada una posicion en el grid recibimos la posicion de la ranura en el mundo real
    public Vector3 GridToPlane(int i, int j) {
        // return slots[i, j].relativePosition + leader.Position; 
        return new Vector3(i * cellSize, 0f, j * cellSize) + leader.Position;
    }

    // Nos indica donde está la posicion (x,0,z) en el grid
    public Vector3 PlaneToGrid(Vector3 position) {
        int i = Mathf.FloorToInt((position.x - leader.Position.x) / cellSize);
        int j = Mathf.FloorToInt((position.z - leader.Position.z) / cellSize);
        return new Vector3(i, 0f, j);
    }

    // Queremos saber el vector rs-pl, es decir posicion relativa de una ranura
    public Vector3 RelativePosition(Vector3 position) {
        return position - leader.Position; // habría que hacer un?? // new Vector3(position.x - leader.Position.x, 0f, position.z - leader.Position.z)
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        for (int i = 0; i <= numColumns; i++) {
            Vector3 start = new Vector3(i * cellSize, 0, 0) + leader.Position;
            Vector3 end = new Vector3(i * cellSize, 0, numRows * cellSize) + leader.Position;
            Gizmos.DrawLine(start, end);
        }
        for (int j = 0; j <= numRows; j++) {
            Vector3 start = new Vector3(0, 0, j * cellSize) + leader.Position;
            Vector3 end = new Vector3(numColumns * cellSize, 0, j * cellSize) + leader.Position;
            Gizmos.DrawLine(start, end);
        }
    }
}
