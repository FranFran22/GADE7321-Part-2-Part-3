using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Colour colour { get; set; }
    public Board board { get; set; }
    public List<Piece> activePieces { get; private set; }

    //constructor
    public Player (Colour colour, Board board)
    {
        this.colour = colour;
        this.board = board;
        activePieces = new List<Piece>();
    }

    //methods
    public void AddPiece(Piece piece)
    {
        if (!activePieces.Contains(piece))
            activePieces.Add(piece);
    }

    public void RemovePiece(Piece piece)
    {
        if (activePieces.Contains(piece))
            activePieces.Remove(piece);
    }

    public void GenerateMoves()
    {
        foreach (var piece in activePieces)
        {
            if (board.HasPiece(piece))
                piece.SelectAvailableSquares();
        }
    }
}
