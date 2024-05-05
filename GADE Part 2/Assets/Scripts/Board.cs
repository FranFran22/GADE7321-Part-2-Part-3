using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform bottomLeftTransform;
    [SerializeField] private float squareSize;

    public Vector3 CalculatePosition(Vector2Int coords)
    {
        return bottomLeftTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
    }
}
