using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeTerrain {
    desierto,
    bosque,
    llanura,
    camino,
    invalido
}

public class TerrainMap : MonoBehaviour
{
    
    public List<Vector3> waypointCuracionRojo = new List<Vector3>();
    public List<Vector3> waypointCuracionAzul = new List<Vector3>();
    public List<Vector3> waypointEstrategicosRojo = new List<Vector3>();
    public List<Vector3> waypointEstrategicosAzul = new List<Vector3>();
    public List<Vector3> waypointBaseRojo = new List<Vector3>();
    public List<Vector3> waypointBaseAzul = new List<Vector3>();
    public List<Vector3> waypointReaparicionRojo = new List<Vector3>();
    public List<Vector3> waypointReaparicionAzul = new List<Vector3>();

    public bool depuration = false;
    TypeTerrain[,] mapa;
    
    void Awake() {
        mapa = new TypeTerrain[30,30];
        initialize();
        putInvalid();
        putPath();
        putForest();
        putDesert();

        waypointCuracionRojo.Add(new Vector3(19f,0f,13.5f));
        waypointCuracionAzul.Add(new Vector3(73f,0f,77f));

        waypointEstrategicosAzul.Add(new Vector3(75f,0f,67.5f));
        waypointEstrategicosAzul.Add(new Vector3(41f,0f,50f));
        waypointEstrategicosAzul.Add(new Vector3(17f,0f,32f));
        waypointEstrategicosAzul.Add(new Vector3(43f,0f,64f));
        waypointEstrategicosAzul.Add(new Vector3(19.5f,0f,64f));

        waypointEstrategicosRojo.Add(new Vector3(75f,0f,58f));
        waypointEstrategicosRojo.Add(new Vector3(48f,0f,41f));
        waypointEstrategicosRojo.Add(new Vector3(17f,0f,23f));
        waypointEstrategicosRojo.Add(new Vector3(73f,0f,38f));

        waypointBaseAzul.Add(new Vector3(19f,0f,70f));
        
        waypointBaseRojo.Add(new Vector3(70f,0f,20f));

        waypointReaparicionRojo.Add(new Vector3(70f,0f,20f));
        waypointReaparicionRojo.Add(new Vector3(50f,0f,10f));
        waypointReaparicionRojo.Add(new Vector3(80f,0f,35f));

        waypointReaparicionAzul.Add(new Vector3(19f,0f,70f));
        waypointReaparicionAzul.Add(new Vector3(43f,0f,74f));
        waypointReaparicionAzul.Add(new Vector3(10f,0f,50f));



    }

    public TypeTerrain[,] MapaTerreno {
        get {return mapa;}
    }

    public TypeTerrain getTerrenoCasilla(int x, int y) {
        return mapa[y,x];
    }

    private void initialize() {
        TypeTerrain t;
        for (int i=0;i<30;i++) {
            for (int j=0;j<30;j++) {

                if ((i==0) || (i==1) || (i==28) ||(i==29) || (j==0) || (j==1) || (j==28) ||(j==29)) {
                    t = TypeTerrain.invalido;
                }
                else t = TypeTerrain.llanura;
                mapa[i,j] = t;
            }
        }
    }

    private void putDesert() {
        TypeTerrain t = TypeTerrain.desierto;

        mapa[8,20] = t;
        mapa[8,21] = t;
        mapa[8,22] = t;

        mapa[9,20] = t;
        mapa[9,21] = t;
        mapa[9,22] = t; 

        mapa[10,20] = t;
        mapa[10,21] = t;
        mapa[10,22] = t;
        mapa[10,23] = t;
        mapa[10,24] = t;

        mapa[11,20] = t;
        mapa[11,21] = t;
        mapa[11,22] = t;
        mapa[11,23] = t;
        mapa[11,24] = t;

        mapa[17,23] = t;
        mapa[17,25] = t;
        mapa[17,26] = t;

        mapa[18,23] = t;
        mapa[18,24] = t;
        mapa[18,25] = t;
        mapa[18,26] = t;

        mapa[19,4] = t;
        mapa[19,5] = t;
        mapa[19,6] = t;
        mapa[19,7] = t;
        mapa[19,23] = t;
        mapa[19,24] = t;
        mapa[19,25] = t;
        mapa[19,26] = t;

        mapa[20,4] = t;
        mapa[20,5] = t;
        mapa[20,6] = t;
        mapa[20,7] = t;
    }
    private void putForest() {
        TypeTerrain t = TypeTerrain.bosque;

        mapa[3,8] = t;

        mapa[4,7] = t;
        mapa[4,8] = t;
        mapa[4,9] = t;
        mapa[4,10] = t;

        mapa[5,7] = t;

        mapa[11,17] = t;
        mapa[11,18] = t;

        mapa[12,17] = t;
        mapa[12,18] = t;

        mapa[13,3] = t;
        mapa[13,4] = t;
        mapa[13,5] = t;
        mapa[13,6] = t;
        mapa[13,7] = t;
        mapa[13,8] = t;
        mapa[13,17] = t;
        mapa[13,18] = t;
        mapa[13,19] = t;

        mapa[14,3] = t;
        mapa[14,4] = t;
        mapa[14,5] = t;
        mapa[14,6] = t;
        mapa[14,7] = t;
        mapa[14,8] = t;

        mapa[22,16] = t;
        mapa[22,17] = t;
        mapa[22,18] = t;
        mapa[22,19] = t;
        mapa[22,20] = t;

        mapa[23,16] = t;
        mapa[23,17] = t;
        mapa[23,18] = t;
        mapa[23,19] = t;
        mapa[23,20] = t;

        mapa[24,16] = t;
        mapa[24,17] = t;
        mapa[24,18] = t;
        mapa[24,19] = t;
        mapa[24,20] = t;

        mapa[25,16] = t;
        mapa[25,17] = t;
        mapa[25,18] = t;
        mapa[25,19] = t;
        mapa[25,20] = t;

    }
    private void putPath() {
        TypeTerrain t = TypeTerrain.camino;
        mapa[3,9] = t;
        mapa[3,10] = t;

        mapa[4,6] = t;
        mapa[4,12] = t;
        mapa[4,13] = t;
        mapa[4,14] = t;

        mapa[5,8] = t;
        mapa[5,14] = t;
        mapa[5,15] = t;
        mapa[5,16] = t;
        mapa[5,17] = t;
        mapa[5,18] = t;

        mapa[6,18] = t;
        mapa[6,19] = t;
        mapa[6,20] = t;
        mapa[6,21] = t;
        mapa[6,22] = t;
        mapa[6,23] = t;

        mapa[7,23] = t;

        mapa[8,5] = t;
        mapa[8,6] = t;
        mapa[8,23] = t;

        mapa[9,5] = t;
        mapa[9,6] = t;
        mapa[9,23] = t;
        mapa[9,24] = t;
        mapa[9,25] = t;

        mapa[10,5] = t;
        mapa[10,6] = t;
        mapa[10,16] = t;
        mapa[10,17] = t;
        mapa[10,18] = t;
        mapa[10,19] = t;
        mapa[10,25] = t;

        mapa[11,5] = t;
        mapa[11,6] = t;
        mapa[11,16] = t;
        mapa[11,19] = t;
        mapa[11,25] = t;

        mapa[12,5] = t;
        mapa[12,6] = t;
        mapa[12,7] = t;
        mapa[12,8] = t;
        mapa[12,9] = t;
        mapa[12,16] = t;
        mapa[12,19] = t;
        mapa[12,20] = t;
        mapa[12,21] = t;
        mapa[12,22] = t;
        mapa[12,23] = t;
        mapa[12,24] = t;
        mapa[12,25] = t;

        mapa[13,9] = t;
        mapa[13,15] = t;
        mapa[13,16] = t;
        mapa[13,24] = t;

        mapa[14,9] = t;
        mapa[14,14] = t;
        mapa[14,15] = t;
        mapa[14,16] = t;
        mapa[14,24] = t;

        mapa[15,7] = t;
        mapa[15,8] = t;
        mapa[15,9] = t;
        mapa[15,13] = t;
        mapa[15,14] = t;
        mapa[15,15] = t;
        mapa[15,24] = t;

        mapa[16,7] = t;
        mapa[16,13] = t;
        mapa[16,14] = t;
        mapa[16,24] = t;

        mapa[17,7] = t;
        mapa[17,13] = t;
        mapa[17,24] = t;

        mapa[18,3] = t;
        mapa[18,4] = t;
        mapa[18,5] = t;
        mapa[18,6] = t;
        mapa[18,7] = t;
        mapa[18,13] = t;
        mapa[18,14] = t;

        mapa[19,3] = t;
        mapa[19,14] = t;

        mapa[20,3] = t;
        mapa[20,14] = t;
        mapa[20,24] = t;
        mapa[20,25] = t;

        mapa[21,3] = t;
        mapa[21,4] = t;
        mapa[21,5] = t;
        mapa[21,6] = t;
        mapa[21,7] = t;
        mapa[21,8] = t;
        mapa[21,9] = t;
        mapa[21,10] = t;
        mapa[21,11] = t;
        mapa[21,12] = t;
        mapa[21,13] = t;
        mapa[21,14] = t;
        mapa[21,15] = t;
        mapa[21,24] = t;
        mapa[21,25] = t;

        mapa[22,6] = t;
        mapa[22,15] = t;
        mapa[22,24] = t;
        mapa[22,25] = t;

        mapa[23,6] = t;
        mapa[23,15] = t;
        mapa[23,21] = t;
        mapa[23,22] = t;
        mapa[23,23] = t;
        mapa[23,24] = t;
        mapa[23,25] = t;

        mapa[24,15] = t;
        mapa[24,21] = t;

        mapa[25,15] = t;
        mapa[25,21] = t;

        mapa[26,15] = t;
        mapa[26,16] = t;
        mapa[26,17] = t;
        mapa[26,18] = t;
        mapa[26,19] = t;
        mapa[26,20] = t;
        mapa[26,21] = t;

    }

    private void putInvalid() {
        TypeTerrain t = TypeTerrain.invalido;
        mapa[2,5] = t;
        mapa[2,6] = t;
        mapa[2,7] = t;
        mapa[2,24] = t;
        mapa[2,25] = t;
        mapa[2,26] = t;
        mapa[2,27] = t;

        mapa[3,5] = t;
        mapa[3,6] = t;
        mapa[3,7] = t;
        mapa[3,23] = t;
        mapa[3,24] = t;
        mapa[3,25] = t;
        mapa[3,26] = t;
        mapa[3,27] = t;

        mapa[4,23] = t;
        mapa[4,24] = t;
        mapa[4,25] = t;
        mapa[4,26] = t;
        mapa[4,27] = t;        

        mapa[5,23] = t;
        mapa[5,24] = t;
        mapa[5,25] = t;
        mapa[5,26] = t;

        mapa[6,24] = t;
        mapa[6,25] = t;

        mapa[7,2] = t;

        mapa[8,2] = t;
        mapa[8,3] = t;
        mapa[8,4] = t;
        mapa[8,7] = t;
        mapa[8,8] = t;
        mapa[8,9] = t;
        mapa[8,17] = t;
        mapa[8,18] = t;
        mapa[8,19] = t;

        mapa[9,2] = t;
        mapa[9,3] = t;
        mapa[9,4] = t;
        mapa[9,7] = t;
        mapa[9,8] = t;
        mapa[9,9] = t;
        mapa[9,10] = t;
        mapa[9,17] = t;
        mapa[9,18] = t;
        mapa[9,19] = t;

        mapa[10,8] = t;
        mapa[10,9] = t;
        mapa[10,10] = t;
        mapa[10,11] = t;

        mapa[11,9] = t;
        mapa[11,10] = t;
        mapa[11,11] = t;
        mapa[11,12] = t;

        mapa[12,10] = t;
        mapa[12,11] = t;
        mapa[12,12] = t;
        mapa[12,13] = t;

        mapa[13,11] = t;
        mapa[13,12] = t;
        mapa[13,13] = t;
        mapa[13,14] = t;

        mapa[14,12] = t;
        mapa[14,13] = t;

        mapa[15,16] = t;
        mapa[15,17] = t;

        mapa[16,15] = t;
        mapa[16,16] = t;  
        mapa[16,17] = t;
        mapa[16,18] = t;

        mapa[17,8] = t;
        mapa[17,9] = t;  
        mapa[17,10] = t;
        mapa[17,16] = t;
        mapa[17,17] = t;  
        mapa[17,18] = t;
        mapa[17,19] = t;    
        
        mapa[18,8] = t;
        mapa[18,9] = t;  
        mapa[18,10] = t;
        mapa[18,17] = t;
        mapa[18,18] = t;  
        mapa[18,19] = t;
        mapa[18,20] = t; 

        mapa[19,18] = t;
        mapa[19,19] = t;  
        mapa[19,20] = t;
        mapa[19,21] = t;
        mapa[19,27] = t;

        mapa[20,19] = t;
        mapa[20,20] = t;  
        mapa[20,21] = t;
        mapa[20,22] = t;
        mapa[20,23] = t;
        mapa[20,26] = t; 
        mapa[20,27] = t;

        mapa[21,21] = t;
        mapa[21,22] = t;
        mapa[21,23] = t;
        mapa[21,26] = t; 
        mapa[21,27] = t;
 
        mapa[22,27] = t;  

        mapa[23,5] = t;

        mapa[24,3] = t;  
        mapa[24,4] = t;  
        mapa[24,5] = t;  
        mapa[24,6] = t;

        mapa[25,2] = t;  
        mapa[25,3] = t;  
        mapa[25,4] = t;  
        mapa[25,5] = t;
        mapa[25,6] = t;

        mapa[26,2] = t;  
        mapa[26,3] = t;  
        mapa[26,4] = t;  
        mapa[26,5] = t;
        mapa[26,6] = t;
        mapa[26,11] = t;  
        mapa[26,12] = t;  
        mapa[26,13] = t;  
        mapa[26,23] = t;
        mapa[26,24] = t;
        mapa[26,25] = t;

        mapa[27,2] = t;  
        mapa[27,3] = t;  
        mapa[27,4] = t;  
        mapa[27,5] = t;
        mapa[27,11] = t;  
        mapa[27,12] = t;  
        mapa[27,13] = t;  
        mapa[27,23] = t;
        mapa[27,24] = t;
        mapa[27,25] = t;
    }

    protected void OnDrawGizmos()
    {
        if (depuration == true) {    
            Gizmos.color = Color.blue;
            foreach (var n in waypointBaseAzul){
                Gizmos.DrawSphere(n, 1);
            }
            foreach (var n in waypointEstrategicosAzul){
                Gizmos.DrawSphere(n, 1);
            }
            foreach (var n in waypointCuracionAzul){
                Gizmos.DrawSphere(n, 1);
            }
            Gizmos.color = Color.red;
            foreach (var n in waypointBaseRojo){
                Gizmos.DrawSphere(n, 1);
            }
            foreach (var n in waypointEstrategicosRojo){
                Gizmos.DrawSphere(n, 1);
            }
            foreach (var n in waypointCuracionRojo){
                Gizmos.DrawSphere(n, 1);
            }
            /*for (int i=0;i<30;i++) {
                for (int j=0;j<30;j++) {
                    TypeTerrain t = mapa[i,j];
                    switch (t) {
                        case TypeTerrain.invalido:
                            Gizmos.color = Color.black;
                            Gizmos.DrawSphere(new Vector3(j*3f+1.5f,0,i*3f+1.5f), 1);
                            break;
                        case TypeTerrain.llanura:
                            Gizmos.color = Color.green;
                            Gizmos.DrawSphere(new Vector3(j*3f+1.5f,0,i*3f+1.5f), 1);
                            break;
                        case TypeTerrain.desierto:
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawSphere(new Vector3(j*3f+1.5f,0,i*3f+1.5f), 1);
                            break;
                        case TypeTerrain.camino:
                            Gizmos.color = Color.gray;
                            Gizmos.DrawSphere(new Vector3(j*3f+1.5f,0,i*3f+1.5f), 1);
                            break;
                        case TypeTerrain.bosque:
                            Gizmos.color = Color.blue;
                            Gizmos.DrawSphere(new Vector3(j*3f+1.5f,0,i*3f+1.5f), 1);
                            break;
                    }
                    
                }
            }*/
        }
    }
    
}
