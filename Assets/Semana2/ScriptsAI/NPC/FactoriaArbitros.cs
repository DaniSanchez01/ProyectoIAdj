using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *Esta clase esta destinada a la creación de arbitros, sera usadada por la clase AgentNPC de manera que cuando 
 */

public class FactoriaArbitros
{
    //Nota: para la factoria he usado un switch de manera que lo que hara un agentNPC es tan facil cambiar de arbitro como que llame a la factoria con el nombre del arbitro y esta le devuelve
    //Una instancia del arbitro deseado. Por otra parte he usado un switch pero se podria haber usado un if-else e incluso otros mecanismos como guardar en un mapa pares (idArbitro,PrefabArbitro)
    public static IArbitraje GetArbitraje(string nombreArbitro)
    {
        switch (nombreArbitro)
        {
            case "ArbitroSimple":
                return new ArbitroSimple();

            default: //por defecto si no se detecta el arbitro pues devuelve un arbitro simple
                return new ArbitroSimple();

        }


    }


}
