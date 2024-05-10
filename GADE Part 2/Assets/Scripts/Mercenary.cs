using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary : Piece
{
    // can move 3 tiles diagonally

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1)
    };


    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();
        float range = 3;

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

        Debug.Log("merc moves calculated");

        return availableMoves;
    }
}