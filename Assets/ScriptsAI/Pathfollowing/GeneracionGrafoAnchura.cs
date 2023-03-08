using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneracionGrafoAnchura 
{
    
    

    /*
     * Se encarga de generar el grafo correspondiente.
     * ¿Se garantiza que el punto al que se lleva en el plano cuando se cuantifica el origen real del personaje se corresponde caundo se localiza otra vez en el mundo con una posicion valida?
     */
    public static Dictionary<Vector3Int,List<Conexion>> generarGrafo(Vector3Int origenGeneracion,int limite)
    {
        //1. Crear el diccionario que representara el grafo y la lista de puntosGenerados que sera usada para el recorrido en anchura
        Dictionary<Vector3Int, List<Conexion>> diccionarioGrafo = new Dictionary<Vector3Int, List<Conexion>>();
        List<Vector3Int> puntosGenerados = new List<Vector3Int>();

        //2. validar que se esta en un punto valido es decir que si se traslada al mundo el punto real NO colisiona con nada
        if (esValidoPMundo(origenGeneracion)) {
            puntosGenerados.Add(origenGeneracion); //se añade al principio el nodo para expandirlo
            diccionarioGrafo.Add(origenGeneracion, new List<Conexion>()); //añado el origen del grafo porque es valido
        }

        /*3. Mientras hayan nodos y estos puedan expandirse, observese que los nodos estan ordenados por niveles al ser un recorrido en profundidad por eso cuando se encuentra
        *un nodo que que cumple que el nodo tiene un limite igual al establecido se espera porque no deberian expandirse, mientras eso no pasae el nodo escogido se explande y conecta
        con el resto de nodos
         */
        while (!(puntosGenerados.Count == 0) && puedeexpandirseNodo(puntosGenerados[0],origenGeneracion,limite))
        {
            //3.1. obtener el punto correspondiente a expandir
            Vector3Int pactual = puntosGenerados[0]; //se obtiene el primer punto como en una cola
            puntosGenerados.RemoveAt(0); //se elimina el punto en la 1º posicion

            //3.2. obtener los vecinos del nodo actual, recordar que el nodo que se esta expandiendo tambien tiene una entrada en el mapa porque se llego por otro nodo

            foreach(Vector3Int v in vecinos(pactual))
            {
                //3.2.1 si el mapa contiene el punto no se va a volver a generar el punto entonces el nodo actual se enlaza con el
                if (diccionarioGrafo.ContainsKey(v)) diccionarioGrafo[pactual].Add(new Conexion(pactual, v, 1)); //ESTOY PONIENDO UN COSTE DE 1 el coste se tiene que ver cual es
                
                //3.2.2 en otro caso comprobara si es valido
                else if (esValidoPMundo(v))
                {
                    //si lo es lo crea añadiendolo al mapa y se conecta a el
                    diccionarioGrafo.Add(v, new List<Conexion>()); //añade el vecino sin conexiones
                    puntosGenerados.Add(v); //se añade a la lista de los puntos a expandir
                    diccionarioGrafo[pactual].Add(new Conexion(pactual, v, 1)); //OJO HE PUESTO COSTE 1
                }
            }
            
        }

        /*4. Cuando se ha formado el grafo hay que revisar que no queden puntos. Cuando se sale del anterior bucle puede ser por 2 causas, o bien ya no quedan nodos esto quiere decir
        * que no se han podido generar mas niveles debido a que los puntos no eran validos o que se ha alcanzado los nodos que estan en el limite indicado. En este ultimo caso para cada nodo
        * Se hacen las conexiones solo con los del mismo nivel o inferior
        */
        while(! (puntosGenerados.Count == 0))
        {
            Vector3Int nodoActual = puntosGenerados[0];
            puntosGenerados.RemoveAt(0);
            
            foreach(Vector3Int v in vecinos(nodoActual))
            {
                //si el nodo generado se encuentra mas alla del limite no lo unas en caso contrario si porque los nodos cuya profundidad es <=limite estan generados
                if (!(calcularProfNodo(v,origenGeneracion) > limite)) diccionarioGrafo[nodoActual].Add(new Conexion(nodoActual, v, 1));
            }


        }

        return diccionarioGrafo;
    }


    /*
     * Genera los vecinos de un punto determinado acorde a una cuadricula. Concretamente tomando vertices de la cuadricula
     */
    private static List<Vector3Int> vecinos(Vector3Int punto)
    {
        List<Vector3Int> vec = new List<Vector3Int>();
        vec.Add(new Vector3Int(punto.x + 1, punto.y, punto.z)); //el vector que esta a la derecha del punto
        vec.Add(new Vector3Int(punto.x - 1, punto.y, punto.z)); //el vector que esta a la izquierdadel punto
        vec.Add(new Vector3Int(punto.x, punto.y, punto.z +1)); //el vector que esta arriba  del punto (en el eje de ordenadas)
        vec.Add(new Vector3Int(punto.x, punto.y, punto.z -1)); //el vector que esta abajo del punto (en el eje de ordenadas)
        return vec;
    }

    /*
     * Dado 1 nodo del grafo calcula la profundidad a la que esta respecto al nodo origen del grafo donde comenzo la generacion. Como se observa el valor obtenido
     */
    private static int calcularProfNodo(Vector3Int nodo,Vector3Int origenGeneracion)
    {
        return Mathf.Abs(origenGeneracion.x - nodo.x) + Mathf.Abs(origenGeneracion.y) + nodo.y + Mathf.Abs(origenGeneracion.z + nodo.z);
    }

    /*
     * Un nodo SOLO puede expandirse si la profunidad de este respecto al origen es menor que el la profundidad seleccionada, pues los nodos que esten en la profundidad = limite
     * NO deben expandirse pues generarian nodos de profundidad n+1
     */
    private static bool puedeexpandirseNodo(Vector3Int nodo,Vector3Int origenGeneracion, int limite)
    {
        return calcularProfNodo(nodo,origenGeneracion) < limite;
    }

    /*
     * Comprueba dado un punto del grafo si el punto correspondiente al mundo al realizar la localizacion es valido es decir se puede llegar a el desde sus regiones contiguas.
     * ¿Esto es correcto se podria usar un rayo o es mejor una esfera?
     */
    private static bool esValidoPMundo(Vector3Int puntoPlano)
    {
        //PENDIENTE comprobar que al transformar un punto del grafo al mundo no haya nada que colisione con este
        //PuntoMundo = new Vector3Int(puntoPlano) * L
        //comprobar colision
        return true;
    }



}
