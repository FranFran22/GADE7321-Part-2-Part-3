using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    private Node parent;
    private Node child;
    public static Node[] children;

    public Piece[,] gameState;
    public Piece[,] nextState;
    public int numOfVisits;
    Dictionary<int, int> results = new Dictionary<int, int>();
    private Vector2Int action;

    private List<Vector2Int> untriedMoves = new List<Vector2Int>();
    public List<Vector2Int> moves = new List<Vector2Int>();

    private GameController gc;


    public Node(Vector2Int parentAction, Node parentN, Piece[,] state) 
    {
        this.gameState = state; //game state
        this.parent = parentN; //parent node
        this.action = parentAction; //action that took place in the parent node
        this.moves = UntriedActions(state); //possible actions from current node
        this.numOfVisits = 0;

        results[1] = 0;
        results[-1] = 0;

        this.untriedMoves = UntriedActions(state); //possible moves not yet perfomed
    }

    void Start()
    {
        gc = GetComponent<GameController>();
    }

    void Update()
    {
        gameState = Board.grid;
    }

    #region METHODS
    public static List<Vector2Int> UntriedActions(Piece[,] state)
    { 
        List<Vector2Int> actions = new List<Vector2Int>();

        actions = possibleActions(state);

        return actions;
    }

    public static List<Vector2Int> possibleActions(Piece[,] state)
    {
        foreach (Piece p in state)
        {
            if (Board.HasPiece(p))
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

    public static Node BestChild(Node child)
    {
        int n, q;
        double c_param = 0.1; //controls the exploration (term 2)
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

        return children[maxIndex];
    }

    private Node Expand(Node current, Piece[,] state)
    {
        action = RolloutPolicy(state);
        nextState = Move(state, action);

        child = new Node(action, current, nextState);

        children.Append(child);
        return child;
    }

    public Vector2Int RolloutPolicy(Piece[,] state)
    {
        //pick a random piece on the board
        int i = UnityEngine.Random.Range(0, 7);
        int j = UnityEngine.Random.Range(0, 7);

        Piece randomPiece = state[i, j];

        while (state[i, j] == null)
        {
            i = UnityEngine.Random.Range(0, 7);
            j = UnityEngine.Random.Range(0, 7);

            randomPiece = state[i, j];
        }

        //calculate its possible moves
        List<Vector2Int> calculatedMoves = randomPiece.SelectAvailableSquares();

        int x = UnityEngine.Random.Range(0, calculatedMoves.Count - 1);
        Vector2Int newAction = calculatedMoves[x];

        return newAction;
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
                currentNode = BestChild(selectedNode);
        }

        return currentNode;
    }

    private int SimulationRollout(Piece[,] state) //simulates the whole game until there is an outcome
    {
        Piece[,] currentRolloutState = state; //current game state
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        while (isGameOver(state) != true)
        {
            possibleMoves = UntriedActions(currentRolloutState);
            action = RolloutPolicy(state);
            currentRolloutState = Move(action); 
        }

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
            reward = SimulationRollout(state);
            Backpropagate(reward, node);
        }

        return BestChild(chosenNode);
    }

    private void Backpropagate(int result, Node currentNode)
    {
        currentNode.numOfVisits++;
        currentNode.results[result]++;

        if (currentNode.parent != null)
        {
            Backpropagate(result, currentNode.parent);
        }
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

    private Piece[,] Move(Piece[,] state, Vector2Int move) //UNFINISHED
    {
        return null;
    }
    #endregion
}
