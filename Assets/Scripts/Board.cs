using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static int w = 10;
    public static int h = 20;
    public static GameObject[,] grid = new GameObject[w, h];

    // Rounds Vector2 so does not have decimal values
    // Used to force Integer coordinates (without decimals) when moving pieces
    public static Vector2 RoundVector2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),
                           Mathf.Round(v.y));
    }

    // Returns true if pos (x,y) is inside the grid, false otherwise
    public static bool InsideBorder(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;

        if (x >= 0 && x < w && y >= 0 && y < h)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // TODO: Deletes all GameObjects in the row Y and set the row cells to null.
    // You can use Destroy function to delete the GameObjects.
    public static void DeleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y]);
                grid[x, y] = null;
            }
        }
    }


    // Moves all gameobject on row Y to row Y-1
    // 2 thing change:
    //  - All GameObjects on row Y go from cell (X,Y) to cell (X,Y-1)
    //  - Changes the GameObject transform position Gameobject.transform.position += new Vector3(0, -1, 0).
    public static void DecreaseRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                // Desplaza el GameObject a la fila inferior
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                // Actualiza la posición del GameObject en el tablero
                grid[x, y - 1].transform.position += new Vector3(0, -1, 0);
            }
        }
    }


    // Decreases all rows above Y
    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
        {
            DecreaseRow(i);
        }
    }


    // Return true if all cells in a row have a GameObject (are not null), false otherwise
    public static bool IsRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }


    // Deletes full rows
    public static void DeleteFullRows()
    {
        for (int y = 0; y < h; ++y)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                --y;
            }
        }
    }

}