using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] positions;
    [SerializeField] GameObject[] goldPieces;
    [SerializeField] GameObject[] greyPieces;

    enum State { goldTurn, greyTurn }
    [SerializeField] State currentState;


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
