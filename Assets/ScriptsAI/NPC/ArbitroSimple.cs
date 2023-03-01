using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Este arbitro calcula dado una lista de steerings que tiene un agente determinado el steering final sumando los pesosrespectivos de estos.
 */

public class ArbitroSimple : MonoBehaviour
{

    public Steering calcula(List<SteeringBehaviour> steerings,Agent agente)
    {
        Steering resultado = new Steering();
        resultado.linear = Vector3.zero;
        resultado.angular = 0;

        foreach (var s in steerings)
        {
            Steering steeractual = s.GetSteering(agente);
            resultado.linear = resultado.linear + s.Weight * steeractual.linear;
            resultado.angular = resultado.angular + s.Weight * steeractual.angular;
        }

        //Nota: si se observa el algoritmo de la diapositiva 7 del tema 8 se puede observar que al final del arbitro que mezcla los steerings y recorta la aceleracion linear y angular obtenida
        //a la maximas aceleraciones del agente, en nuestro caso no hace falta hacerlo porque la clase "Bodi" ya lo hace por nosotros.

        //Nota2: el algoritmo comentado de la diapositiva 7 del tema 8 tiene una errata porque pone "max" y deber�a ser "min" cuando comprueba que los steer.linear y steer.angular obtenidos
        //no son mayores que las aceleraciones permitidas
        return resultado;


    }
}
