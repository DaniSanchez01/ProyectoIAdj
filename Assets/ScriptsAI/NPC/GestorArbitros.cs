using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *Esta clase esta destinada a la creación de arbitros, sera usadada por la clase AgentNPC 
 */

public class GestorArbitros
{
    /*usamos una factoria para crear los diferentes arbitros, se le necesita pasar el agente que ha llamado al metodo y los objetos (que pueden ser muchos, uno o ninguno)
    Ej: con el WallAvoidance NO necesitas ningun target */
    public static List<SteeringBehaviour> GetArbitraje(string nombreArbitro,Agent agente,List<Agent> objetivos)
    {
        List<SteeringBehaviour> steeringsDevueltos = new List<SteeringBehaviour>();
        SteeringBehaviour actual; //steering que se esta cambiando
        switch (nombreArbitro)
        {
            case "huidizo":
                actual = new Evade();
                actual.Weight = 0.6f;
                steeringsDevueltos.Add(actual);
                break;

            case "perseguidor":
                actual = new Pursue();
                Pursue seguidor = (Pursue)actual; //hago un casting para poder acceder al campo "target"
                seguidor.pursueTarget = objetivos[0]; //obtiene el primer elemento que deberia haberlo OJO AQUI QUE SE PUEDE LIAR SI NO HAY UN PursueTarget
                steeringsDevueltos.Add(seguidor);
                break;

            case "vagante":
                actual = new Wander();
                steeringsDevueltos.Add(actual);
                //actual = new WallAvoidance
                break;

            default: //por defecto podemos devolver por ejemplo el wander
                actual = new Wander();
                steeringsDevueltos.Add(actual);
                break;
        }

        return steeringsDevueltos; //se devuelven los steerings
    }


}
