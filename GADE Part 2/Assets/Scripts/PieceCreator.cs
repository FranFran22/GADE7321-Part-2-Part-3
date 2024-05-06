using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;

public class PieceCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] pieces;
    [SerializeField] private Material gold;
    [SerializeField] private Material grey;

    private Dictionary<string, GameObject> nameToPieceDictionary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (var piece in pieces)
        {
            nameToPieceDictionary.Add(piece.GetComponent<Piece>().GetType().ToString(), piece);
        }
      
    }

    public GameObject CreatePiece(Type type)
    {
        GameObject prefab = nameToPieceDictionary[type.ToString()];

        if (prefab)
        {
            GameObject newPiece = Instantiate(prefab);
            return newPiece;
        }

        return null;
    }

    public Material GetColour(Colour colour)
    {
        return colour == Colour.Gold ? gold : grey;
    }
}
