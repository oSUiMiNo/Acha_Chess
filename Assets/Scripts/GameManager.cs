using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


//[System.Serializable]
public class GameManager : MonoBehaviour
{
    public Dictionary<string, ISceneHandler> sceneHandlers = new Dictionary<string, ISceneHandler>();

    [SerializeField] public List<Player> players = new List<Player>();
    
    public TimesCounter roopGameCounter = new TimesCounter(1);


    #region 【個別シングルトン化】 ===================================================================

    public static GameManager Compo = null;//staticをつけた変数は１つしか存在しなくなる。だから呼び出すときにクラスをよびだしてそのクラスの関数名.変数名としなくて変数名だけで
    void Singletonize()  //シングルトン。１つしかインスタンス作らない。まだインスタンスがなかったらインスタンス(このクラス）作る。
    {
        if (Compo == null)
        {
            Compo = this;
            DontDestroyOnLoad(gameObject);  //gameObjectはthis.gameobjectの略。つまりこのGameManagerクラス。
        }
        else   //もしインスタンスすでにあったら１つしかだめだからこわす。
        {
            Destroy(gameObject);
        }
    }

    #endregion 【個別シングルトン化】 ================================================================

    public void Awake()
    {
        DebugView.Log("げーーーーーーーむまねーーじゃーーーーーー");
        Singletonize();
        UserData.Ins.Load();
    }

    public void Start()
    {
        RegisterScenes();

        DebugView.Log($"プレイヤー名 :   {UserData.Ins.playerName}");
        RoomDoor.ins.GetPlayers();      //オフラインの時の処理はどうなる？
    }


    void RegisterScenes()
    {
        sceneHandlers.Add(SceneName.Menu, SceneHandler_Menu.Compo);
        sceneHandlers.Add(SceneName.Game, SceneHandler_Game.Compo);
        sceneHandlers.Add(SceneName.Setting, SceneHandler_Setting.Compo);
        SceneManager.sceneLoaded += OnSceneLoaded; //sceneloadedはシーンがロードされると呼ばれる
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        InSceneOnAwake(SceneManager.GetActiveScene());
    }
    
    public void LoadScene(string sceneName)
    {
        sceneHandlers[sceneName].LoadScene();
    }

    void InSceneOnAwake(Scene scene)
    {
        DebugView.Log($"--------{scene.name}シーン  から起動 --------");
        StartCoroutine(sceneHandlers[scene.name].InitScene());
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DebugView.Log($"--------{scene.name}シーン  をロード --------");
        StartCoroutine(sceneHandlers[scene.name].InitScene());
    }

    void OnSceneUnloaded(Scene scene)
    {
        DebugView.Log($"--------{scene.name}シーン  をアンロード --------");
        StartCoroutine(sceneHandlers[scene.name].ExitScene());
    }
}
