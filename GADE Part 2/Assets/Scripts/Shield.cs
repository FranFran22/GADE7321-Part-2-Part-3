using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Piece
{
    // can move 2 tiles linearly, OR 2 tiles diagonally

    private Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1)
    };

    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();
        float range = 2;

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
