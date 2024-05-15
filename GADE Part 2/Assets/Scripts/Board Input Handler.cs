using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInputHandler : MonoBehaviour, IInputHandler
{
    private Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }

    public void ProcessInput(Vector3 position, GameObject selectedObject, Action callback)
    {
        board.OnSquareSelected(position);
        Debug.Log("Selected Square pos: " + position);
    }
}