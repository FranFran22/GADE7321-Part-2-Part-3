using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public int numberOfMOves;
    public bool terminalVal;
    public List<Vector2Int> moves = new List<Vector2Int>();


    public Path(int numMoves, bool terminalValue, List<Vector2Int> pathMOves)
    {
        numberOfMOves = numMoves;
        terminalVal = terminalValue;
        moves = pathMOves;
    }
}
