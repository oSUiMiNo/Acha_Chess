using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece_Common
{
    protected override void Define_PieceType() { type = PieceType.Rook; }
    public override List<Vector2Int> GetMovePlaceList(Vector2Int initialplace2)
    {
        GetSelectedInfo();
        movePlaceList = new List<Vector2Int>();

        for (int a = 1; a < 8; a++)//右
        {
            AddMoveList(new Vector2Int(initialplace2.x + a, initialplace2.y));
            RemoveMoveList();
            if (existPiece == true) { break; }
        }
        for (int a = 1; a < 8; a++)//左
        { 
            AddMoveList(new Vector2Int(initialplace2.x - a, initialplace2.y));
            RemoveMoveList();
            if (existPiece == true) { break; }
        }
        for (int a = 1; a < 8; a++)//手前
        {
            AddMoveList(new Vector2Int(initialplace2.x, initialplace2.y - a));
            RemoveMoveList();
            if (existPiece == true) { break; }
        }
        for (int a = 1; a < 7; a++)//奥
        {
            AddMoveList(new Vector2Int(initialplace2.x, initialplace2.y + a));
            RemoveMoveList();
            if (existPiece == true) { break; }
        }
        return movePlaceList;
    }
}
