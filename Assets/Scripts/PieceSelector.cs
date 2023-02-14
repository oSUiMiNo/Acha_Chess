using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Linq;

public class PieceSelector : MonoBehaviourPunCallbacks
{
    public PlayerType playerType;

    public GameObject selectedPiece = null;
    public GameObject[] white = null;
    public GameObject[] black = null;
    public GameObject selectedPiece_Model = null;
    public GameObject selectedPiece_HighLightModel = null;
    public GameObject beforePiece = null;   
    public Vector2Int initialPlace2;
    
    Piece_Common common = null;
    PieceManager pieceManager = null;
    MovePiece movePiece = null;
    ChessSet chessSet = null;
    void GetComponets()
    {
        int A = Piece_Common.a;

        pieceManager = GameObject.Find("ChessSet(Clone)").GetComponent<PieceManager>();
        movePiece = GameObject.Find("ChessSet(Clone)").GetComponent<MovePiece>();
        chessSet = GameObject.Find("ChessSet(Clone)").GetComponent<ChessSet>();

        tile_Original = GameObject.Find("tiles");
    }


    private void Init()
    {
        white = new GameObject[16];
        black = new GameObject[16];
        for (int a = 0; a < 16; a++)
        {
            white[a] = pieceManager.Pieces[0, a];  //コライダー用のオブジェクト
            ResetPieceColor(white[a]);

            black[a] = pieceManager.Pieces[1, a];
            ResetPieceColor(black[a]);
        }
    }

    void Start()
    {
        Debug.Log("はいった");
       
        //オンラインモードかつthisが相手のアバターにくっついているPieceSelectorの場合はエントリーポイントに入らない**********
        if (RoomDoor.ins.IsOnline && !photonView.IsMine) return;
        //***********************************************************************************************
        
        GetComponets();
        Init();
    }

    void Update()  //選んだ場所を黄色にする
    {
        //オンラインモードかつthisが相手のアバターにくっついているPieceSelectorの場合はエントリーポイントに入らない**********
        if (RoomDoor.ins.IsOnline && !photonView.IsMine) return;
        //***********************************************************************************************
        //Debug.Log($"こまえらべるよーーーーー1");
        if ((playerType == PlayerType.WhitePlayer && SceneHandler_Game.Compo.whiteturn) || (playerType == PlayerType.BlackPlayer && !SceneHandler_Game.Compo.whiteturn))
        {
            //Debug.Log($"こまえらべるよーーーーー2  {gameObject} {playerType} {SceneHandler_Game.Compo.whiteturn}");   
            SelectPiece();
        }
    }

    private void SelectPiece()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0.02f, false);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;
        if (!Input.GetMouseButtonDown(0)) return;
        Debug.Log($"Click   {gameObject}");
        if (playerType == PlayerType.WhitePlayer && hit.collider.gameObject.CompareTag("BlackPiece")) return;
        if (playerType == PlayerType.BlackPlayer && hit.collider.gameObject.CompareTag("WhitePiece")) return;

        if (beforePiece != null) ClearPiece(beforePiece);
        //if (!((gameManager.whiteturn && hit.collider.gameObject.CompareTag("WhitePiece")) || (!gameManager.whiteturn && hit.collider.gameObject.CompareTag("BlackPiece")))) return;
        //if (!((GameManager.ins.whiteturn && hit.collider.gameObject.CompareTag("WhitePiece")) || (!GameManager.ins.whiteturn && hit.collider.gameObject.CompareTag("BlackPiece")))) return;
        if (!((SceneHandler_Game.Compo.whiteturn && hit.collider.gameObject.CompareTag("WhitePiece")) || (!SceneHandler_Game.Compo.whiteturn && hit.collider.gameObject.CompareTag("BlackPiece")))) return;
        //Debug.Log("選ぶよ");    
        selectedPiece = hit.collider.gameObject;
        selectedPiece_Model = selectedPiece.transform.GetChild(0).gameObject;
        selectedPiece_HighLightModel = selectedPiece.transform.GetChild(1).gameObject;
        initialPlace2 = Grid.PointToGrid(selectedPiece.transform.position);
        common = selectedPiece.GetComponent<Piece_Common>();

        if(SceneHandler_Game.Compo.whiteturn)
        {
            if (common.color == PieceColor.white)
            {
                if (CanSelect())
                {
                    Hilight();
                    ThisOff();                    
                }
            }
            else Debug.Log("なんでやねん！"); //白の番に白が選ばれなかったら
        }
        else
        {
            if (common.color == PieceColor.black)
            {
                if (CanSelect())
                {
                    Hilight();
                    ThisOff();
                }
            }
            else Debug.Log("なんでやねん！");
        }   
        beforePiece = selectedPiece;
    }


    private void Hilight()
    {
        selectedPiece_Model.GetComponent<Renderer>().enabled = false;
        selectedPiece_HighLightModel.GetComponent<Renderer>().enabled = true;
        GenerateTiles(common.movePlaceList, new Color(0.05f, 1, 0.45f, 1));
        GenerateTiles(common.attackPlaceList, Color.red);
        GenerateTiles(common.castilngPlaceList, Color.blue);
    }

    public GameObject tile_Original;
    public List<GameObject> tileList;
    public void GenerateTiles(List<Vector2Int> placeList, Color tileColor) //選択したpieceが動ける場所(リストに入った場所)全部緑にする
    {
        for (int a = 0; a < placeList.Count; a++)
        {
            Vector3 movePlace_Point = Grid.GridToPoint(placeList[a]); //まだ駒がある場所を取り除いてないリストに入れたmoveplace2(Vector2)をVector3に
            GameObject tile = Instantiate(tile_Original, movePlace_Point, Quaternion.identity);
            Renderer tileRenderer = tile.GetComponent<Renderer>();
            tileRenderer.material.color = tileColor;
            
            if(!SceneHandler_Game.Compo.useGuide) 
                tileRenderer.enabled = false;
            else
                tileRenderer.enabled = true;
            
            tileList.Add(tile);
        }
    }

    public void ClearPiece(GameObject piece)
    {
        foreach (GameObject a in tileList)
        {
            a.SetActive(false);
        }
        tileList.Clear();

        ResetPieceColor(piece);
    }

    public bool CanSelect()　//黄色にする処理
    {
        common.selectedPiece = selectedPiece;
        common.GetMovePlaceList(initialPlace2);
        if (common.movePlaceList.Count == 0 && common.attackPlaceList.Count == 0) return false;
        for (int a = 0; a < 16; a++)
        {
            if (white[a] == selectedPiece || black[a] == selectedPiece) return true;
        }
        return false;
    }
    
    private void ThisOff()
    {
        movePiece.enabled = true;
        movePiece.pieceSelector = this;
        movePiece.selectedPiece = selectedPiece;
    }

  

    public void ResetPieceColor(GameObject piece)
    {
        //Debug.Log(piece);
        piece.GetComponent<Renderer>().enabled = false;
        piece.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        piece.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
    }
}
        
