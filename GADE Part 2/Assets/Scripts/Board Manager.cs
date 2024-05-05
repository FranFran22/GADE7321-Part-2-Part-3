using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] GameObject tile; //the selected tile to move to
    [SerializeField] GameObject piece; //the selected piece (to move)


    void Start()
    {

    }


    void Update()
    {
        if (Raycast.hitObject.tag == "Tile")
        {
            tile = Raycast.hitObject;
            Debug.Log("tile " + tile + " selected");
        }

        else
        { 
            if (Raycast.hitObject.tag == "Grey" || Raycast.hitObject.tag == "Gold")
            {
                piece = Raycast.hitObject;
                Debug.Log("piece " + piece + " selected");
            }
        }
    }


    public void Move(GameObject Tile, GameObject Piece)
    {
        //check if the piece can move to the selected tile
        //is the tile occupied?
        //if no, move the piece
    }


    private void SelectedPiece(Piece piece)
    {
        //ShowSelectedSquares(selection);
    }

    private void ShowSelectedSquares(List<Vector2Int> selection)
    {
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();

        for (int i = 0; i < selection.Count; i++)
        {
            //Vector3 position = CalculatePositionFromCoords(selection[i]);
            //bool isSquareFree = GetPieceOnSquare(selection[i]) == null;
            //squaresData.Add(position, isSquareFree);
        }

        //SquareSelector.ShowSelection(squaresData);
    }
}
