using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // nodes to be used in the MCST class

    int nodeNum;
    List<Vector2Int> calculatedMoves();
    Node parentNode;


    public Node(List<Vector2Int> moves(), int num, Node parent) 
    {
        nodeNum = num;
        calculatedMoves = moves;
        parentNode = parent;
    }

}
