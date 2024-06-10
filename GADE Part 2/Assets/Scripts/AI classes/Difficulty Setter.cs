using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySetter : MonoBehaviour
{
    [SerializeField] private double param;

    public void SetDifficultyParameter()
    {
        Node.parameter = param;
    }
}
