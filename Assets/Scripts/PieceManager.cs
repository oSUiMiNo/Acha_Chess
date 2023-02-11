using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public GameObject Board;

    public GameObject whiteKing0;
    public GameObject whiteQueen1;
    public GameObject whiteBishop0;
    public GameObject whiteBishop1;
    public GameObject whiteKnight0;
    public GameObject whiteKnight1;
    public GameObject whiteRook0;
    public GameObject whiteRook1;
    public GameObject whitePawn0;
    public GameObject whitePawn1;
    public GameObject whitePawn2;
    public GameObject whitePawn3;
    public GameObject whitePawn4;
    public GameObject whitePawn5;
    public GameObject whitePawn6;
    public GameObject whitePawn7;


    public GameObject blackKing0;
    public GameObject blackQueen1;
    public GameObject blackBishop0;
    public GameObject blackBishop1;
    public GameObject blackKnight0;
    public GameObject blackKnight1;
    public GameObject blackRook0;
    public GameObject blackRook1;
    public GameObject blackPawn0;
    public GameObject blackPawn1;
    public GameObject blackPawn2;
    public GameObject blackPawn3;
    public GameObject blackPawn4;
    public GameObject blackPawn5;
    public GameObject blackPawn6;
    public GameObject blackPawn7;

    public GameObject[,] Pieces = null;

    PieceSelector pieceSelector = null;


    private void Awake()
    {
        Init();
        StorePieces();
    }

    private void Start()
    {
        Pieceinitialplace();
        pieceSelector = GetComponent<PieceSelector>();
        //Init();
    }

    private void Addpiece(GameObject piece, int col, int low)
    {
        piece.transform.position = Grid.GridToPoint(new Vector2Int(col, low));
    }

    private void Pieceinitialplace()
    {
        for (int a = 0; a < 8; a++)
            Addpiece(Pieces[0, a], a, 0);

        for (int a = 8; a < 16; a++)
            Addpiece(Pieces[0, a], a - 8, 1);

        for (int a = 0; a < 8; a++)
            Addpiece(Pieces[1, a], a, 7);

        for (int a = 8; a < 16; a++)
            Addpiece(Pieces[1, a], a - 8, 6);
    }

    public void Init()
    {
        whiteKing0 = GameObject.Find("WhiteKing0");
        whiteQueen1 = GameObject.Find("WhiteQueen0");
        whiteBishop0 = GameObject.Find("WhiteBishop0");
        whiteBishop1 = GameObject.Find("WhiteBishop1");
        whiteRook0 = GameObject.Find("WhiteRook0");
        whiteRook1 = GameObject.Find("WhiteRook1");
        whiteKnight0 = GameObject.Find("WhiteKnight0");
        whiteKnight1 = GameObject.Find("WhiteKnight1");
        whitePawn0 = GameObject.Find("WhitePawn0");
        whitePawn1 = GameObject.Find("WhitePawn1");
        whitePawn2 = GameObject.Find("WhitePawn2");
        whitePawn3 = GameObject.Find("WhitePawn3");
        whitePawn4 = GameObject.Find("WhitePawn4");
        whitePawn5 = GameObject.Find("WhitePawn5");
        whitePawn6 = GameObject.Find("WhitePawn6");
        whitePawn7 = GameObject.Find("WhitePawn7");

        blackKing0 = GameObject.Find("BlackKing0");
        blackQueen1 = GameObject.Find("BlackQueen0");
        blackBishop0 = GameObject.Find("BlackBishop0");
        blackBishop1 = GameObject.Find("BlackBishop1");
        blackRook0 = GameObject.Find("BlackRook0");
        blackRook1 = GameObject.Find("BlackRook1");
        blackKnight0 = GameObject.Find("BlackKnight0");
        blackKnight1 = GameObject.Find("BlackKnight1");
        blackPawn0 = GameObject.Find("BlackPawn0");
        blackPawn1 = GameObject.Find("BlackPawn1");
        blackPawn2 = GameObject.Find("BlackPawn2");
        blackPawn3 = GameObject.Find("BlackPawn3");
        blackPawn4 = GameObject.Find("BlackPawn4");
        blackPawn5 = GameObject.Find("BlackPawn5");
        blackPawn6 = GameObject.Find("BlackPawn6");
        blackPawn7 = GameObject.Find("BlackPawn7");
    }

    public void StorePieces()
    {
        Pieces = new GameObject[2, 16]
        {
            {
               whiteRook0,
               whiteKnight0,
               whiteBishop0,
               whiteQueen1,
               whiteKing0,
               whiteBishop1,
               whiteKnight1,
               whiteRook1,
               whitePawn0,
               whitePawn1,
               whitePawn2,
               whitePawn3,
               whitePawn4,
               whitePawn5,
               whitePawn6,
               whitePawn7
            },
            {
               blackRook0,
               blackKnight0,
               blackBishop0,
               blackQueen1,
               blackKing0,
               blackBishop1,
               blackKnight1,
               blackRook1,
               blackPawn0,
               blackPawn1,
               blackPawn2,
               blackPawn3,
               blackPawn4,
               blackPawn5,
               blackPawn6,
               blackPawn7
            }
        };

    }


    //private GameObject[,] chessState;  //2次元のインデックスを各タイルととらえて、盤面全体の状況を保存する
    //private Dictionary<GameObject, Vector2> piecePositions; //各駒の今の位置を検索できる
    //private 
    //private void StoreChessState()
    //{
    //    foreach (GameObject piece in Pieces)
    //    {
    //        piecePositions[piece] = grid.PointToGrid(piece.transform.position);
    //    }

    //    chessState = new GameObject[8, 8];
    //    for (int a = 0; a < 8; a ++)
    //    {
    //        for(int b = 0; b < 8; b++)
    //        {
    //            foreach(KeyValuePair<GameObject, Vector2> piecePositionPair in piecePositions)
    //            {
    //                if(piecePositionPair.Value.x == a && piecePositionPair.Value.y == b)
    //                {
    //                    chessState[a, b] = piecePositionPair.Key;
    //                }
    //            }
    //        }
    //    }
    //}
}

