using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum typeArbitro {
    Huidizo,
    Perseguidor,
    Posicionar,
    Quieto,
    Aleatorio,
    RecorreCamino,

    Observar,
}

public class GestorArbitros
{
    /*usamos una factoria para crear los diferentes arbitros, se le necesita pasar el agente que ha llamado al metodo y los objetos (que pueden ser muchos, uno o ninguno)
    Ej: con el WallAvoidance NO necesitas ningun target */
    public static List<SteeringBehaviour> GetArbitraje(typeArbitro arbitro,Agent agente,Agent target,typePath pathToFollow)
    {
        List<SteeringBehaviour> steeringsDevueltos = new List<SteeringBehaviour>();
        switch (arbitro)
        {
            case typeArbitro.Huidizo:
                
                Flee flee = agente.gameObject.AddComponent<Flee>();
                flee.Weight = 1f;
                flee.NewTarget(target);
                steeringsDevueltos.Add(flee);
                
                AntiFace antiface = agente.gameObject.AddComponent<AntiFace>();
                antiface.Weight = 1f;
                antiface.AntiFaceNewTarget(target);
                steeringsDevueltos.Add(antiface);

                WallAvoidance wall = agente.gameObject.AddComponent<WallAvoidance>();
                wall.Weight = 50f;
                steeringsDevueltos.Add(wall);

                break;

            case typeArbitro.Perseguidor:
                
                Arrive arrive = agente.gameObject.AddComponent<Arrive>();
                arrive.Weight = 1f;
                arrive.NewTarget(target);
                steeringsDevueltos.Add(arrive);
                
                Face face = agente.gameObject.AddComponent<Face>();
                face.Weight = 1f;
                face.FaceNewTarget(target);
                steeringsDevueltos.Add(face);

                wall = agente.gameObject.AddComponent<WallAvoidance>();
                wall.Weight = 50f;
                wall.gizmos = true;
                steeringsDevueltos.Add(wall);
                break;

            case typeArbitro.Posicionar:
                arrive = agente.gameObject.AddComponent<Arrive>();
                arrive.Weight = 1f;
                arrive.NewTarget(target);
                steeringsDevueltos.Add(arrive);
                
                Align align = agente.gameObject.AddComponent<Align>();
                align.Weight = 1f;
                align.NewTarget(target);
                steeringsDevueltos.Add(align);

                wall = agente.gameObject.AddComponent<WallAvoidance>();
                wall.Weight = 50f;
                steeringsDevueltos.Add(wall);
                break;

            case typeArbitro.Quieto:
                arrive = agente.gameObject.AddComponent<Arrive>();
                arrive.Weight = 1f;
                arrive.NewTarget(agente);
                steeringsDevueltos.Add(arrive);
                align = agente.gameObject.AddComponent<Align>();
                align.Weight = 1f;
                align.NewTarget(agente);
                steeringsDevueltos.Add(align);
                break;

            case typeArbitro.Aleatorio:
                Wander wander = agente.gameObject.AddComponent<Wander>();
                wander.Weight = 0.5f;
                steeringsDevueltos.Add(wander);

                wall = agente.gameObject.AddComponent<WallAvoidance>();
                wall.Weight = 50f;
                steeringsDevueltos.Add(wall);
                break;
            
            case typeArbitro.RecorreCamino:
                PathFollowingNoOffset pathF = agente.gameObject.AddComponent<PathFollowingNoOffset>();
                pathF.setTypePath(pathToFollow);
                steeringsDevueltos.Add(pathF);
                face = agente.gameObject.AddComponent<Face>();
                face.Weight = 1f;
                face.FaceNewTarget(target);
                face.path = pathF;
                steeringsDevueltos.Add(face);
                wall = agente.gameObject.AddComponent<WallAvoidance>();
                wall.Weight = 50f;
                wall.gizmos = true;
                steeringsDevueltos.Add(wall);
                break;

            case typeArbitro.Observar:
                arrive = agente.gameObject.AddComponent<Arrive>();
                arrive.Weight = 1f;
                arrive.NewTarget(agente);
                steeringsDevueltos.Add(arrive);
    
                face = agente.gameObject.AddComponent<Face>();
                face.Weight = 1f;
                face.FaceNewTarget(target);
                steeringsDevueltos.Add(face);
                break;
        }

        return steeringsDevueltos; //se devuelven los steerings
    }


}
