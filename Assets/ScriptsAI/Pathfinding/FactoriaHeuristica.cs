using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeHeuristica {
    Manhattan,
    Chebychev,
    Euclidea
}

public class FactoriaHeuristica 
{
    public static Heuristica crearHeuristica(typeHeuristica h)
    {
       
        switch(h)
        {
            case typeHeuristica.Manhattan:
                return new Manhattan();
            case typeHeuristica.Chebychev:
                return new Chebychev();
            default:
                return new Euclidea(); //por defecto se da la euclediana
        }
    }
}
