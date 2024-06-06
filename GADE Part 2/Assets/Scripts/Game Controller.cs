using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using TMPro;


public class GameController : MonoBehaviour
{
    [SerializeField] private BoardLayout startingLayout1;
    [SerializeField] private Board board;
    [SerializeField] private Material gold;
    [SerializeField] private Material grey;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private GameObject victoryCanvas;

    private PieceCreator pieceCreator;
    private Player goldPlayer;
    private Player greyPlayer;
    private Player activePlayer;
    private bool victory, goldWin, greyWin, goldCrownExists, greyCrownExists;

    private void Awake()
    {
        pieceCreator = GetComponent<PieceCreator>();
        CreatePlayers();
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        Victory();
    }

    private void StartGame()
    {
        board.SetDependancies(this);
        CreateGame(startingLayout1);
        activePlayer = goldPlayer;
        GeneratePlayerMoves(activePlayer);
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

        board.SetPieceOnBoard(coords, newPiece);

        Player currentPlayer = colour == Colour.Gold ? goldPlayer : greyPlayer;
        currentPlayer.AddPiece(newPiece);
    }

    private void CreatePlayers()
    {
        goldPlayer = new Player(Colour.Gold, board);
        greyPlayer = new Player(Colour.Grey, board);
    }

    private void GeneratePlayerMoves(Player player)
    {
        player.GenerateMoves();
    }

    public bool IsTurnActive(Colour colour)
    {
        return activePlayer.colour == colour;
    }

    public void EndTurn()
    {
        GeneratePlayerMoves(activePlayer);
        GeneratePlayerMoves(GetOpponentToPlayer(activePlayer));
        ChangeActivePlayer();
    }

    private void ChangeActivePlayer()
    {
        activePlayer = activePlayer == goldPlayer ? greyPlayer : goldPlayer;
    }

    private Player GetOpponentToPlayer(Player player)
    {
        return player == goldPlayer ? greyPlayer : goldPlayer;
    }

    public void WinConditions() //working on this
    {
        // need to create a scenario that can turn the "crowns exist" bold to "false" (perma true atm)

        GameObject[] crowns = GameObject.FindGameObjectsWithTag("Crown");

        foreach (GameObject obj in crowns)
        {
            if (obj.GetComponent<Renderer>().material.name == "Gold (Instance)")
                goldCrownExists = true;
              
            if (obj.GetComponent<Renderer>().material.name == "Grey (Instance)")
                greyCrownExists = true;
        }

        Debug.Log("grey: " + greyCrownExists);
        Debug.Log("gold: " + goldCrownExists);

        //check if crowns are still on board
        if (goldCrownExists == false)
            greyWin = true;

        if (greyCrownExists == false)
            goldWin = true;
    }

    private void Victory()
    {
        if (victory == true)
        {
            victoryCanvas.SetActive(true);

            if (goldWin == true)
                victoryText.text = "Gold wins";

            if (greyWin == true)
                victoryText.text = "Grey wins";
        }

        else victoryCanvas.SetActive(false);
    }
}
