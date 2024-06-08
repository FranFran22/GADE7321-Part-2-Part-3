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
    private Piece[,] nextState;
    private Piece[,] gameState;
    private Piece[,] currentRolloutState;
    private Node[] children;
    private Node currentNode;
    private Node childNode;


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

    

    

    

    

    

    

    

    private Piece[,] ChangedState()
    {
        return null;
    }

    #endregion
}
