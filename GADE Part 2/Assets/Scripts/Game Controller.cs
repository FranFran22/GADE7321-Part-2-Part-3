using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;


//[RequiresComponent(typeof(PieceCreator))]
public class GameController : MonoBehaviour
{
    [SerializeField] private BoardLayout startingLayout1;
    [SerializeField] private Board board;

    private PieceCreator pieceCreator;

    private void Awake()
    {
        pieceCreator = GetComponent<PieceCreator>();
    }

    void Start()
    {
        StartGame();
    }


    void Update()
    {
        
    }

    private void StartGame()
    {
        CreateGame(startingLayout1);
    }

    private void CreateGame(BoardLayout layout)
    {
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {
            Vector2Int squareCoords = layout.GetSquareCoords(i);
            Colour colour = layout.GetSquareColour(i);
            string typeName = layout.GetSquareName(i);

            Type type = Type.GetType(typeName);
            CreatePieceAndInitialise(squareCoords, colour, type);
        }
    }

    private void CreatePieceAndInitialise(Vector2Int coords, Colour colour, Type type)
    {
        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
        newPiece.SetData(coords, colour, board);

        Material teamMaterial = pieceCreator.GetColour(colour);
        newPiece.SetMaterial(teamMaterial);
    }
}
