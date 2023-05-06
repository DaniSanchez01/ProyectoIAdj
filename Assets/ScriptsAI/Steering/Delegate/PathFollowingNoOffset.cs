using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typePath {
    vigilaBaseRoja,
    vigilaBaseAzul,
    vigilaRioRojo,
    vigilaRioAzul,
    hospitalAzul,
    hospitalRojo,
    soldadoHospitalRojo,
    soldadoDesiertoRojo,
    Vikingo1Rojo,
    Vikingo2Rojo,
    ArqueroRojo,
    soldadoHospitalAzul,
    soldadoDesiertoAzul,
    Vikingo1Azul,
    Vikingo2Azul,
    ArqueroAzul,
    CaminoOfensivoAzul1,
    CaminoOfensivoRojo1,
    CaminoOfensivoAzul2,
    CaminoOfensivoRojo2,
    CaminoOfensivoAzul3,
    CaminoOfensivoRojo3,
    vigilaRioRojo2,
    vigilaRioAzul2,


    
}

public class PathFollowingNoOffset : SeekCraig
{

    List<Vector3> path = new List<Vector3>();
    List<Agent> nodes = new List<Agent>(); 
    int pathDir = 1;
    int currentNode = 0;
    public int mode = 1;
    public bool giz = false;
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

    public List<Vector3> getPath() {
        return path;
    }

    public void setTypePath(typePath typePath) {
        type= typePath;
        path.Clear();
        switch (type) {
            case typePath.vigilaBaseRoja:
                path.Add(new Vector3(67f,0f,22f));
                path.Add(new Vector3(67f,0f,9f));
                path.Add(new Vector3(67f,0f,22f));
                path.Add(new Vector3(80f,0f,22f));
                break;

            case typePath.vigilaBaseAzul:
                path.Add(new Vector3(24f,0f,67f));
                path.Add(new Vector3(24f,0f,80f));
                path.Add(new Vector3(24f,0f,67f));
                path.Add(new Vector3(12,0f,67f));
                break;

            case typePath.vigilaRioRojo:
                path.Add(new Vector3(28f,0f,21f));
                path.Add(new Vector3(65f,0f,53.5f));
                break;
            case typePath.vigilaRioRojo2:
                path.Add(new Vector3(65f,0f,53.5f));
                path.Add(new Vector3(28f,0f,21f));
                break;
            case typePath.vigilaRioAzul:
                path.Add(new Vector3(23f,0f,35f));
                path.Add(new Vector3(59f,0f,66f));
                break;
            case typePath.vigilaRioAzul2:
                path.Add(new Vector3(59f,0f,66f));
                path.Add(new Vector3(23f,0f,35f));
                break;
            case typePath.hospitalAzul:
                path.Add(new Vector3(77f,0f,70f));
                path.Add(new Vector3(65f,0f,70f));
                path.Add(new Vector3(65f,0f,80f));
                path.Add(new Vector3(65f,0f,70f));
                break;
            case typePath.hospitalRojo:
                path.Add(new Vector3(13f,0f,15f));
                path.Add(new Vector3(28.5f,0f,15f));
                path.Add(new Vector3(28.5f,0f,8f));
                path.Add(new Vector3(28.5f,0f,15f));
                break;
            case typePath.soldadoHospitalRojo:
                path.Add(new Vector3(18.65f,0f,18.4f));
                break;
            case typePath.soldadoDesiertoRojo:
                path.Add(new Vector3(74f,0f,50.5f));
                break;
            case typePath.Vikingo1Rojo:
                path.Add(new Vector3(59f,0f,18f));
                break;
            case typePath.Vikingo2Rojo:
                path.Add(new Vector3(74f,0f,28f));
                break;
            case typePath.ArqueroRojo:
                path.Add(new Vector3(77.5f,0f,36.5f));
                break;
            case typePath.soldadoHospitalAzul:
                path.Add(new Vector3(69.5f,0f,70f));
                break;
            case typePath.soldadoDesiertoAzul:
                path.Add(new Vector3(20f,0f,47f));
                break;
            case typePath.Vikingo1Azul:
                path.Add(new Vector3(16.5f,0f,54.5f));
                break;
            case typePath.Vikingo2Azul:
                path.Add(new Vector3(29f,0f,64f));
                break;
            case typePath.ArqueroAzul:
                path.Add(new Vector3(46f,0f,71.5f));
                break;
            //Este camino guarda el puente izquierdo
            case typePath.CaminoOfensivoAzul1:
                path.Add(new Vector3(25.5f,0f,38f));
                path.Add(new Vector3(12f,0f,38f));
                break;
            //Psa por el camino de la base a la curacion azul
            case typePath.CaminoOfensivoAzul2:
                path.Add(new Vector3(46.5f,0f,65f));
                path.Add(new Vector3(46.5f,0f,80f));
                path.Add(new Vector3(46.5f,0f,65f));
                path.Add(new Vector3(22.5f,0f,65f));
                break;
            //Este camino da vueltas por la motaña de la zona azul
            case typePath.CaminoOfensivoAzul3:
                path.Add(new Vector3(36f,0f,49f));
                path.Add(new Vector3(36f,0f,59f));
                path.Add(new Vector3(22f,0f,59f));
                path.Add(new Vector3(22f,0f,49f));
                break;
            //Desierto de la zona roja
            case typePath.CaminoOfensivoRojo1:
                path.Add(new Vector3(62f,0f,34.5f));
                path.Add(new Vector3(72.5f,0f,34.5f));
                path.Add(new Vector3(72.5f,0f,31.5f));               
                path.Add(new Vector3(62f,0f,31.5f));
                
                break;
            //Puente derecho
            case typePath.CaminoOfensivoRojo2:
                path.Add(new Vector3(67f,0f,50f));
                path.Add(new Vector3(82f,0f,50f));
                break;
            //Centro zona roja
            case typePath.CaminoOfensivoRojo3:
                path.Add(new Vector3(55f,0f,17f));
                path.Add(new Vector3(44f,0f,17f));
                path.Add(new Vector3(44f,0f,27f));
                path.Add(new Vector3(36f,0f,27f));
                path.Add(new Vector3(44f,0f,27f));
                path.Add(new Vector3(44f,0f,17f));
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
            //Debug.LogFormat("{0},{1}",currentNode,nodes.Count);
            if (currentNode >= nodes.Count) {
                //Modo 0 = Me quedo al final
                if (mode == 0) {
                    currentNode = nodes.Count-1;
                    /*if (agent.agentState==State.LRTA) {
                        agent.agentState = State.Formation;
                        DestroyPath();
                        GameObject.FindObjectOfType<FormationManager>().notifyEndLRTA(agent);
                    }*/
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
