using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    [SerializeField] private MaterialSetter materialSetter;

    public Board board { protected get; set; }
    public Vector2Int occupiedSquare { get; set; }
    public Colour colour { get; set; }
    public bool hasMoved { get; private set; }

    public List<Vector2Int> availableMoves;

    private IObjectTweener tweener;

    public abstract List<Vector2Int> SelectAvailableSquares();

    //methods
    private void Awake()
    {
        availableMoves = new List<Vector2Int>();
        tweener = GetComponent<IObjectTweener>();
        materialSetter = GetComponent<MaterialSetter>();
        hasMoved = false;
    }

    public void SetMaterial(Material material)
    {
        materialSetter.SetAMaterial(material);
    }

    public bool SameTeam(Piece piece)
    {
        return colour == piece.colour;
    }

    public bool CanMoveTo(Vector2Int coords)
    {
        return availableMoves.Contains(coords);
    }

    public virtual void MovePiece(Vector2Int coords)
    {
        Vector3 targetPosition = board.CalculatePosition(coords);

        Debug.Log("target position calculated");

        occupiedSquare = coords;
        hasMoved = true;
        tweener.MoveTo(transform, targetPosition);
    }

    protected void TryToAddMove (Vector2Int coords)
    {
        availableMoves.Add(coords);
    }

    public void SetData(Vector2Int coords, Colour colour, Board board)
    {
        this.colour = colour;
        occupiedSquare = coords;
        this.board = board;
        transform.position = board.CalculatePosition(coords);
    }

    public abstract void DestroyObj();
}
