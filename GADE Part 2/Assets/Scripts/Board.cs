using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform bottomLeftTransform;
    [SerializeField] private float squareSize;

    private Piece[,] grid;
    public Piece selectedPiece;
    public const int BOARD_SIZE = 8;

    private GameController controller;
    private SquareSelectorCreator squareSelector;


    private void Awake()
    {
        squareSelector = GetComponent<SquareSelectorCreator>();
        CreateGrid();
    }

    public void SetDependancies(GameController controller)
    {
        this.controller = controller;
    }

    public Vector3 CalculatePosition(Vector2Int coords)
    {
        return bottomLeftTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
    }

    private void CreateGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }

    public bool HasPiece(Piece piece)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (grid[i, j] == piece)
                    return true;
            }
        }

        return false;
    }

    public void OnSquareSelected(Vector3 inputPosition)
    {
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        Piece piece = GetPieceOnSquare(coords);
        if (selectedPiece)
        {
            if (piece != null && selectedPiece == piece)
                DeselectPiece();

            else if (piece != null && selectedPiece != piece && controller.IsTurnActive(piece.colour))
                SelectPiece(piece);

            else if (selectedPiece.CanMoveTo(coords))
                OnSelectedPieceMoved(coords, selectedPiece);
        }

        else
        {
            if (piece != null && controller.IsTurnActive(piece.colour))
                SelectPiece(piece);
        }
    }

    private void OnSelectedPieceMoved(Vector2Int coords, Piece piece)
    {
        UpdateBoardOnMove(coords, piece.occupiedSquare, piece, null);
        selectedPiece.MovePiece(coords);

        Debug.Log("Piece moved, square occuppied");

        DeselectPiece();
        EndTurn();
    }

    private void EndTurn()
    {
        controller.EndTurn();
    }

    private void UpdateBoardOnMove(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = newPiece;
    }

    private void SelectPiece(Piece piece)
    {
        selectedPiece = piece;
        List<Vector2Int> selection = selectedPiece.availableMoves;
        ShowSelectionSquares(selection);
    }

    private void ShowSelectionSquares(List<Vector2Int> selection)
    {
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();

        for (int i = 0; i < selection.Count; i++)
        {
            Vector3 position = CalculatePosition(selection[i]);
            bool isSquareFree = GetPieceOnSquare(selection[i]) == null;
            squaresData.Add(position, isSquareFree);
        }

        squareSelector.ShowSelection(squaresData);
    }

    private void DeselectPiece()
    {
        selectedPiece = null;
        squareSelector.ClearSelection();
    }

    public Piece GetPieceOnSquare(Vector2Int coords)
    {
        if (CheckIfCoordsAreOnBoard(coords))
            return grid[coords.x, coords.y];

        return null;
    }

    private Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(inputPosition.x / squareSize) + (BOARD_SIZE / 2);
        int y = Mathf.FloorToInt(inputPosition.z / squareSize) + (BOARD_SIZE / 2);

        Debug.Log("Coords Calculated: " + Convert.ToString(x) + ", " + Convert.ToString(y));

        return new Vector2Int(x, y);
    }

    public bool CheckIfCoordsAreOnBoard(Vector2Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
            return false;

        return true;
    }

    public void SetPieceOnBoard(Vector2Int coords, Piece piece)
    {
        if (CheckIfCoordsAreOnBoard(coords))
            grid[coords.x, coords.y] = piece;
    }

    public void CheckForCapture(Vector2Int coords) //make neater 
    {
        // check if the piece is directly behind another piece
        Colour pColour = selectedPiece.colour;

        Vector2Int targetPiece1 = new Vector2Int(0,0);
        Vector2Int targetPiece2 = new Vector2Int(0, 0);

        if (pColour == Colour.Gold)
        {
            targetPiece1 = new Vector2Int(coords.x, coords.y + 1);
            targetPiece2 = new Vector2Int(coords.x - 1, coords.y);
        }

        else if (pColour == Colour.Grey)
        {
            targetPiece1 = new Vector2Int(coords.x, coords.y - 1);
            targetPiece2 = new Vector2Int(coords.x + 1, coords.y);
        }


        // if there is a piece to available for capture, delete it from the board
        if (GetPieceOnSquare(targetPiece1) != null)
        {
            Destroy(GetPieceOnSquare(targetPiece1));
            grid[targetPiece1.x, targetPiece1.y] = null;
            
        }

        if (GetPieceOnSquare(targetPiece2) != null)
        {
            Destroy(GetPieceOnSquare(targetPiece1));
            grid[targetPiece2.x, targetPiece2.y] = null;
        }
    }

    public void CapturePiece(Vector2Int xy)
    {
        CheckForCapture(xy); //check if there is a piece to be captured

        // remove the captured piece from the board


    }
}
