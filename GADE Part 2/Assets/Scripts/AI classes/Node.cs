using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // nodes to be used in the MCST class

    private Node parent;
    private Node child;
    List<Vector2Int> moves = new List<Vector2Int>();
    private Piece[,] gameState;
    public int numOfVisits;
    Dictionary<int, int> results = new Dictionary<int, int>();
    private Vector2Int action;
    List<Vector2Int> untriedMoves = new List<Vector2Int>();


    public Node(Vector2Int parentAction, Node parentN, Piece[,] state ) 
    {
        gameState = state; //game state
        parent = parentN; //parent node
        action = parentAction; //action that took place in the parent node
        moves = null; //possible actions from current node
        numOfVisits = 0;

        results[1] = 0;
        results[-1] = 0;

        untriedMoves = UntriedActions(); //possible moves not yet perfomed
    }

    void Update()
    {
        gameState = Board.grid;
    }

    List<Vector2Int> UntriedActions()
    {
        List<Vector2Int> actions = new List<Vector2Int>();

        //calculate all moves not performed yet

        return actions;
    }

    private List<Vector2Int> possibleActions()
    {
        foreach (Piece p in gameState)
        {
            if (Board.HasPiece(p))
            {
                List<Vector2Int> temp = p.SelectAvailableSquares();
                moves.AddRange(temp);
            }
        }

        return moves;
    }

    private int WinLoss()
    {
        int wins = results[1];
        int losses = results[-1];

        return (wins - losses);
    }
}
