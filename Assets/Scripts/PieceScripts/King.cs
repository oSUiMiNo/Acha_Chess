using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece_Common
{
    protected override void Define_PieceType() { type = PieceType.King; }
    public PlayerType playerType;
    private Vector2Int kingNext2 = new Vector2Int();
    private Vector2Int kingNext3 = new Vector2Int();
    public GameObject rook;

    protected override void SubStart()
    {
        // Rook rook = new Rook(); これだと新しいインスタンスになっちゃう。
    }

    public override List<Vector2Int> GetMovePlaceList(Vector2Int initialplace2)
    {
        movePlaceList = new List<Vector2Int>();
        GetSelectedInfo();

        //キャスリング
        if (first == true) //&& whiteRook.first == true || blackRook.first == true)　　 
        {
            if (!Grid.GridToPiece(initialplace2 + new Vector2Int(1, 0)))  //Kingの一つ隣に駒がなかったら
            {
                kingNext2 = initialplace2 + new Vector2Int(2, 0);
                if (!Grid.GridToPiece(kingNext2)) //Kingの二つ隣にも駒がなかったら
                {
                    kingNext3 = initialplace2 + new Vector2Int(3, 0);
                    //GameObject piece = Grid.GridToPiece(kingNext3);
                    if (Grid.GridToPiece(kingNext3).GetComponent<Piece_Common>().type == PieceType.Rook) //Kingの三つ隣がRookだったら
                    {
                        rook = Grid.GridToPiece(kingNext3);
                        castilngPlaceList.Add(kingNext2);
                        //AddMoveList(kingNext2);
                        //Castling = true;
                    }
                }
            }
        }

        //通常
        Vector2Int[][] Direction_King = new Vector2Int[][] //Direction_kingは配列の配列
        {
                Direction_Rook,
                Direction_Bishop
        };

        foreach (Vector2Int[] Directions in Direction_King) //Directionは配列
        {
            foreach (Vector2Int Direction in Directions)
            {
                AddMoveList(initialplace2 + Direction);
                RemoveMoveList();
            }
        }

        return movePlaceList;
    }
}

//GridToPiece作る前のGetMoveListの文
//    if (type == PieceType.King) 
//    {
//        if (first == true || rook.first == true)　　  //キャスリング
//        {
//            kingNext1 = initialplace2 + new Vector2Int(1, 0);
//            kingNext1_Point = Grid.GridToPoint(kingNext1); //まだ駒がある場所を取り除いてないリストに入れたmoveplace2(Vector2)をVector3に
//            rayDirection = kingNext1_Point;
//            Ray ray = new Ray(cam.transform.position, rayDirection - cam.transform.position);
//            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue, 1, false);
//            RaycastHit hit;
//            if (!Physics.Raycast(ray, out hit))  //Kingの一つ隣に駒がなかったら
//            {
//                kingNext2 = initialplace2 + new Vector2Int(2, 0);
//                kingNext2_Point = Grid.GridToPoint(kingNext2);
//                rayDirection = kingNext2_Point;
//                Ray ray2 = new Ray(cam.transform.position, rayDirection - cam.transform.position);
//                Debug.DrawRay(ray2.origin, ray2.direction * 100, Color.blue, 1, false);
//                RaycastHit hit2;
//                if (!Physics.Raycast(ray2, out hit2))　//Kingの二つ隣にも駒がなかったら
//                {
//                    kingNext3 = initialplace2 + new Vector2Int(3, 0);
//                    kingNext3_Point = Grid.GridToPoint(kingNext3);
//                    rayDirection = kingNext3_Point;
//                    Ray ray3 = new Ray(cam.transform.position, rayDirection - cam.transform.position);
//                    Debug.DrawRay(rayDirection, ray3.direction * 100, Color.blue, 1, false);
//                    RaycastHit hit3;


//                    AddMoveList(kingNext2);
//                    Castling = true;
//                }
//        