using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingNoOffset : SeekCraig
{

    List<Vector3> path = new List<Vector3>();
    List<Agent> nodes = new List<Agent>(); 
    int pathDir = 1;
    int currentNode = 0;
    public int mode = 1;
    public bool giz = true;
    public float intRadius = 1.5f;

    // Start is called before the first frame update
    void Awake()
    {
        base.nameSteering = "PathFollowing";
        path.Add(new Vector3(30f,0f,38f));
        path.Add(new Vector3(13f,0f,38f));
        path.Add(new Vector3(13f,0f,17f));
        path.Add(new Vector3(30f,0f,4.5f));
        path.Add(new Vector3(0f,0f,0f));
        path.Add(new Vector3(2f,0f,45f));


    }

    public void setPath(List<Vector3> newPath) {
        path = newPath;
        int nNodes = nodes.Count-1;
        for (int n = nNodes;n>=0;n-- ) {
            Destroy(nodes[n].gameObject);
        }
        nodes.Clear();
    }

    public Agent getTarget() {
        return nodes[currentNode];
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
            target = nodes[currentNode];
        }
        
        
        return base.GetSteering(agent); 
    }

}
