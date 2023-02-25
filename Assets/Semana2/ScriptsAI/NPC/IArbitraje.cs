using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Es la interfaz que siguen todos los arbitros porque todos implementan esta operacion que es obtener una serie de steerings
 * con sus pesos y calcular el steering final. Aparte se debe pasar el agente
 * el agente es a quien se le va a aplicar el steering (NO es el target/targets del steering)
 */

public interface IArbitraje
{
    
    public Steering calcula(List<SteeringBehaviour> steerings,Agent agente);
}
