using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class MovePiece : MonoBehaviourPunCallbacks
{
    private PieceManager pieceManager = null;
    private Piece_Common common = null;

    public PieceSelector pieceSelector = null;
    public GameObject selectedPiece = null;

    [SerializeField]
    private float pieceMoveSpeed = 10;

    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();

    void GetComponets()
    {
        pieceSelector = GameObject.Find("ChessSet(Clone)").GetComponent<PieceSelector>();
        pieceManager = GameObject.Find("ChessSet(Clone)").GetComponent<PieceManager>();

        //whiteKing = GameObject.Find("WhiteKing0").GetComponent<King>();
        //blackKing = GameObject.Find("BlackKing0").GetComponent<King>();
    }

    void Start()
    {
        GetComponets();
        this.enabled = false;
    }

    void Update()
    {
        WhetherMove();
    }

    RaycastHit hit;
    private void WhetherMove()
    {
        if (selectedPiece == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        common = selectedPiece.GetComponent<Piece_Common>();/////？？？動かしちゃったけどだいじ？？？
        if (!Physics.Raycast(ray, out hit)) return;
        if (!Input.GetMouseButtonDown(0)) return;

        Vector2Int selectedGrid = Grid.PointToGrid(hit.point);　　//①コライダーにRayが当たった点がhit.point②その点をx.y座標(平面)上の点に
        Vector3 selectedPoint = Grid.GridToPoint(selectedGrid);  //③その点を(x,0,z)(3次元)の位置に直す(わざわざ①～③やらないと、hit.pointにピース動かすとyが０以外にも行っちゃうから下がったり上がったりしてタイルが見えなくなったりしちゃうから)

        //移動
        if (common.movePlaceList.Contains(selectedGrid))//もしListに選んだ場所が含まれたら
        {
            photonView.RPC(nameof(Move), RpcTarget.AllBuffered, selectedPiece.name, selectedPoint);
        }
        //攻撃
        else 
        if (common.attackPlaceList.Contains(selectedGrid))//Getリストに選んだ場所含まれたら
        {
            GameObject gotPiece = hit.collider.gameObject;  //敵から奪う駒
            photonView.RPC(nameof(Attack), RpcTarget.AllBuffered, selectedPiece.name, selectedPoint, gotPiece.name);
        }
        //キャスリング
        else
        if (common.castilngPlaceList.Contains(selectedGrid))
        {
            Debug.Log($"キャスリング1{selectedPiece}");
            if (selectedPiece.GetComponent<Piece_Common>().type == PieceType.King)
            {
                Debug.Log("キャスリング2");
                //king自体はKingスクリプトで動く処理やってるからRookだけ動かす処理

                //キング移動
                //RPC()の第一引数:実行するメソッド名,第二引数:RPCを実行する対象(AllBfferdは自分、通信相手、途中参加の人全員,
                //第三引数以降;実行するメソッドの引数(ここではたぶんselectedPieceをmovepointに動かす）
                photonView.RPC(nameof(Move), RpcTarget.AllBuffered, selectedPiece.name, selectedPoint);

                //ルーク移動
                GameObject rook = selectedPiece.GetComponent<King>().rook;
                Vector2Int Rook_moveGrid = selectedGrid + new Vector2Int(-1, 0);
                Vector3 Rook_movePoint = Grid.GridToPoint(Rook_moveGrid);
                photonView.RPC(nameof(Move), RpcTarget.AllBuffered, rook.name, Rook_movePoint);
            }  
        }
        else return;

        ThisOff();
    }


    
    [PunRPC]
    private void Move(string selectedPiece_Name, Vector3 movePoint)
    {
        selectedPiece = GameObject.Find(selectedPiece_Name);
        selectedPiece.GetComponent<Piece_Common>().first = false;  //RPCなのでcommonを引数で共有しないといけないのでここで取得しなおした。
        StartCoroutine(Move_Co(movePoint));
    }
    IEnumerator Move_Co(Vector3 movePoint)
    {
        float moveTime = (movePoint - selectedPiece.transform.position).magnitude / pieceMoveSpeed;
        MoveAnim(movePoint, moveTime);
        
        yield return new WaitForSeconds(moveTime - 0.2f);
        AudioManager.Units[AudioName.SE1].PlayOneShot();
    }


    [PunRPC]
    private void Attack(string selectedPiece_Name, Vector3 movePoint, string gotPiece_Name)
    {
        selectedPiece = GameObject.Find(selectedPiece_Name);
        selectedPiece.GetComponent<Piece_Common>().first = false;  //RPCなのでcommonを引数で共有しないといけないのでここで取得しなおした。
        StartCoroutine(Attack_Co(movePoint, gotPiece_Name));
    }
    IEnumerator Attack_Co(Vector3 movePoint, string gotPiece_Name)
    {
        float moveTime = (movePoint - selectedPiece.transform.position).magnitude / pieceMoveSpeed;
        MoveAnim(movePoint, moveTime);
        
        yield return new WaitForSeconds(moveTime - 0.2f);
        AudioManager.Units[AudioName.SE0].PlayOneShot();

        GetPiece(gotPiece_Name);
    }


    void MoveAnim(Vector3 movePoint, float moveTime)
    {
        Debug.Log(selectedPiece + " を " + movePoint + " に動かす");
        if (SceneHandler_Game_OnLine.Compo.useAnimation)
        {
            //selectedPiece.transform.position = movePoint;
            if (selectedPiece.GetComponent<Piece_Common>().type == PieceType.Knight)
            {
                selectedPiece.transform.DOPath(
                    new[]
                    {
                    selectedPiece.transform.position,
                    (movePoint + selectedPiece.transform.position)/2 + new Vector3(0, 3, 0),
                    movePoint
                    },
                    moveTime,
                    PathType.CatmullRom
                    ).SetEase(Ease.InSine);
            }
            else
            {
                selectedPiece.transform.DOMove(movePoint, moveTime).SetEase(Ease.InQuad);
            }
        }
        else
        {
            selectedPiece.transform.position = movePoint;
        }
    }
    
    void GetPiece(string gotPiece_Name)
    {
        GameObject gotPiece = GameObject.Find(gotPiece_Name);
        gotPiece.SetActive(false);
        Debug.Log("ゲット" + gotPiece_Name);
        //if (gotPiece.GetComponent<Piece_Common>().type == PieceType.King) GameManager.ins.CheckMate();
        //if (gotPiece.GetComponent<Piece_Common>().type == PieceType.King) StartCoroutine(GameManager.ins.CheckMate());
        if (gotPiece.GetComponent<Piece_Common>().type == PieceType.King) StartCoroutine(SceneHandler_Game_OnLine.Compo.CheckMate());
    }

    public void ThisOff()
    {
        common.movePlaceList.Clear();
        common.attackPlaceList.Clear();
        common.castilngPlaceList.Clear();
        if (common.first) common.first = false;

        pieceSelector.enabled = true;
        pieceSelector.ClearPiece(selectedPiece);

        selectedPiece = null;
        photonView.RPC(nameof(NextPlayer), RpcTarget.AllBuffered);
        this.enabled = false;
    }

    [PunRPC]
    private void NextPlayer()
    {
        SceneHandler_Game_OnLine.Compo.NextPlayer();
    }
}
      

