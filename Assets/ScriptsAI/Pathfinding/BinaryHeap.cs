using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryHeap
{
    private List<Nodo> nodos;
    public BinaryHeap()
    {
        this.nodos = new List<Nodo>();
    }

    public bool sameNode(Nodo a, Nodo b) {
        return (a.Celda.x == b.Celda.x) && (a.Celda.y == b.Celda.y);
    }
    public bool contiene (Nodo a) {
        foreach (var n in nodos) {
            if (sameNode(a,n)) {
                return true;
            }
        }
        return false;
    }

    //Añade un nuevo nodo en su posición correspondiente
    public void Enqueue(Nodo item)
    {
        // Se añade el nodo en la última posición del arbol binario
        nodos.Add(item);
        //Posición del nodo
        int childIndex = nodos.Count - 1;
        //Mientras el nodo no este en la raiz del arbol binario
        while (childIndex > 0)
        {
            //Coge su nodo padre
            int parentIndex = (childIndex - 1) / 2;
            //Si el nodo tiene un costo mayor que el padre, lo dejamos en esta posición
            if (nodos[childIndex].f >= nodos[parentIndex].f)
                break;
            //Si es menor, cambiamos la posición del padre por la del hijo
            Nodo tmp = nodos[childIndex];
            nodos[childIndex] = nodos[parentIndex];
            nodos[parentIndex] = tmp;
            childIndex = parentIndex;
        }
    }

    //Devuelve el nodo con mayor prioridad y reordena la cola
    public Nodo Dequeue()
    {
        int lastIndex = nodos.Count - 1;
        Nodo frontItem = nodos[0];

        //Reemplaza el primer elemento de la cola de prioridad con el ultimo
        nodos[0] = nodos[lastIndex];
        nodos.RemoveAt(lastIndex);

        lastIndex--;
        int parentIndex = 0;
        //Mientras el arbol no vuelva a estar bien ordenado, jugar con este último elemento
        while (true)
        {
            //Cojo el hijo izquierdo
            int childIndex = parentIndex * 2 + 1;
            //Si no existe el hijo izquierdo, terminar
            if (childIndex > lastIndex)
                break;
            //Cojo el hijo derecho
            int rightChild = childIndex + 1;
            //Si el hijo derecho existe y tiene menor coste que el izquierdo, recordarlo
            if (rightChild <= lastIndex && (nodos[rightChild].f < nodos[childIndex].f))
                childIndex = rightChild;
            //Si el hijo con el menor coste tiene más coste que este elemento, terminar
            if ((nodos[parentIndex].f <= nodos[childIndex].f))
                break;
            //Si no, intercambiar nodos
            Nodo tmp = nodos[parentIndex];
            nodos[parentIndex] = nodos[childIndex];
            nodos[childIndex] = tmp;
            parentIndex = childIndex;
        }
        return frontItem;
    }

    public void Update(Nodo a) {
        int index = nodos.IndexOf(a);
        //Solo actualizamos el valor de g cuando lo mejoramos (solo puede mejorarse su prioridad, no empeorar)
        while (index > 0)
        {
            //Coge su nodo padre
            int parentIndex = (index - 1) / 2;
            //Si el nodo tiene un costo mayor que el padre, lo dejamos en esta posición
            if (nodos[index].f >= nodos[parentIndex].f)
                break;
            //Si es menor, cambiamos la posición del padre por la del hijo
            Nodo tmp = nodos[index];
            nodos[index] = nodos[parentIndex];
            nodos[parentIndex] = tmp;
            index = parentIndex;
        }
    }

    public int Count
    {
        get { return nodos.Count; }
    }
}

