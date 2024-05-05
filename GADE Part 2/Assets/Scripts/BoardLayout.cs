using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Scriptable Objects/Board/Layout")]
public class BoardLayout : ScriptableObject
{
    [Serializable]
    private class BoardSetup
    {
        public Vector2Int position;
        public PieceType pieceType;
        public Colour colour;
    }

    [SerializeField] private BoardSetup[] boardSquares;

    public int GetPiecesCount()
    {
        return boardSquares.Length;
    }

    public Vector2Int GetSquareCoords(int index)
    {
        if (boardSquares.Length <= index)
        {
            Debug.Log("Index out of range");
            return new Vector2Int(-1, - 1);
        }

        return new Vector2Int(boardSquares[index].position.x - 1, boardSquares[index].position.y - 1);
    }

    public string GetSquareName(int index)
    {
        if (boardSquares.Length <= index)
        {
            Debug.Log("Index out of range");
            return "";
        }

        return boardSquares[index].pieceType.ToString();
    }

    public Colour GetSquareColour(int index)
    {
        if (boardSquares.Length <= index)
        {
            Debug.Log("Index out of range");
            return Colour.Grey;
        }

        return boardSquares[index].colour;
    }
}
