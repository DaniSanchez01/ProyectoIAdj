using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Slot {
    //npc asociado a la celda
    public AgentNPC npc;

    //posicion con respecto al lider
    public Vector3 relativePosition;

    //orientacion con respecto al lider
    public float relativeOrientation;
    
    //Indica si esta celda es la del lider
    public bool  leaderCell;

    //Agente virtual que usarán los npc para posicionarse como deben en la celda
    public Agent virtualAgent;
}

public class GridFormation: MonoBehaviour {

    //Columnas del grid
    public int numColumns;
    //Filas del grid
    public int numRows;
    //Tamaño de celda
    public float cellSize;
    //Agente lider (una vez se asigne SIEMPRE será el mismo)
    public AgentNPC leader;

    //Orientacion real del lider (su orientacion relativa será 0)
    public float leaderAngle;

    //Celdas del grid
    public Slot[,] slots; // matriz de ranuras

    //Posicion real del grid (posicion de la celda del lider)
    public Vector3 gridPosition;

    //Usado para llevar el grid a la posicion del lider al hacer Shift+F
    public bool activated = false;

    //"Constructor" del grid. Como GridManager hereda de MonoBehaviour no permite constuctores ya que esto se haría con AddComponent<>()
    //Usaremos está funcion justo después del AddComponent para preparar el grid para ser usado. 
    public void CreateGridManager(float cellSize, AgentNPC leader, int leaderI, int leaderJ,float angle, int numColumns, int numRows){
        this.activated = true;
        this.numColumns = numColumns;
        this.numRows = numRows;
        this.slots = new Slot[numColumns, numRows];
        this.cellSize = cellSize;
        this.leader = leader;
        this.leaderAngle = angle;
        this.gridPosition = leader.Position;
        //Para cada celda del grid
        for (int i = 0; i < numColumns; i++) {
            for (int j = 0; j < numRows; j++) {
                this.slots[i, j] = new Slot();
                //Calculamos la posición relativa creando un vector desde la celda del lider hasta esta celda
                this.slots[i, j].relativePosition = new Vector3(i * cellSize - leaderI*cellSize, 0f, j * cellSize - leaderJ*cellSize);
                //Creamos un virtualAgent en las posicion real del mundo donde se encuentra la celda
                this.slots[i, j].virtualAgent = Agent.CreateStaticVirtual(GridToPlane(i,j),ori:angle, paint: false);
                //Las orientaciones se especificarán más adelante, dependiendo del tipo de formación
                this.slots[i, j].relativeOrientation = 0f;
                //Si esta celda es la celda del lider
                if (leaderI == i && leaderJ == j) {
                    //Conectar el lider a esta
                    this.slots[i,j].npc = leader;
                    this.slots[i,j].leaderCell = true;
                }  
                else {
                    this.slots[i, j].npc = null;
                    this.slots[i, j].leaderCell = false;
                }
            }
        }
    }
    
    //Conectar un agente a su celda
    public void linkToSlot(int i, int j, float angle, AgentNPC npc) {
        
        this.slots[i,j].npc = npc;
        //La orientación relativa es la orientación respecto al lider (ej. lider = 50º, npc = 10º, npcRelativa = -40º)
        this.slots[i,j].relativeOrientation = Bodi.MapToRange(angle-leaderAngle,Range.grados180);
        Agent v = this.slots[i,j].virtualAgent;
        //Actualizar el agente virtual con la orientación necesaria
        v.UpdateVirtual(v.Position,ori:angle);
    }

    // Dada una posicion realtiva recibimos la posicion de la ranura en el mundo real
    public Vector3 GridToPlane(int i, int j) {
        return slots[i,j].relativePosition + gridPosition;
    }

    // Queremos saber la posicion relativa de una ranura
    public Vector3 RelativePosition(int i, int j) {
        return slots[i,j].relativePosition;
    }

    //Mover el grid a una nueva posición
    public void moveGrid(Vector3 newPosition){
        this.gridPosition = newPosition;
        //Actualizar la posición de todos los agentes virtuales (trabajan con posiciones reales)
        for (int i = 0; i < numColumns; i++) {
            for (int j = 0; j < numRows; j++) {
                Agent v = this.slots[i,j].virtualAgent;
                v.Position = GridToPlane(i,j);
            }
        }
    }

    //Devuelve la celda del lider
    public Slot getLeaderSlot() {
        for (int i = 0; i < numColumns; i++) {
            for (int j = 0; j < numRows; j++) {
                if (slots[i,j].leaderCell) return slots[i,j];
            }
        }
        return slots[0,0];

    }

    //Implementación del leaderFollowing. Cuando se mueve el grid, el lider deberá hacer 
    //un arrive a su ranura, y todos los demás npcs deberán seguir al lider
    public void leaderFollowing(){
        //Para cada celda
        for (int i = 0; i < numColumns; i++) {
            for (int j = 0; j < numRows; j++) {
                //Si hay un npc conectado a esa ranura
                if (slots[i, j].npc!= null) {

                    Arrive a;
                    Agent leaderVirtual = getLeaderSlot().virtualAgent;
                    if (!slots[i, j].npc.TryGetComponent<Arrive>(out a)) {
                        a = slots[i, j].npc.gameObject.AddComponent<Arrive>();
                    }
                    //Si eres el lider, hacer un arrive hasta la nueva posición del grid
                    if (slots[i, j].npc== leader) {
                        a.NewTarget(leaderVirtual);
                        //ESTE ESTADO SE USA EN EL ARRIVE PARA NOTIFICAR AL FORMATIONMANAGER CUANDO EL LIDER LLEGA A ESTA NUEVA POSICIÓN
                        leader.agentState = State.leaderFollowing;
                    }
                    //Si eres otro npc, hacer el arrive al lider
                    else a.NewTarget(leader);

                    //Si tienes asignado un steering Align eliminalo (para poder hacer un Face después y que no le moleste el Align)
                    if (slots[i, j].npc.TryGetComponent<Align>(out Align x))
                        DestroyImmediate (slots[i, j].npc.GetComponent<Align>());

                    
                    Face b;
                    if (!slots[i, j].npc.TryGetComponent<Face>(out b)) {
                        b = slots[i, j].npc.gameObject.AddComponent<Face>();
                    }
                    //Si eres el lider, mira hacia la nueva posicion del grid
                    if (slots[i, j].npc== leader) {
                        b.FaceNewTarget(leaderVirtual);
                    }
                    //Si eres otro npc, mira hacia el lider
                    else b.FaceNewTarget(leader);
                } 
            }
        }
        
    }

    //A esta función se le llama cuando el lider hallegado a la nueva posición del grid (Cuando entra en el radio interior del 
    //virtualAgent de su ranura, (mirar cambios en el script Arrive))
    //Esta función hace que cada npc vaya a su celda correspondiente y que se orienten como deben
    public void agentsToCell() {
        //Para cada celda
        for (int i = 0; i < numColumns; i++) {
            for (int j = 0; j < numRows; j++) {
                //Si hay un npc conectado a esa ranura
                if (slots[i, j].npc!= null) {
                    //Hacer un arrive a la celda correspondiente
                    Arrive a =slots[i, j].npc.GetComponent<Arrive>();
                    a.NewTarget(slots[i,j].virtualAgent);
                    
                    //Destruir el steering Face (para poder hacer un Align después y que no le moleste el Face)
                    DestroyImmediate(slots[i, j].npc.GetComponent<Face>());
                    
                    //Hacer un align a la celda correspondiente
                    Align b;
                    if (!slots[i, j].npc.TryGetComponent<Align>(out b)) {
                        b = slots[i, j].npc.gameObject.AddComponent<Align>();
                    }
                    b.NewTarget(slots[i,j].virtualAgent);
                } 
            }
        }
    }

    //Se borrarán de las celdas todos los npcs conectados (menos el lider)
    public void liberarAgents() {
        this.activated = false;
        for (int i = 0; i < numColumns; i++) {
            for (int j = 0; j < numRows; j++) {
                if (slots[i,j].npc!=null && !slots[i,j].leaderCell) {
                    //Cambiar el estado a normal
                    slots[i,j].npc.agentState = State.Normal;
                    slots[i,j].npc = null;
                }
            }
        }
    }
    
    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        for (int i = 1; i < numColumns; i++) {
            Vector3 start = this.GridToPlane(i,0)-new Vector3(cellSize/2,0,cellSize/2);
            Vector3 end = this.GridToPlane(i,numRows-1)+new Vector3(-cellSize/2,0,cellSize/2);
            Gizmos.DrawLine(start, end);
        }
        for (int j = 1; j < numRows; j++) {
            Vector3 start = this.GridToPlane(0,j)-new Vector3(cellSize/2,0,cellSize/2);
            Vector3 end = this.GridToPlane(numColumns-1,j)+new Vector3(cellSize/2,0,-cellSize/2);
            Gizmos.DrawLine(start, end);
        }
        bool first = true;
        Gizmos.color = Color.green;
        foreach (var slot in slots) {
            Gizmos.DrawSphere(slot.relativePosition+gridPosition,0.3f);
            if (first) {
                first = false;
                Gizmos.color = Color.blue;

            }
        }
    }
}
