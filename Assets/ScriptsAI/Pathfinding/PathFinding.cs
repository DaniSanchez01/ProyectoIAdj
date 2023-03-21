using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PathFindingState {
    NonUsed,
    Start,
    Walking,
}

public class PathFinding : MonoBehaviour
{
    
    int infinito = 9999999;
    GridPathFinding grid;
    Nodo objetivo;
    Nodo posicion;
    int prof;
    
    // Start is called before the first frame update
    public List<Nodo> searchLocalSpace()
    {
        List<Nodo> localSpace = grid.getVecinosValidosProf(posicion,prof);
        return localSpace;
    }

    public bool existsInfinite(List<Nodo> localSpace) {
        foreach (var nodo in localSpace) {
            if (nodo.CosteHeuristica == infinito) {
                return true;
            }
        }
        return false;
    }

    public float calculateMinimum(List<Nodo> vecinos) {
        if (vecinos.Count == 0) {} /*¿Que pasaria si no tiene nigun vecino transitable?*/
        float min = infinito;
        foreach (var nodo in vecinos) {
            float valor = nodo.Weight + nodo.CosteHeuristica;
            if (valor < min) min = valor;
        }
        return min;

        
    }
    public void updateValues(List<Nodo> localSpace)
    {
        
        foreach (var nodo in localSpace) {
            nodo.TempH = nodo.CosteHeuristica;
            nodo.CosteHeuristica = infinito;
        }
        while (existsInfinite(localSpace)) {
            foreach (var nodo in localSpace) {
                List<Nodo> vecinos = grid.getVecinosValidosProf(nodo,1);
                float minimum = calculateMinimum(vecinos);
            }
        }
    }

}
