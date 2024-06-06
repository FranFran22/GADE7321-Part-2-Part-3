using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform bottomLeftTransform;
    [SerializeField] private float squareSize;
    [SerializeField] private Colour pColour;
    [SerializeField] private Piece target;

    public static Piece[,] grid;
    public Piece selectedPiece;
    public const int BOARD_SIZE = 8;
    private GameController controller;
    private SquareSelectorCreator squareSelector;
    private bool targetExists;


    private void Awake()
    {
        squareSelector = GetComponent<SquareSelectorCreator>();
        CreateGrid();
    }


#region METHODS
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

    public static bool HasPiece(Piece piece)
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

        if (BoardCheck(coords) == true)
        {
            Capture();
            Debug.Log("Piece captured");
        }

        controller.WinConditions();
             
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

    private int ConvertCoords(double x)
    {
        int output = Convert.ToInt32(Math.Round(x));

        if (output < BOARD_SIZE && output >= 0)
            return output;
        else return 0;
    }

    private bool BoardCheck(Vector2Int coords)
    {
        Piece temp = GetPieceOnSquare(coords);
        pColour = temp.colour;

        //coordinate conversions
        Piece goldTarget1 = grid[ConvertCoords(coords.x), ConvertCoords(coords.y - 1)]; 
        Piece goldTarget2 = grid[ConvertCoords(coords.x + 1), ConvertCoords(coords.y)];
        Piece greyTarget1 = grid[ConvertCoords(coords.x), ConvertCoords(coords.y + 1)];
        Piece greyTarget2 = grid[ConvertCoords(coords.x - 1), ConvertCoords(coords.y)];

        switch (pColour)
        {
            case Colour.Gold:
                if (goldTarget1 != null)
                {
                    targetExists = true;
                    target = goldTarget1;
                }

                if (goldTarget2 != null)
                {
                    targetExists = true;
                    target = goldTarget2;
                }

                else if (goldTarget1 == null && goldTarget2 == null)
                    targetExists = false;

                break;

            case Colour.Grey:
                if (greyTarget1 != null)
                {
                    targetExists = true;
                    target = greyTarget1;
                }

                if (greyTarget2 != null)
                {
                    targetExists = true;
                    target = greyTarget2;
                }

                else if (greyTarget1 == null && greyTarget2 == null)
                    targetExists = false;

                break;
        }

        Debug.Log("Target exists: " + Convert.ToString(targetExists));
        return targetExists;
    }

    public void Capture()
    {
        if (grid[target.occupiedSquare.x, target.occupiedSquare.y].colour != pColour)
        {
            grid[target.occupiedSquare.x, target.occupiedSquare.y] = null;
            target.DestroyObj();
            Debug.Log("Piece destroyed");
        }
    }
    #endregion
}
