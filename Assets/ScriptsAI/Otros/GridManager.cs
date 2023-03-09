using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridManager<T>
{
    private T[,] gridArray;
    private int columns;
    private int rows;
    private float cellSize;
    private Vector3 origin;
    public Agent leader;

    public GridManager(int columns, int rows, float cellSize, Vector3 origin)
    {
        this.columns = columns;
        this.rows = rows;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new T[columns, rows];
    }

    // Dada una posicion del grid retorna la posicion en el plano
    public Vector3 GetPosition(int i, int j)
    {
        float x = origin.x + (i * cellSize);
        float z = origin.z + (j * cellSize);

        return new Vector3(x, 0f, z);
    }

    // Dada una posicion obtenemos el index (cuadrante/ casillero) del grid donde se encuentra
    public Tuple<int, int> GetGridIndex(Vector3 position)
    {
        int i = Mathf.RoundToInt((position.x - origin.x) / cellSize);
        int j = Mathf.RoundToInt((position.z - origin.z) / cellSize);

        return new Tuple<int, int>(i, j);
    }

    public Vector3 GetRelativePosition(Vector3 position, Agent leader)
    {
        int i = Mathf.RoundToInt((position.x - origin.x) / cellSize);
        int j = Mathf.RoundToInt((position.z - origin.z) / cellSize);

        int leaderI = Mathf.RoundToInt((leader.Position.x - origin.x) / cellSize);
        int leaderJ = Mathf.RoundToInt((leader.Position.z - origin.z) / cellSize);

        int relativeI = i - leaderI;
        int relativeJ = j - leaderJ;

        return new Vector3(relativeI * cellSize, 0f, relativeJ * cellSize);
    }

    // getters y setters
    public T GetCellData(int i, int j)
    {
        return gridArray[i, j];
    }

    public void SetCellData(int i, int j, T data)
    {
        gridArray[i, j] = data;
    }

    public void DrawGrid()
    {
        for (int i = 0; i <= columns; i++)
        {
            Vector3 startPoint = GetPosition(i, 0f, 0f);
            Vector3 endPoint = GetPosition(i, 0f, rows);
            Debug.DrawLine(startPoint, endPoint, Color.white, 100f);
        }

        for (int j = 0; j <= rows; j++)
        {
            Vector3 startPoint = GetPosition(0, 0f, j);
            Vector3 endPoint = GetPosition(columns, 0f, j);
            Debug.DrawLine(startPoint, endPoint, Color.white, 100f);
        }
    }
}
