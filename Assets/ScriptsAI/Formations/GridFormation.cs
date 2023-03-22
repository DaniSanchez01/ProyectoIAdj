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

    public FormationManager formation;

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
        this.formation = FindObjectOfType<FormationManager>();
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
                    this.slots[i,j].npc = this.leader;
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
                if (this.slots[i,j].npc!=null) {
                    this.slots[i,j].npc.NoWait();
                }

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
                    slots[i, j].npc.changeArbitro(typeArbitro.Perseguidor);
                    Agent leaderVirtual = getLeaderSlot().virtualAgent;
                    Arrive a = (Arrive) slots[i, j].npc.takeSteering("Arrive");
                    Face f = (Face) slots[i, j].npc.takeSteering("Face");

                    if (slots[i, j].npc== leader) {
                        a.NewTarget(leaderVirtual);
                        f.FaceNewTarget(leaderVirtual);
                        slots[i,j].npc.agentState = State.leaderFollowing;
                    }
                    else {
                        a.NewTarget(leader);
                        f.FaceNewTarget(leader);
                    }
                } 
            }
        }
        formation.startTimer();

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
                    slots[i, j].npc.changeArbitro(typeArbitro.Posicionar);
                    Agent leaderVirtual = getLeaderSlot().virtualAgent;
                    Arrive a = (Arrive) slots[i, j].npc.takeSteering("Arrive");
                    Align al = (Align) slots[i, j].npc.takeSteering("Align");

                    if (slots[i, j].npc== leader) {
                        a.NewTarget(leaderVirtual);
                        al.NewTarget(leaderVirtual);
                    }
                    else {
                        a.NewTarget(slots[i, j].virtualAgent);
                        al.NewTarget(slots[i, j].virtualAgent);
                    }
                }
                
            }
        }
        formation.startTimer();
    }


    public void pathfinding(typeHeuristica heur, int prof) {
        //Para cada celda
        for (int i = 0; i < numColumns; i++) {
            for (int j = 0; j < numRows; j++) {
                //Si hay un npc conectado a esa ranura
                if (slots[i, j].npc!= null) {
                    if (!slots[i, j].npc.gameObject.TryGetComponent<GridPathFinding>(out GridPathFinding gridPath)){
                        gridPath = slots[i, j].npc.gameObject.AddComponent<GridPathFinding>();
                        gridPath.inicializarGrid(19,19,3,heur,true);
                    }
                    slots[i, j].npc.agentState = State.LRTA;
                    Vector2Int celdaPosicion = gridPath.getCeldaDePuntoPlano(slots[i, j].npc.Position);
                    Nodo nodoPosicion = gridPath.GetNodo(celdaPosicion.x,celdaPosicion.y);

                    Vector2Int celdaObjetivo = gridPath.getCeldaDePuntoPlano(slots[i,j].virtualAgent.Position);
                    Nodo nodoObjetivo = gridPath.GetNodo(celdaObjetivo.x,celdaObjetivo.y);
                    PathFinding algorithm= new PathFinding(gridPath,nodoPosicion,nodoObjetivo,slots[i, j].npc, prof, false);
                    algorithm.LRTA();
                }
            }
        }
    }

    public void LRTAtoCell(AgentNPC npc) {
        for (int i = 0; i < numColumns; i++) {
            for (int j = 0; j < numRows; j++) {
                if (slots[i, j].npc== npc) {
                    slots[i, j].npc.changeArbitro(typeArbitro.Posicionar);
                    Agent leaderVirtual = getLeaderSlot().virtualAgent;
                    Arrive a = (Arrive) slots[i, j].npc.takeSteering("Arrive");
                    Align al = (Align) slots[i, j].npc.takeSteering("Align");
                    a.NewTarget(slots[i, j].virtualAgent);
                    al.NewTarget(slots[i, j].virtualAgent);
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
                    slots[i,j].npc = null;
                }
                if (slots[i,j].npc== leader) {
                    leader.agentState = State.Normal;
                    leader.changeArbitro(leader.firstArbitro);
                    slots[i,j].npc.agentState = State.Normal;
                    slots[i,j].npc.changeArbitro(slots[i,j].npc.firstArbitro);

                }
            }
        }
    }

    public void leaderWander() {
        for (int i = 0; i < numColumns; i++) {
            for (int j = 0; j < numRows; j++) {
                //Si hay un npc conectado a esa ranura
                if (slots[i, j].npc!= null) {
                    

                    if (slots[i, j].npc== leader) {
                        slots[i, j].npc.changeArbitro(typeArbitro.Aleatorio);
                    }
                    else {
                        slots[i, j].npc.changeArbitro(typeArbitro.Perseguidor);
                        Arrive a = (Arrive) slots[i, j].npc.takeSteering("Arrive");
                        Face f = (Face) slots[i, j].npc.takeSteering("Face");
                        a.NewTarget(leader);
                        f.FaceNewTarget(leader);
                    }
                } 
            }
        }
        formation.startTimer();

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
