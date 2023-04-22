using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Terrain {
    Camino,
    Bosque,
    Desierto,
    Llanura
}
public class Nodo
{
    
    private float costeHeuristica; //representa el coste heuristico a un determinado objetivo
    private float tempH;
    private int fila; //fila de la celda que representa
    private int col; //columna de la celda que representa
    private Terrain terreno;

    private float _g;

    private float _h;

    private float _f {get {return _g + _h;}}

    private Nodo parent;

    private bool transitable;
    private float weight;


    public Nodo(int fila, int col, bool transitable, int weight)
    {
        
        this.fila = fila;
        this.col = col;
        this.transitable = transitable;
        this.weight = weight;
    }

    public Vector2Int Celda {
        get {return new Vector2Int(fila, col); }
    }

    public bool Transitable
    {
        set { transitable = value;  }
        get { return transitable;  }
    }

    public float CosteHeuristica
    {
        set { costeHeuristica = value;  }
        get { return costeHeuristica; }
    }

    public float Weight
    {
        set { weight = value;  }
        get { return weight; }
    }

    public float TempH
    {
        set { tempH = value;  }
        get { return tempH; }
    }

    public float g
    {
        set { _g = value;  }
        get { return _g; }
    }

    public float h
    {
        set { _h = value;  }
        get { return _h; }
    }

    public float f
    {
        get { return _f; }
    }

    public Nodo Parent 
    {    
            set {parent = value;}
            get {return parent;}
        
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

