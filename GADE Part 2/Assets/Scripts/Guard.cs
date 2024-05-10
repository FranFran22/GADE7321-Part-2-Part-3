using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Piece
{
    // can move any number of tiles in a linear direction 

    private Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down
    };


    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();
        float range = Board.BOARD_SIZE;
        
        foreach (var direction in directions)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = occupiedSquare + direction * i;
                Piece piece = board.GetPieceOnSquare(nextCoords);

                if (!board.CheckIfCoordsAreOnBoard(nextCoords))
                    break;

                if (piece == null)
                    TryToAddMove(nextCoords);

                else if (!piece.SameTeam(this))
                {
                    TryToAddMove(nextCoords);
                    break;
                }

                else if (piece.SameTeam(this))
                    break;
            }
        }
           
        return availableMoves;
    }
}
