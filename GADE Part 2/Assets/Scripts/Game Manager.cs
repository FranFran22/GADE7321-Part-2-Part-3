using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[RequiresComponent(typeof(PieceCreator))]
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] boardTiles;
    [SerializeField] GameObject[] goldPieces;
    [SerializeField] GameObject[] greyPieces;
    [SerializeField] public static GameObject[] positions;

    [SerializeField] private Board board;

    private PieceCreator pieceCreator;
    private Player greyPlayer;
    private Player goldPlayer;
    private Player activePlayer;

    enum State { goldTurn, greyTurn }
    [SerializeField] State currentState;

    private void Awake()
    {
        CreatePlayers();
    }



    void Start()
    {
        
    }


    void Update()
    {
        
    }


    private void CreatePlayers()
    {
        goldPlayer = new Player(Colour.Gold, board);
        greyPlayer = new Player(Colour.Grey, board);
    }

    private void StartGame()
    {
        //CreatePieces(startingLayout);
        activePlayer = goldPlayer;
        GeneratePlayerMoves(activePlayer);
    }

    private void GeneratePlayerMoves(Player player)
    {
        player.GenerateMoves();
    }
}
