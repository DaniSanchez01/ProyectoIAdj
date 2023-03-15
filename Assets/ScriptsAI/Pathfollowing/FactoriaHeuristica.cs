using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoriaHeuristica 
{
    public static Heuristica crearHeuristica(string nombre)
    {
       
        switch(nombre)
        {
            case "Manhattan":
                return new Manhattan();
            case "Chebyshev":
                return new Chebychev();
            default:
                return new Euclidea(); //por defecto se da la euclediana
        }
    }
}
