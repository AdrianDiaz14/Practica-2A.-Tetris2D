using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private float lastFall = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Default position not valid? Then it's game over
        if (!IsValidBoard())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame.
    // Implements all piece movements: right, left, rotate and down.
    void Update()
    {
        // Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);

            // See if it's valid
            if (IsValidBoard())
                // It's valid. Update grid.
                UpdateBoard();
            else
                // Its not valid. revert.
                transform.position += new Vector3(1, 0, 0);
        }
        // Implement Move Right (key RightArrow)
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Modify position
            transform.position += new Vector3(1, 0, 0);

            // See if it's valid
            if (IsValidBoard())
                // It's valid. Update grid.
                UpdateBoard();
            else
                // Its not valid. revert.
                transform.position += new Vector3(-1, 0, 0);
        }
        // Implement Rotate, rotates the piece 90 degrees (Key UpArrow)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Modify position
            transform.Rotate(0, 0, -90);

            // See if it's valid
            if (IsValidBoard())
                // It's valid. Update grid.
                UpdateBoard();
            else
                // Its not valid. revert.
                transform.Rotate(0, 0, 90);
        }
        // Implement move Downwards and Fall (each second)
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1)
        {
            MovePiece(new Vector3(0, -1, 0));
            lastFall = Time.time;
        }
    }

    void MovePiece(Vector3 direction)
    {
        // Modify position
        transform.position += direction;

        // See if it's valid
        if (IsValidBoard())
        {
            // It's valid. Update grid.
            UpdateBoard();
        }
        else
        {
            // It's not valid. revert.
            transform.position -= direction;

            // If the move was downward, the piece cannot move further down and needs to be set in place
            if (direction == new Vector3(0, -1, 0))
            {
                // Call DeleteFullRows from the Board script to eliminate any full rows
                Board.DeleteFullRows();
                // Spawn the next piece
                Object.FindFirstObjectByType<Spawner>().SpawnNext();
                // Disable this script for the current piece
                enabled = false;
            }
        }
    }


    // Updates the board with the current position of the piece. 
    void UpdateBoard()
    {
            // A: Primero tienes que hacer un bucle sobre el tablero y hacer que las posiciones actuales de la pieza sean nulas.
            for (int y = 0; y < Board.h; ++y)
            {
                for (int x = 0; x < Board.w; ++x)
                {
                    if (Board.grid[x, y] != null && Board.grid[x, y].transform.parent == transform)
                    {
                        Board.grid[x, y] = null;
                    }
                }
            }

            // B: Añadir las nuevas posiciones de los bloques de la pieza al tablero.
            foreach (Transform child in transform)
            {
                Vector2 v = Board.RoundVector2(child.position);
                Board.grid[(int)v.x, (int)v.y] = child.gameObject;
            }
    }

    // Returns if the current position of the piece makes the board valid or not
    bool IsValidBoard()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Board.RoundVector2(child.position);

            // Not inside Border?
            if (!Board.InsideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Board.grid[(int)v.x, (int)v.y] != null &&
                Board.grid[(int)v.x, (int)v.y].transform.parent != transform)
                return false;
        }
        return true;
    }
}
