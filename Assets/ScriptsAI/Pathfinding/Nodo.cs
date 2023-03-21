using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo
{
    private float costeCelda; //representa el coste de una celda
    private float costeHeuristica; //representa el coste heuristico a un determinado objetivo
    private int fila; //fila de la celda que representa
    private int col; //columna de la celda que representa
    private bool transitable;


    public Nodo(int costeCelda, int fila, int col, bool transitable)
    {
        this.costeCelda = costeCelda;
        this.fila = fila;
        this.col = col;
        this.transitable = transitable;
    }

    public Vector2Int Celda {
        get {return new Vector2Int(fila, col); }
    }

    public bool Transitable
    {
        get { return transitable;  }
    }

    public float CosteHeuristica
    {
        set { costeHeuristica = value;  }
        get { return costeHeuristica; }
    }

    public float CosteCelda
    {
        get { return costeCelda; }
    }

    public override bool Equals(object obj)
    {
        if (obj is Nodo) return Equals(obj as Nodo);
        else
        {
            Debug.Log("Error comparacion con objeto que no es un nodo");
            return false; //no es un nodo y retornamos false
        }

    }

    private bool Equals(Nodo obj)
    {
        return obj != null && obj.Celda.x == fila && obj.Celda.y == fila;
    }

    public override int GetHashCode()
    {
        return new Vector2Int(fila, col).GetHashCode();
    }

}

