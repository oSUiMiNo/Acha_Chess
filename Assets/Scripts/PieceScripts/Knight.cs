using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece_Common
{
    protected override void Define_PieceType() { type = PieceType.Knight; }
    public override List<Vector2Int> GetMovePlaceList(Vector2Int initialplace2)
    {
        GetSelectedInfo();
        movePlaceList = new List<Vector2Int>();

        AddMoveList(initialplace2 + new Vector2Int(-1, 2));
        RemoveMoveList();

        AddMoveList(initialplace2 + new Vector2Int(1, 2));
        RemoveMoveList();

        AddMoveList(initialplace2 + new Vector2Int(2, 1));
        RemoveMoveList();

        AddMoveList(initialplace2 + new Vector2Int(-2, 1));
        RemoveMoveList();

        AddMoveList(initialplace2 + new Vector2Int(2, -1));
        RemoveMoveList();

        AddMoveList(initialplace2 + new Vector2Int(-2, -1));
        RemoveMoveList();

        AddMoveList(initialplace2 + new Vector2Int(1, -2));
        RemoveMoveList();

        AddMoveList(initialplace2 + new Vector2Int(-1, -2));
        RemoveMoveList();

        return movePlaceList;
    }
}
   