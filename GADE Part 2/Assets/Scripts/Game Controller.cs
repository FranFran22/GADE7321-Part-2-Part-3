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
    public Player goldPlayer;
    public Player greyPlayer;
    public Player activePlayer;
    public bool victory, goldWin, greyWin, goldCrownExists, greyCrownExists;

    private Vector2Int chosenMove;
    private Node nodeChosen;
    private Piece[,] gameStateGrid;

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
        CheckForCrowns();
        WinConditions();
        Victory();

        gameStateGrid = Board.grid;
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
        if (activePlayer == goldPlayer)
        {
            GeneratePlayerMoves(activePlayer);
            ChangeActivePlayer();

            Debug.Log("changed to grey player");
        }
        
        if (activePlayer == greyPlayer)
        {
            

            nodeChosen = Node.MCST(gameStateGrid);
            Vector2Int move = nodeChosen.action;

            board.OnAIMove(move, nodeChosen.chosenPiece);

            ChangeActivePlayer();

            Debug.Log("changed to gold player");
        }
    }

    private void ChangeActivePlayer()
    {
        activePlayer = activePlayer == goldPlayer ? greyPlayer : goldPlayer;
    }

    private Player GetOpponentToPlayer(Player player)
    {
        return player == goldPlayer ? greyPlayer : goldPlayer;
    }

    public void WinConditions() 
    {
        //check if crowns are still on board
        if (goldCrownExists == false)
            greyWin = true;

        if (greyCrownExists == false)
            goldWin = true;
    }

    public void CheckForCrowns()
    {
        GameObject[] crowns = GameObject.FindGameObjectsWithTag("Crown");

        foreach (GameObject obj in crowns)
        {
            if (obj.GetComponent<Renderer>().material.name == "Gold (Instance)")
                goldCrownExists = true;

            if (obj.GetComponent<Renderer>().material.name == "Grey (Instance)")
                greyCrownExists = true;
        }

        //Debug.Log("grey: " + greyCrownExists);
        //Debug.Log("gold: " + goldCrownExists);
    }

    private void Victory()
    {
        if (goldWin == true)
        {
            victoryCanvas.SetActive(true);
            victoryText.text = "Gold wins";
        }

        if (greyWin == true)
        {
            victoryCanvas.SetActive(true);
            victoryText.text = "Grey wins";
        }

        else victoryCanvas.SetActive(false);
    }
}
