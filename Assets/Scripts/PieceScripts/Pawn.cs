using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece_Common
{
    protected override void Define_PieceType() { type = PieceType.Pawn; }

    public List<Vector2Int> PawnNanameList;
    public override List<Vector2Int> GetMovePlaceList(Vector2Int initialplace2) 
    {
        GetSelectedInfo();
        movePlaceList = new List<Vector2Int>();
        if (color == PieceColor.white)
        {
            pawnNaname = true;
            AddMoveList(initialplace2 + new Vector2Int(-1, 1));
            RemoveMoveList();
            pawnNaname = false;

            pawnNaname = true;
            AddMoveList(initialplace2 + new Vector2Int(1, 1));
            RemoveMoveList();
            pawnNaname = false;

            AddMoveList(initialplace2 + new Vector2Int(0, 1));
            RemoveMoveList();
            if (first && !existPiece)
            {
                AddMoveList(initialplace2 + new Vector2Int(0, 2));
                RemoveMoveList();
            }
        }
        else
        {
            pawnNaname = true;
            AddMoveList(initialplace2 + new Vector2Int(-1, -1));
            RemoveMoveList();
            pawnNaname = false;

            pawnNaname = true;
            AddMoveList(initialplace2 + new Vector2Int(1, -1));
            RemoveMoveList();
            pawnNaname = false;

            AddMoveList(initialplace2 + new Vector2Int(0, -1));
            RemoveMoveList();
            if (first && !existPiece)
            {
                AddMoveList(initialplace2 + new Vector2Int(0, -2));
                RemoveMoveList();
            }
        }
        return movePlaceList;
    }
}
 
      


