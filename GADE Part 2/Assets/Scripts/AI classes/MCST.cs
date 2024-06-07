using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private Node parent;
    private Node child;
    List<Vector2Int> moves = new List<Vector2Int>();
    private Piece[,] gameState;
    private int numOfVisits;
    Dictionary<int, int> results = new Dictionary<int, int>();
    private Vector2Int action;

    public List<Node> nodes = new List<Node>(); // stores all the generated nodes


    public MCST(Piece[,] state, Node parentN)
    {
        gameState = state;
        parent = parentN;
        //parent action --> the action that the parent carried out
        //actions = null; // --> possible actions from current node
        numOfVisits = 0;

        results[1] = 0;
        results[-1] = 0;

        //untriedActions = null;
    }

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
        double utility = (w / n) + c * Math.Sqrt(t) / n;

        return utility;
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

    private int CalculateVisits(Node node1)
    {
        return node1.timesVisited;
    }

    private Node Expand()
    {
        //int range = moves.GetRange();
        //int rnd = UnityEngine.Random.Range(0, range - 1);

        //action = moves;

        return null;
    }

    private bool isNodeTerminal()
    {
        // check if the current node is terminal
        return false;
    }

    private void SimulationRollout()
    {

    }

    #endregion
}
