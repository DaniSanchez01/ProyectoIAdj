using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typePath {
    pruebaPathFollowing,
    pruebaPathFollowing2,
    rodeaCasasAzules,
    rodeaCasasRojas,
    patrullaPuenteAzul,
    patrullaPuenteRojo,
    liderAzul,
    liderRojo,
}

public class PathFollowingNoOffset : SeekCraig
{

    List<Vector3> path = new List<Vector3>();
    List<Agent> nodes = new List<Agent>(); 
    int pathDir = 1;
    int currentNode = 0;
    public int mode = 1;
    public bool giz = true;
    public float intRadius = 1.5f;
    public typePath type;


    // Start is called before the first frame update
    void Awake()
    {
        base.nameSteering = "PathFollowing";
        setTypePath(type);
    }

    public void setPath(List<Vector3> newPath) {
        path = newPath;
        DestroyPath();
    }

    public void setTypePath(typePath typePath) {
        type= typePath;
        path.Clear();
        switch (type) {
            case typePath.pruebaPathFollowing:
                path.Add(new Vector3(30f,0f,38f));
                path.Add(new Vector3(13f,0f,38f));
                path.Add(new Vector3(13f,0f,17f));
                path.Add(new Vector3(30f,0f,4.5f));
                path.Add(new Vector3(0f,0f,0f));
                path.Add(new Vector3(2f,0f,45f));
                break;
            case typePath.pruebaPathFollowing2:
                path.Add(new Vector3(23f,0f,32f));
                path.Add(new Vector3(23f,0f,46f));
                path.Add(new Vector3(40f,0f,46f));
                path.Add(new Vector3(40f,0f,32f));
                break;
            case typePath.rodeaCasasAzules:
                path.Add(new Vector3(10f,0f,54f));
                path.Add(new Vector3(2.5f,0f,60f));
                path.Add(new Vector3(10f,0f,64.5f));
                path.Add(new Vector3(19f,0f,59f));
                path.Add(new Vector3(28f,0f,50f));
                path.Add(new Vector3(36f,0f,57.5f));
                path.Add(new Vector3(29f,0f,63f));
                path.Add(new Vector3(19f,0f,59f));
                break;

            case typePath.rodeaCasasRojas:
                path.Add(new Vector3(10f,0f,16f));
                path.Add(new Vector3(2.5f,0f,9f));
                path.Add(new Vector3(10f,0f,3f));
                path.Add(new Vector3(19f,0f,8f));
                path.Add(new Vector3(28f,0f,12f));
                path.Add(new Vector3(36f,0f,6f));
                path.Add(new Vector3(29f,0f,1.5f));
                path.Add(new Vector3(19f,0f,8f));
                break;

            case typePath.patrullaPuenteAzul:
                path.Add(new Vector3(4f,0f,39f));
                path.Add(new Vector3(35f,0f,39f));
                break;

            case typePath.patrullaPuenteRojo:
                path.Add(new Vector3(4f,0f,28f));
                path.Add(new Vector3(35f,0f,28f));
                break;

            case typePath.liderAzul:
                path.Add(new Vector3(28f,0f,42f));
                path.Add(new Vector3(11f,0f,42f));
                break;

            case typePath.liderRojo:
                path.Add(new Vector3(11f,0f,24f));
                path.Add(new Vector3(28f,0f,24f));
                break;

            default:
                break;    
        }
        DestroyPath();

    }

    public override void DestroyVirtual(Agent first)
    {
        DestroyPath();
    }
    public void DestroyPath(){
        int nNodes = nodes.Count-1;
        for (int n = nNodes;n>=0;n-- ) {
            Destroy(nodes[n].gameObject);
        }
        nodes.Clear();
    }

    public Agent getTarget() {
        if (nodes.Count!=0) {
            return nodes[currentNode];
        }
        else {
            return null;
        }
    }
    // Update is called once per frame
    public override Steering GetSteering(AgentNPC agent) {
        
        if (nodes.Count == 0) {
            foreach (var point in path) {
                Agent virt = agent.CreateVirtual(point, intRadius: intRadius, paint: giz);
                nodes.Add(virt);
            }
        }
        target = nodes[currentNode];
        float distance = Mathf.Abs((target.Position - agent.Position).magnitude); 
        if (distance <= target.interiorRadius) { 
            currentNode += pathDir;
            if (currentNode >= nodes.Count) {
                //Modo 0 = Me quedo al final
                if (mode == 0) {
                    currentNode = nodes.Count-1;
                    if (agent.agentState==State.LRTA) {
                        agent.agentState = State.Formation;
                        DestroyPath();
                        GameObject.FindObjectOfType<FormationManager>().notifyEndLRTA(agent);
                    }
                }
                //Modo 1 = Vuelta al primer punto
                if (mode == 1) currentNode = 0; 
            }
            if (currentNode >= nodes.Count || currentNode < 0) {
                //Modo 2 = marcha atrás
                if (mode == 2) {
                    pathDir = -pathDir;
                    currentNode+=pathDir;
                }
            }
            //Cuando destruimos el camino inicial antes de desturir el steering entra una última vez aqui, evitar que de error
            if (nodes.Count!=0) {
                target = nodes[currentNode];
            }
            else return new Steering();
        }
        return base.GetSteering(agent); 
    }

}
