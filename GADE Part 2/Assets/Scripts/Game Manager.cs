using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] boardTiles;
    [SerializeField] GameObject[] goldPieces;
    [SerializeField] GameObject[] greyPieces;
    [SerializeField] public static GameObject[] positions;

    enum State { goldTurn, greyTurn }
    [SerializeField] State currentState;


    void Start()
    {
        
    }


    void Update()
    {
        
    }


    public void PositionManager()
    {

    }
}
