using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType   //enumついてるPieceType、PieceColorはクラスみたいなもの。PieceType、PieceColorはクラス名みたいなもの。列挙できる特殊なクラス。
{//使うときはGameObject名.piece_Common.typeとする。なぜかはUnityの駒オブジェクトみるとわかりやすい。
 //ちなみにそれぞれの駒のスクリプトはPiece_Commonの子クラス(継承してる)だから、それぞれのスクリプトはPiece_commonでもあるといえる。
    Pawn,
    King,
    Queen,
    Bishop,
    Knight,
    Rook
}
public enum PieceColor 
{
    white,
    black 
}


public abstract class Piece_Common : MonoBehaviour
{
    public PieceType type = PieceType.Pawn;
    public PieceColor color;

    public static int a = 1;

    public GameObject marker;

    //protected PieceSelector pieceSelector = null;
    public GameObject cam;
    
    public GameObject selectedPiece;
    public GameObject gettablePiece;
    public GameObject tile;

    public Vector2Int initialPlace_Grid = new Vector2Int();
    public Vector3 initialPlace_Point = new Vector3();
    public Vector3 movePlace_Point = new Vector3();
    protected Vector2Int atackPlace_Point = new Vector2Int();

    public List<Vector2Int> movePlaceList;
    public List<Vector2Int> attackPlaceList;
    public List<Vector2Int> castilngPlaceList;

    public Vector3 rayDirection = new Vector3();

    public bool first = true;
    public bool existPiece = false;
    public bool pawnNaname = false;
    
    private void GetComponents()
    {
        tile = GameObject.Find("tiles");
        cam = Camera.main.gameObject;
        marker = GameObject.Find("Marker");
    }
    
    protected abstract void Define_PieceType();

    private void Init()
    {
        first = true;
    }

    private void Start()
    {
        Define_PieceType();
        GetComponents();
        Init();
        SubStart();
    }
    protected virtual void SubStart() { }
       
    public abstract List<Vector2Int> GetMovePlaceList(Vector2Int initialplace2);//←継承元。継承先は参照に。もとの場所入れると動ける場所をMovePlaceListに追加する関数。これは継承元で宣言だけしておいて、７個の参照に継承してるクラスあり、そこにこの関数の中身が書いてある。
    public void GetSelectedInfo()
    { 
        //pieceSelector = GameObject.Find("Avatar_maku").GetComponent<PieceSelector>();
        //selectedPiece = pieceSelector.selectedPiece;
        initialPlace_Point = selectedPiece.transform.position;
        initialPlace_Grid = Grid.PointToGrid(initialPlace_Point);
    }

    
    public void AddMoveList(Vector2Int moveGrid)
    {
        existPiece = false;
        if (MovablePlace(moveGrid))
        {
            movePlaceList.Add(moveGrid);
        }
    }


    public void RemoveMoveList()　//MoveListから削除。GetListに追加する処理もここで！
    {
        for (int a = 0; a < movePlaceList.Count; a++)
        {
            Vector3 movePlace_Point = Grid.GridToPoint(movePlaceList[a]); //まだ駒がある場所を取り除いてないリストに入れたmoveplace2(Vector2)をVector3に
            rayDirection = movePlace_Point;
            Ray ray = new Ray(cam.transform.position, rayDirection - cam.transform.position);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue, 1, false);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return;　　//何も当たらなかったら抜ける(Removeされない）
            
            //GameObject mar = Instantiate(marker, hit.point, Quaternion.identity);
            //StartCoroutine(DestroyMarker(mar, 1f)); 

            if (hit.collider.gameObject.CompareTag("WhitePiece") || hit.collider.gameObject.CompareTag("BlackPiece"))
              existPiece = true;

            if (existPiece)
            {
                gettablePiece = hit.collider.gameObject;
                atackPlace_Point = Grid.PointToGrid(gettablePiece.transform.position);
                movePlaceList.RemoveAt(a); //MovePlaceListからは削除する
                
                if ((selectedPiece.CompareTag("WhitePiece") && gettablePiece.CompareTag("BlackPiece")) ||
                    (selectedPiece.CompareTag("BlackPiece") && gettablePiece.CompareTag("WhitePiece")))
                {
                    if (type == PieceType.Pawn)
                    {
                        if (pawnNaname)
                        {
                            attackPlaceList.Add(atackPlace_Point); //GetListに駒がある場所を追加する(GetListではただ動くだけじゃなくコマをゲットする
                            Debug.Log("ポーン斜め追加");
                        }
                    }
                    else
                    {
                        attackPlaceList.Add(atackPlace_Point); //GetListに駒がある場所を追加する(GetListではただ動くだけじゃなくコマをゲットする
                    }
                }
            }
            else
            {
                if (type == PieceType.Pawn)
                {
                    if (pawnNaname) movePlaceList.RemoveAt(a);
                }
            }
        }
    }



    public bool MovablePlace(Vector2Int moveplace2) //選んだ場所を動ける場所ならTrue返す
    {
        if ( moveplace2.x < 0 || moveplace2.x > 7 || moveplace2.y < 0 || moveplace2.y > 7)
        { return false; }
        else { return true; }

        if (initialPlace_Grid == moveplace2)
        { return false; }
        else { return true; }
    }

    protected Vector2Int[] Direction_Rook = new Vector2Int[4]
    {
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0)
    };
    protected Vector2Int[] Direction_Bishop = new Vector2Int[4]
    {
        new Vector2Int(1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(1, -1)
    };


    private IEnumerator DestroyMarker(GameObject target, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(target);
    }
}
