using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class RoomDoor : MonoBehaviourPunCallbacks
{
    public static RoomDoor ins = null; //staticだからインスタンスには存在せず、たった一つ(大元のクラスに)しか存在しないので、インスタンス名．じゃなくてクラス名．で関数を取得できる
    void Singletonize()
    {
        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Awake()
    {
        Singletonize();
    }
    

    public bool tryingConectingToMasterServer = false;
    public bool isConectedToMasterServer = false;
    public bool ready = false;
    public ChessSet chessset = null;
    private Avatar avatar0 = null;
    private Avatar avatar1 = null;
    OnLineSelect onLineSelect = null;
    [SerializeField] public bool IsOnline;

    [SerializeField] private bool player0Ready = false;
    [SerializeField] private bool player1Ready = false;
    public event System.Action OnBothReady;
    [SerializeField] private bool gotMasterColor = false;  //マスターの色が決め終わったかのフラグ
    [SerializeField] private bool masterIsWhite = false;
    //public OnLineSelect onLineSelect = new OnLineSelect();
    /// <summary>
    /// 【まくまくまくまくまく】
    /// MonoBehavior 継承してるからnewすると怒られる。
    /// MonoBehabiorを継承してないとGameObjectにつけられない。Start関数、Update関数などなどユニティー特有の関数が使える。逆に継承してないと使えない。（ただ、StartとUpDateを使わなくても他のクラスのUpDate関数に関数をいれるとか、他のクラスで関数を呼び出すとか代わりのやりかたはある。）
    /// スクリプトはGameObjectのコンポーネントとして付いてないとStartから実行されない。（関数をどこかで呼び出せばその関数は使えるけど。）
    /// つまりコンポーネントとして付けられる前提でつくられたもの。
    /// 本来C♯はStart関数ではなくてコンストラクタが初期化の役割をしてる。コンストラクタとはクラスと同じ名前の関数。newでインスタンスを作ったときに必ず呼ばれる関数。
    /// でもunityのMonoBehaviorを継承してる場合、コンポーネントになってる（インスタンスができてる）からnewでまたインスタンス作れるけど良くないというか意味ない。
    /// newで作ったインスタンスでは、コンストラクタは実行されるけど、Start関数とかは実行されない。つまり、わざわざMonoBehaviorを継承してStart関数つくっても意味ない。
    /// 
    /// 【問】
    /// OnLineSelectは、MonoBehavior を外して new か、MonoBehavior つけたまま GetComponent だけど、今回ははどっちが良いでしょう→MonoBehavior
    /// </summary>



    #region 【カスタムプロパティ系】 ===================================================================

    string key1 = "gotMasterColor";
    string key2 = "masterIsWhite";
    string key3 = "MasterPlayerReady";
    string key4 = "NormalPlayerReady";
    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
    //一通りの更新が終わったら周知しときたい変数をまとめて更新する
    public void SetRoomProps()
    {
        Debug.Log("るーむぷろぱてぃーセット");
        hashtable[key1] = gotMasterColor;
        hashtable[key2] = masterIsWhite;
        hashtable[key3] = player0Ready;
        hashtable[key4] = player1Ready;

        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        hashtable.Clear();
    }
    //周知の変数を使う際は念のためこれを呼んでから
    public void GetRoomProps()
    {
        gotMasterColor = (PhotonNetwork.CurrentRoom.CustomProperties[key1] is bool a) ? a : false;
        masterIsWhite = (PhotonNetwork.CurrentRoom.CustomProperties[key2] is bool b) ? b : false;
        player0Ready = (PhotonNetwork.CurrentRoom.CustomProperties[key3] is bool c) ? c : false;
        player1Ready = (PhotonNetwork.CurrentRoom.CustomProperties[key4] is bool d) ? d : false;
    }
    //更新されたら一応自動で取得しとく
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        GetRoomProps();
        Debug.Log($"るーむぷろぱてぃーやった  {player0Ready} {player1Ready}");
        if(player0Ready && player1Ready) OnBothReady?.Invoke();
    }

    #endregion 【カスタムプロパティ系】 ================================================================


    private void Start()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        onLineSelect = GameObject.Find("OnLineButton").GetComponent<OnLineSelect>();
    }
    //オフラインのときにチェスセットを生成
    public void ConnectToMasterServer()
    {
        /// <summary>
        /// 【まく】
        ///この onLineSelect、newでインスタンス化されたやつだからこいつの変数参照しても意味ないよ。どうして意味が無いんでしょう！
        ///オンラインボタンを押すとif文に入ってるOnlinedがtrueになるけど、ここではnewした新しいインスタンスを使ってるから、Onlinedはfalseのままだから。 
        /// </summary>
        if (IsOnline)
        {
            tryingConectingToMasterServer = true;
            isConectedToMasterServer = false;
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("マスターサーバーへの接続を試みます");
        }
    }

    public void Join()
    {
        ready = false;

        PhotonNetwork.NickName = UserData.Ins.playerName;

        Debug.Log("ランダムなルームへの参加を試みます");
        PhotonNetwork.JoinRandomRoom();  // ランダムなルームに参加する
    }

    public void Leave()
    {
        Debug.Log("ルームからの退出を試みます");
        PhotonNetwork.LeaveRoom();
    }

    
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーに接続しました");
        tryingConectingToMasterServer = false;
        isConectedToMasterServer = true;
        //GameManager.ins.Smokes.SetActive(false);  ここにあるとオンラインの状態でオンラインボタン押したときにこの関数呼ばれないからスモーク消えない。（オンラインでゲーム入ってメニューにもどったときとか）
    }                                                                                                                                                                                                                           

    public void DisconnectfromMasterServer()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("マスターサーバーを切断しました");
        isConectedToMasterServer = false;
    }
    public override void OnDisconnected(DisconnectCause cause)　//OnDisconnect:ConnectUsingSettingsが失敗したとき
    {
        Debug.Log("Photonサーバーから切断しました");
        Debug.Log(cause);
        isConectedToMasterServer = false;
        if (IsOnline)
        {
            tryingConectingToMasterServer = true;
            Debug.Log("再度マスターサーバーへの接続を試みます");
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("ルームが無いので作成します");
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;  // ルームの参加人数を2人に設定する
        PhotonNetwork.CreateRoom(null, roomOptions);  // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    }



    public override void OnJoinedRoom()
    {
        Debug.Log("ルームに参加しました");
        GivePlayers();
        StartCoroutine(InitNetworkObject());
    }
    IEnumerator InitNetworkObject()
    {
        int randomNumber = UnityEngine.Random.Range(0, 2);

        GetRoomProps();
        Debug.Log("ネットワークオブジェクトやる0");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("ネットワークオブジェクトやる1");
            //chessset = PhotonNetwork.InstantiateRoomObject("ChessSet", Vector3.zero, Quaternion.identity).GetComponent<ChessSet>();
            chessset = LoadRoomObject("ChessSet", Vector3.zero, Quaternion.identity).GetComponent<ChessSet>();
            
            yield return new WaitUntil(() => chessset != null);

            if (randomNumber == 0) SetMasterColor(PlayerType.WhitePlayer); //randomNumberが0だったらマスターの色が白になる
            else SetMasterColor(PlayerType.BlackPlayer);
            
            if (!gotMasterColor) yield break;
            if (masterIsWhite)
            {
                Debug.Log("マスターの色は白 OnJoinedRoom");
                avatar0 = LoadNetWorkObject("Avatar0", new Vector3(0f, 2f, -15f), Quaternion.identity).GetComponent<Avatar>();
                Camera.main.transform.position = new Vector3(-0.4f, 28.86f, -19.95f);
                Camera.main.transform.rotation = Quaternion.Euler(55, 0, 0);
            
                player0Ready = true;
                avatar0.LockAction();
            }
            else
            {
                Debug.Log("マスターの色は黒 OnJoinedRoom");
                avatar1 = LoadNetWorkObject("Avatar1", new Vector3(0f, 2f, 15f), Quaternion.identity).GetComponent<Avatar>();
                Camera.main.transform.position = new Vector3(0.4f, 28.86f, 19.95f);
                Camera.main.transform.rotation = Quaternion.Euler(55, 180, 0);
                
                player1Ready = true;
                avatar1.LockAction();
            }

            yield return new WaitUntil(() => SceneHandler_Game.Compo.IsInitialized);
            Debug.Log("OnInitialized");
            SetRoomProps();
        }
        else
        {
            yield return new WaitUntil(() => GameObject.Find("ChessSet(Clone)"));
            chessset = GameObject.Find("ChessSet(Clone)").GetComponent<ChessSet>();
            if (!gotMasterColor) yield break;

            if (masterIsWhite)
            {
                Debug.Log("自分の色は黒 OnJoinedRoom");
                avatar1 = LoadNetWorkObject("Avatar1", new Vector3(0f, 2f, 15f), Quaternion.identity).GetComponent<Avatar>();
                Camera.main.transform.position = new Vector3(0.4f, 28.86f, 19.95f);
                Camera.main.transform.rotation = Quaternion.Euler(55, 180, 0);

                player1Ready = true;
            }   
            else
            {
                Debug.Log("自分の色は白 OnJoinedRoom");
                avatar0 = LoadNetWorkObject("Avatar0", new Vector3(0f, 2f, -15f), Quaternion.identity).GetComponent<Avatar>();
                Camera.main.transform.position = new Vector3(-0.4f, 28.86f, -19.95f);
                Camera.main.transform.rotation = Quaternion.Euler(55, 0, 0);

                player0Ready = true;
            }

            yield return new WaitUntil( () => SceneHandler_Game.Compo.IsInitialized);
            Debug.Log("OnInitialized");
            SetRoomProps();
        }
    }


    public override void OnLeftRoom()
    {
        Debug.Log("ルームから退出しました");
        if (SceneManager.GetActiveScene().name == SceneName.Game_OnLine) SceneHandler_Menu.Compo.LoadScene();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer + " が参加しました");
        GivePlayers();

        OnBothReady += () =>
        {
            Debug.Log("二人ともじゅんびおっけー");
            if (masterIsWhite) avatar0.AllowAction();
            else avatar1.AllowAction();
            //avatar0.SetAvaterNameDisplay();
            //avatar1.SetAvaterNameDisplay();
        };
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer + " が退出しました");
        GivePlayers();
        Debug.Log("退出します");
        Leave();
        GameManager.Compo.LoadScene(SceneName.Menu);
    }
    
    
    void SetMasterColor(PlayerType color)
    {
        gotMasterColor = true;
        if (color == PlayerType.WhitePlayer) masterIsWhite = true;
        if (color == PlayerType.BlackPlayer) masterIsWhite = false;
    }


    GameObject LoadNetWorkObject(string name, Vector3 position, Quaternion rotation)
    {
        GameObject obj = PhotonNetwork.Instantiate(name, position, rotation);
        //obj.name = obj.name.Replace("(Clone)", "");
        return obj;
    }

    GameObject LoadRoomObject(string name, Vector3 position, Quaternion rotation)
    {
        GameObject obj = PhotonNetwork.InstantiateRoomObject(name, position, rotation);
        //obj.name = obj.name.Replace("(Clone)", "");
        return obj;
    }



    //多分使ってない****************************
    public Player[] GetPlayers()
    {
        if (IsOnline)
        {
            return PhotonNetwork.PlayerList;
        }
        else return null;
    }
    public void GivePlayers()
    {
        GameManager.Compo.players.Clear();
        foreach(var a in PhotonNetwork.PlayerList)
        {
            GameManager.Compo.players.Add(a);
        }
    }
    //多分使ってない****************************



    PhotonStates state = PhotonStates.Default;
}

public enum PhotonStates
{
    Default,
    TryingConectingToMasterServer,
    ConectedToMasterServer,
    Ready
}






    //////////////カスタムプロパティ―(RPCに置きかえ済）//////////////////////

    //public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)  //
    //{
    //    //Debug.Log("マスターが白かどうか " + GetMasterColor());
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        if (GetMasterColor())
    //        {
    //            Debug.Log("マスターの色は白 OnRoomPropertiesUpdate");
    //            avatar0 = PhotonNetwork.Instantiate("Avatar0", new Vector3(0f, 2f, -15f), Quaternion.identity);
    //            Camera.main.transform.position = new Vector3(-0.4f, 28.86f, -19.95f);
    //            Camera.main.transform.rotation = Quaternion.Euler(55, 0, 0);
    //            //photonView.RPC(nameof(MasterIsWhite), RpcTarget.AllBuffered);

    //            Debug.Log(avatar0);
    //            avatar0.GetComponent<Avatar>().photonView.RPC(nameof(LockAction), RpcTarget.All);
    //        }
    //        else
    //        {
    //            Debug.Log("マスターの色は黒 OnRoomPropertiesUpdate");
    //            avatar1 = PhotonNetwork.Instantiate("Avatar1", new Vector3(0f, 2f, 15f), Quaternion.identity);
    //            Camera.main.transform.position = new Vector3(0.4f, 28.86f, 19.95f);
    //            Camera.main.transform.rotation = Quaternion.Euler(55, 180, 0);
    //            //photonView.RPC(nameof(MasterIsWhite), RpcTarget.AllBuffered);

    //            Debug.Log(avatar1);
    //            avatar1.GetComponent<Avatar>().photonView.RPC(nameof(LockAction), RpcTarget.All);
    //        }
    //    }
    //    else
    //    {
    //        if (GetMasterColor())
    //        {
    //            Debug.Log("自分の色は黒 OnRoomPropertiesUpdate");
    //            avatar1 = PhotonNetwork.Instantiate("Avatar1", new Vector3(0f, 2f, 15f), Quaternion.identity);
    //            Camera.main.transform.position = new Vector3(0.4f, 28.86f, 19.95f);
    //            Camera.main.transform.rotation = Quaternion.Euler(55, 180, 0);
    //            //photonView.RPC(nameof(MasterIsWhite), RpcTarget.AllBuffered);
    //        }
    //        else
    //        {
    //            Debug.Log("自分の色は白 OnRoomPropertiesUpdate");
    //            avatar0 = PhotonNetwork.Instantiate("Avatar0", new Vector3(0f, 2f, -15f), Quaternion.identity);
    //            Camera.main.transform.position = new Vector3(-0.4f, 28.86f, -19.95f);
    //            Camera.main.transform.rotation = Quaternion.Euler(55, 0, 0);
    //            //photonView.RPC(nameof(MasterIsWhite), RpcTarget.AllBuffered);
    //        }
    //    }

    //    ready = true;
    //    Debug.Log("Ready = true");
    //}
    //private static Hashtable prop = new Hashtable();  //以下カスタムプロパティ―参照　Hashtable;キーと値のペアを保持しているコレクション。
    //private static bool gotMasterColor = false;
    //public static void SetMasterColor(bool isMaster)   //：設定する
    //{
    //    Debug.Log("SetMasterColor");
    //    prop["masterIsWhite"] = isMaster;　//ハッシュテーブルへのペアの追加（masterIsWhiteとisMasterのペア、GotMasterColorとtrueのペア）
    //    prop["GotMasterColor"] = true;
    //    PhotonNetwork.CurrentRoom.SetCustomProperties(prop);  //この文でカスタムプロパティ―に追加される
    //    prop.Clear();
    //}
    //public static bool GetMasterColor() 　　　　　　　//：取得する
    //{
    //    Debug.Log("GetMasterColor"); //isは型変換できるかどうか。Photon…[GotMasuterColor"]をboolにできるか。できるならgotに代入、できなければfalse.
    //    Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["masterIsWhite"]);
    //    gotMasterColor = (PhotonNetwork.CurrentRoom.CustomProperties["GotMasterColor"] is bool got) ? got : false;
    //    return (PhotonNetwork.CurrentRoom.CustomProperties["masterIsWhite"] is bool isMaster) ? isMaster : false;
    //}
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


