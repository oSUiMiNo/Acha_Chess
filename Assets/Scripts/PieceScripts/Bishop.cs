using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece_Common
{
    protected override void Define_PieceType() { type = PieceType.Bishop; }
    public override List<Vector2Int> GetMovePlaceList(Vector2Int initialplace2)
    {
        GetSelectedInfo();　//選んだ場所をgrid(Vector2)に
        movePlaceList = new List<Vector2Int>();

        if (type == PieceType.Bishop)
        {
            Debug.Log("ビショップえらんだ");
                Debug.Log("びしょっぷ");
            foreach (Vector2Int Direction in Direction_Bishop)
            {
                for (int a = 1; a < 7; a++)　　
                {
                    AddMoveList(initialplace2 + (a * Direction));
                    RemoveMoveList();
                    if (existPiece == true) { break; }
                };
                Debug.Log("びしょっぷ4");
            }
        }
        //↓書き直す前
    //  for (int a = 1; a < 7; a++)//左上
    //  {
    //      movePlace_Grid.x = initialplace2.x - a;
    //      movePlace_Grid.y = initialplace2.y + a;
    //
    //      AddMoveList();
    //      RemoveMoveList();
    //      if (existPiece == true) { break; }
    //  }
    //  for (int a = 1; a < 8; a++)//右下
    //  {
    //      movePlace_Grid.x = initialplace2.x + a;
    //      movePlace_Grid.y = initialplace2.y - a;
    //
    //      AddMoveList();
    //      RemoveMoveList();
    //      if (existPiece == true) { break; }
    //  }
    //  for (int a = 1; a < 7; a++)//左下
    //  {
    //      movePlace_Grid.x = initialplace2.x - a;
    //      movePlace_Grid.y = initialplace2.y - a;
    //
    //      AddMoveList();
    //      RemoveMoveList();
    //      if (existPiece == true) { break; }
    //  }
        return movePlaceList;
    }
}
