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
}
