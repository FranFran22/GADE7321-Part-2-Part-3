using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Node
{
    //NOTES:
    // > need to select only the grey pieces dummy
    // > code is running weirdly, needs more bugfixing

    [SerializeField] private GameController gc;

    private Node parent;
    private Node child;
    public static Node chosenN;
    public static Node[] children;

    public Piece chosenPiece;
    public Vector2Int chosenMove;
    public static Vector2Int nullVector;

    public Piece[,] gameState;
    public Piece[,] nextState;
    public int numOfVisits;
    Dictionary<int, int> results = new Dictionary<int, int>();
    public Vector2Int action;
    public static double parameter;

    public static List<Vector2Int> moves = new List<Vector2Int>();
    public static List<Node> bestNodes = new List<Node>(); //stores the best chosen path


    public Node(Piece[,] state, Vector2Int parentAction, Node parentN = null, Piece piece = null) 
    {
        this.gameState = state; //game state
        this.parent = parentN; //parent node
        this.action = parentAction; //action that took place in the parent node
        this.chosenPiece = piece;
        moves = UntriedActions(state); //possible actions from current node
        this.numOfVisits = 0;

        results[1] = 0;
        results[-1] = 0;
    }

    void Start()
    {
        nullVector = Vector2Int.zero;
        Debug.Log(parameter);
    }

    void Update()
    {
        gameState = Board.grid;
    }

    #region METHODS
    public static Node MCST(Piece[,] state)
    {
        Debug.Log(parameter);

        Node root = new Node(state, nullVector);
        chosenN = root.BestAction(root, state);

        Debug.Log("main runs");
        return chosenN;
    }
    
    public static List<Vector2Int> UntriedActions(Piece[,] state)
    {
        List<Vector2Int> actions = possibleActions(state);
        return actions;
    }

    public static List<Vector2Int> possibleActions(Piece[,] state)
    {
        foreach (Piece p in state)
        {

            if (p != null)
            {
                List<Vector2Int> temp = p.SelectAvailableSquares();
                moves.AddRange(temp);
            }
        }

        return moves;
    }

    public int WinLoss()
    {
        int wins = results[1];
        int losses = results[-1];

        return (wins - losses);
    }

    public static int CalculateVisits(Node node)
    {
        return node.numOfVisits;
    }

    public static bool isGameOver(Piece[,] state)
    {
        // check if the current node is terminal
        bool value = false;

        foreach (Piece piece in state)
        {
            if (piece != null)
            {
                List<Vector2Int> temp = piece.SelectAvailableSquares();

                if (temp != null)
                    value = false;

                else value = true;
            }
        }

        return value;
    }

    public bool isNodeTerminal(Node node)
    {
        return isGameOver(node.gameState);
    }

    public bool IsFullyExpanded(Node node)
    {

        if (possibleActions(node.gameState) == null)
            return true;

        else return false;
    }

    public static Node BestChild()
    {
        int n, q;
        double c_param = parameter;//0.1; //controls the exploration (term 2)
        List<double> childWeighting = new List<double>();

        foreach (Node node in children)
        {
            n = CalculateVisits(node);
            q = node.WinLoss();

            //UCB1 formula
            double choicesWeights = (q / n) + (c_param * Math.Sqrt(2 * Math.Log(n)) / n);
            childWeighting.Append(choicesWeights);
        }

        int maxIndex = 0;
        double maxValue = int.MinValue;

        for (int i = 0; i < childWeighting.Count; i++)
        {
            if (childWeighting[i] > maxValue)
            {
                maxValue = childWeighting[i];
                maxIndex = i;
            }
        }

        Debug.Log("Step 1: Selection completed");
        return children[maxIndex];
    }

    private Node Expand(Node current, Piece[,] state) //FIX!!
    {
        action = RolloutPolicy(state);
        nextState = Move(action, current);

        child = new Node(nextState, action, current);

        children.Append(child); //this is returning a null value ??
        Debug.Log("Step 2: Expansion completed");
        return child;
    }

    public Vector2Int RolloutPolicy(Piece[,] state)
    {
        //pick a random piece on the board
        int i = UnityEngine.Random.Range(0, 7);
        int j = UnityEngine.Random.Range(0, 7);

        chosenPiece = state[i, j];

        while (state[i, j] == null)
        {
            i = UnityEngine.Random.Range(0, 7);
            j = UnityEngine.Random.Range(0, 7);

            chosenPiece = state[i, j];
        }

        //calculate its possible moves
        List<Vector2Int> calculatedMoves = chosenPiece.SelectAvailableSquares();

        while (calculatedMoves.Count < 1)
        {
            i = UnityEngine.Random.Range(0, 7);
            j = UnityEngine.Random.Range(0, 7);

            chosenPiece = state[i, j];
            calculatedMoves = chosenPiece.SelectAvailableSquares();
        }

        Debug.Log(calculatedMoves.Count);

        int x = UnityEngine.Random.Range(0, calculatedMoves.Count-1);
        chosenMove = calculatedMoves[x];

        return chosenMove;
    }

    private Node TreePolicy(Node selectedNode, Piece[,] state)
    {
        //selects node to rollout
        Node currentNode = selectedNode;

        while (!isNodeTerminal(selectedNode))
        {
            if (!IsFullyExpanded(selectedNode))
                return Expand(currentNode, state);
            else
                currentNode = BestChild();
        }

        return currentNode;
    }

    private int SimulationRollout(Piece[,] state, Node node) //simulates the whole game until there is an outcome
    {
        Piece[,] currentRolloutState = state; //current game state
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        while (isGameOver(state) != true)
        {
            possibleMoves = UntriedActions(currentRolloutState);
            action = RolloutPolicy(state);
            currentRolloutState = Move(action, node); 
        }

        Debug.Log("Step 3: Simulation completed");
        return GameResult();
    }

    public Node BestAction(Node node, Piece[,] state)
    {
        int sim_no = 100;
        int reward;

        Node chosenNode = null;

        for (int i = 0; i < sim_no; i++)
        {
            chosenNode = TreePolicy(node, state);
            reward = SimulationRollout(state, node);
            Backpropagate(reward, node);
        }

        Debug.Log("best action runs");
        return BestChild();
    }

    private void Backpropagate(int result, Node currentNode)
    {
        currentNode.numOfVisits++;
        currentNode.results[result]++;

        if (currentNode.parent != null)
        {
            Backpropagate(result, currentNode.parent);
        }

        Debug.Log("Step 4: Backpropagation completed");
    }

    private int GameResult()
    {
        int result = 0;

        gc.WinConditions();

        if (gc.greyWin)
            result = 1;

        if (gc.goldWin)
            result = -1;

        if (gc.greyCrownExists && gc.goldCrownExists)
            result = 0;

        return result;
    }

    private Piece[,] Move(Vector2Int move, Node current)
    {
        Board.UpdateBoardOnMove(move, chosenPiece.occupiedSquare, chosenPiece, null);
        chosenPiece.MovePiece(move);
        current.chosenPiece = chosenPiece;

        return Board.grid;
    }
    #endregion
}
