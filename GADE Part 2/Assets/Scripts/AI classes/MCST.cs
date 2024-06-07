using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MCST : MonoBehaviour
{
    // Monte Carlo Search Tree class

    // Selection:
    //   - selection formula
    //   - keep track of if the node has been chosen

    // Expansion:
    //   - choose the moves to move to the next node

    // Simulation:
    //   - expand each node (& chosen moves) to find the terminal state (either 0 or 1)

    // Backpropagation:
    //   - traverse back along the chosen nodes

    private int sims; //simulations after the i-th move
    private int wins; //wins
    private double C = Math.Sqrt(2); //exploration parameter
    private int parentSims; //simulations for the parent node

    private Vector2Int action;
    private Node childNode;
    private Piece[,] nextState;
    private Node[] children;
    private Piece[,] gameState;


    void Start()
    {

    }

    
    void Update()
    {
        gameState = Board.grid;
    }


    #region Methods
    public double SelectionFunction(int w, int n, double c, int t)
    {
        double value = (w / n) + c * Math.Sqrt(t) / n;
        return value;
    }

    private int CalculateVisits(Node node1)
    {
        return node1.numOfVisits;
    }

    private Node Expand(Node current)
    {
        //pick a random piece on the board
        int i = UnityEngine.Random.Range(0, 7);
        int j = UnityEngine.Random.Range(0, 7);

        Piece randomPiece = gameState[i,j];

        while (gameState[i,j] == null)
        {
            i = UnityEngine.Random.Range(0, 7);
            j = UnityEngine.Random.Range(0, 7);

            randomPiece = gameState[i, j];
        }

        //calculate its possible moves
        List<Vector2Int> calculatedMoves = randomPiece.SelectAvailableSquares();

        int x = UnityEngine.Random.Range(0, calculatedMoves.Count - 1);
        action = calculatedMoves[x];

        //change the game state
        //nextState = change grid[]

        childNode = new Node(action, current, nextState);

        children.Append(childNode);
        return childNode;
    }

    private bool isNodeTerminal()
    {
        return isGameOver();
    }

    private void SimulationRollout()
    {

    }

    private void Backpropagation()
    {

    }

    private Piece[,] ChangedState()
    {
        return null;
    }

    private bool isGameOver()
    {
        // check if the current node is terminal
        bool value = false;

        foreach (Piece piece in gameState)
        {
            if (Board.HasPiece(piece))
            {
                List<Vector2Int> temp = piece.SelectAvailableSquares();

                if (temp != null)
                    value = false;

                else value = true;
            }
        }

        return value;
    }

    #endregion
}
