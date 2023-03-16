using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum typeArbitro {
    Huidizo,
    Perseguidor,
    Vagante,
    Quieto
}

public class GestorArbitros
{
    /*usamos una factoria para crear los diferentes arbitros, se le necesita pasar el agente que ha llamado al metodo y los objetos (que pueden ser muchos, uno o ninguno)
    Ej: con el WallAvoidance NO necesitas ningun target */
    public static List<SteeringBehaviour> GetArbitraje(typeArbitro arbitro,Agent agente,List<Agent> objetivos)
    {
        List<SteeringBehaviour> steeringsDevueltos = new List<SteeringBehaviour>();
        SteeringBehaviour actual; //steering que se esta cambiando
        switch (arbitro)
        {
            case typeArbitro.Huidizo:
                
                Flee flee = new Flee();
                flee.Weight = 1f;
                flee.NameSteering = "Flee";
                flee.NewTarget(GameObject.FindObjectOfType<AgentPlayer>());
                steeringsDevueltos.Add(flee);
                
                AntiFace antiface = new AntiFace();
                antiface.Weight = 1f;
                antiface.instantiateAntiFace();
                antiface.AntiFaceNewTarget(GameObject.FindObjectOfType<AgentPlayer>());
                steeringsDevueltos.Add(antiface);

                WallAvoidance wall = new WallAvoidance();
                wall.Start();
                wall.Weight = 8f;
                steeringsDevueltos.Add(wall);

                break;

            case typeArbitro.Perseguidor:
                actual = new Pursue();
                Pursue seguidor = (Pursue)actual; //hago un casting para poder acceder al campo "target"
                seguidor.pursueTarget = objetivos[0]; //obtiene el primer elemento que deberia haberlo OJO AQUI QUE SE PUEDE LIAR SI NO HAY UN PursueTarget
                steeringsDevueltos.Add(seguidor);
                break;

            case typeArbitro.Vagante:
                actual = new Wander();
                steeringsDevueltos.Add(actual);
                //actual = new WallAvoidance
                break;

            case typeArbitro.Quieto:
                break;
            default: //por defecto podemos devolver por ejemplo el wander
                actual = new Wander();
                steeringsDevueltos.Add(actual);
                break;
        }

        return steeringsDevueltos; //se devuelven los steerings
    }


}
