using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece_Common
{
    protected override void Define_PieceType() { type = PieceType.Queen; }
    public override List<Vector2Int> GetMovePlaceList(Vector2Int initialplace2)
    {
        GetSelectedInfo();
        movePlaceList = new List<Vector2Int>();
       
        Vector2Int[][] Direction_Queen = new Vector2Int[][]
        {
            Direction_Rook,
            Direction_Bishop
        };

        foreach (Vector2Int[] Directions in Direction_Queen)
        {
            foreach (Vector2Int Direction in Directions)
            {
                for (int a = 1; a < 8; a++)
                {
                    AddMoveList(initialplace2 + a * Direction);
                    RemoveMoveList();
                    if (existPiece) { break; }
                }
            }
        }
        return movePlaceList;
    }
}
