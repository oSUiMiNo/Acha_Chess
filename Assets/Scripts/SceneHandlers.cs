using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class SceneName
{
    public static string Game = "Create_tyesu";
    public static string Menu = "Menu";
    public static string Setting = "SettingScene";
}

public interface ISceneHandler 
{
    void LoadScene();
    IEnumerator InitScene();
    IEnumerator ExitScene();

    event System.Action OnInitScene;
    event System.Action OnExitScene;
}


#region 1�y���j���[�V�[���̃n���h���[�z ========================================================================

public class SceneHandler_Menu : SingletonCompo<SceneHandler_Menu>, ISceneHandler
{
    public GameObject playerNameField;
    public GameObject warningMessage;
    public GameObject offlineYet;
    public GameObject startButton;
    public GameObject smokes;
    public GameObject onlineButton;
    public GameObject offlineButton;
    public bool menuLoaded = false;

    public BannerDefault bannerDefault;
    public InterstitialOnRoad interstitialOnRoad;

    public event System.Action OnInitScene;
    public event System.Action OnExitScene;

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName.Menu);
    }

    public IEnumerator InitScene()
    {
        DebugView.Log("--------------------�X�^�[�g��ʏ���������00--------------------");
        yield return new WaitForSeconds(0.1f);
        //TimeHandler.CreateClock("clock111", 1, true);
        //TimeHandler.CreateClock("clock222", 2, true);
        OnInitScene?.Invoke();

        GameObject.Find("PlayerNameField").GetComponent<InputField>().onSubmit.AddListener(Define_PlayerName);
        GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(OnClickStratButton);
        GameObject.Find("OnLineButton").GetComponent<Button>().onClick.AddListener(OnClickOnLineButton);
        GameObject.Find("OffLineButton").GetComponent<Button>().onClick.AddListener(OnClickOffLineButton);

        playerNameField = GameObject.Find("PlayerNameField");
        //playerNameField.GetComponent<InputField>().text = playerName;
        playerNameField.GetComponent<InputField>().text = UserData.Ins.playerName;
        playerNameField.SetActive(false);

        startButton = GameObject.Find("StartButton");
        startButton.SetActive(false);

        warningMessage = GameObject.Find("WarningMessage");
        warningMessage.SetActive(false);

        offlineYet = GameObject.Find("OfflineYet");
        offlineYet.SetActive(false);

        smokes = GameObject.Find("Smokes");
        smokes.SetActive(false);

        onlineButton = GameObject.Find("OnLineButton");
        onlineButton.SetActive(true);

        offlineButton = GameObject.Find("OnLineButton");
        offlineButton.SetActive(true);

        menuLoaded = true;
        
        DebugView.Log("�L��0");
        if (AdManager.IsCreated)
        {
            DebugView.Log("�L��1");

            AdManager.LoadAd(AdType.BannerDefault);
            DebugView.Log("�L��2-1");
            AdManager.ShowAd(AdType.BannerDefault);
            DebugView.Log("�L��2-2");

            if (GameManager.Compo.roopGameCounter.HitExeptFirst())
            {
                DebugView.Log("�L��3");

                if (Roulet.BoolRoulet(1))
                {
                    DebugView.Log("�L��4");

                    AdManager.LoadAd(AdType.InterstitialOnRoad);
                    AdManager.ShowAd(AdType.InterstitialOnRoad);
                    DebugView.Log("----------�C���^�[�X�e�B�V�����L���\��----------");
                }
            }
        }
        DebugView.Log("--------------------�X�^�[�g��ʏ���������--------------------");
    }


    public IEnumerator ExitScene() 
    {
        yield return null;
        OnExitScene?.Invoke();
    }


    public void Define_PlayerName(string value)
    {
        UserData.Ins.playerName = playerNameField.GetComponent<InputField>().text;
        UserData.Ins.Save();
        #region �X�N���v�^�u���I�u�W�F�N�g�ł�낤�Ƃ������
        //gameData.playerName = GameObject.Find("InputField").GetComponent<InputField>().text;

        //EditorUtility.SetDirty(gameData);  //�_�[�e�B�Ƃ��ă}�[�N����(�ύX�������������L�^����)
        //Debug.Log("�ۑ����ׂ��ύX���L�����܂���");

        //AssetDatabase.SaveAssets();
        //Debug.Log("�L�������ύX��ۑ����܂���");
        #endregion
    }


    public void OnClickStratButton()
    {
        string nameText = playerNameField.GetComponent<InputField>().text;
        if (RoomDoor.ins.IsOnline == true &&
            nameText != "" &&
            RoomDoor.ins.isConectedToMasterServer)
        {
            GameManager.Compo.LoadScene(SceneName.Game);
        }
        else
        if (RoomDoor.ins.IsOnline == true &&
            nameText == "")
        {
            warningMessage.SetActive(true);
        }
        else
        if (RoomDoor.ins.IsOnline == false)
        {
            GameManager.Compo.LoadScene(SceneName.Game);
        }
    }


    public void OnClickOnLineButton()
    {
        /// <Summary>
        ///�y�܂��z
        /// �ڑ��������� PhotonNetwork.IsConnectedAndReady �� false �ɂȂ��Ă��܂��A���̃R���[�`�������f���Ă��܂��B
        /// �̂ŁA���łɐڑ�����Ă���ꍇ�͂ނ�݂� RoomDoorWay.ins.ConnectToMasterServer() �Ă΂Ȃ��ق����悳���B
        /// ���Ȃ݂ɁA
        /// �l�[���T�[�o�[(photon�T�[�o�[)�ɂȂ�������Ԃ��� PhotonNetwork.IsConnected �� true
        /// �}�X�^�[�T�[�o�[�ɂȂ�������Ԃ��� PhotonNetwork.IsConnectedAndReady �� true
        /// </Summary>
        StartCoroutine(SwitchToOnline());
        StartCoroutine(ReadyToPlay_Online());
    }
    private IEnumerator SwitchToOnline()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            RoomDoor.ins.IsOnline = true;
            RoomDoor.ins.ConnectToMasterServer();
        }
        yield return new WaitForSeconds(0.1f);
        //GameManager.ins.onlineButton.GetComponent<Image>().color = Color.red;
        //GameManager.ins.offlineButton.GetComponent<Image>().color = Color.white;
        //Debug.Log(GameManager.ins.onlineButton.GetComponent<Image>().color);
    }
    private IEnumerator ReadyToPlay_Online()
    {
        yield return new WaitForSeconds(0.1f);
        startButton.SetActive(true);
        playerNameField.SetActive(true);
        smokes.SetActive(true);
        warningMessage.SetActive(false);
        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
        smokes.SetActive(false);
    }


    public void OnClickOffLineButton()
    {
        //StartCoroutine(SwitchToOffline());
        //StartCoroutine(ReadyToPlay_Offline());
        offlineYet.SetActive(true);
    }
    private IEnumerator SwitchToOffline()
    {
        //�ڑ��؂�
        RoomDoor.ins.IsOnline = false;
        RoomDoor.ins.DisconnectfromMasterServer();
        yield return new WaitForSeconds(0.1f);
        //GameManager.ins.offlineButton.GetComponent<Image>().color = new Color(1, 0.4f, 0.4f, 1);
        //GameManager.ins.onlineButton.GetComponent<Image>().color = Color.white;
        //Debug.Log(GameManager.ins.offlineButton.GetComponent<Image>().color);
    }
    private IEnumerator ReadyToPlay_Offline()
    {
        yield return new WaitForSeconds(0.1f);
        playerNameField.SetActive(false);
        startButton.SetActive(true);
        smokes.SetActive(false);
        warningMessage.SetActive(false);
    }
}

#endregion 1�y���[�h�V�[���̃n���h���[�z =====================================================================







#region 1�y�Q�[���V�[���̃n���h���[�z ========================================================================

public class SceneHandler_Game : SingletonCompo<SceneHandler_Game>, ISceneHandler
{
    public PieceManager pieceManager = null;
    public GameObject WhiteKing;
    public GameObject BlackKing;
    public GameObject winImage;
    public bool whiteturn = true;

    public event System.Action OnInitScene;
    public event System.Action OnExitScene;

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName.Game);
    }

    public IEnumerator InitScene()
    {
        OnInitScene?.Invoke();
        AdManager.HideAd(AdType.BannerDefault);
        GameManager.Compo.roopGameCounter.Count();

        if (RoomDoor.ins.IsOnline)
        { RoomDoor.ins.Join(); }
        yield return new WaitForSeconds(2.5f); //�R���[�`�����ꎞ���f����2.5�b��Ɏ��̍s����̏����ĊJ
        Debug.Log(GameObject.Find("ChessSet"));

        pieceManager = GameObject.Find("ChessSet").GetComponent<PieceManager>();
        Debug.Log("!!!" + pieceManager);
        WhiteKing = pieceManager.whiteKing0;
        BlackKing = pieceManager.blackKing0;
        winImage = GameObject.Find("WinImage");
        winImage.GetComponent<Image>().enabled = false;
        winImage.transform.GetChild(0).GetComponent<Text>().enabled = false;
        whiteturn = true;

        Debug.Log("--------------------�Q�[������������--------------------");  //Photon�̏���������̂ł����ɂ͊������Ȃ��B
    }

    public IEnumerator ExitScene()
    {
        yield return new WaitForSeconds(0.1f);
        OnExitScene?.Invoke();

        if (RoomDoor.ins.IsOnline)
        {
            RoomDoor.ins.Leave();
        }
    }


    public IEnumerator CheckMate()
    {
        if (BlackKing.activeSelf == false)
        {
            Debug.Log("menu�ɂ��ǂ�?");
            winImage.SetActive(true);
            winImage.GetComponent<Image>().enabled = true;
            winImage.transform.GetChild(0).GetComponent<Text>().enabled = true;
            winImage.transform.GetChild(0).GetComponent<Text>().text = "White Win!";
            yield return new WaitForSeconds(3);
            GameManager.Compo.LoadScene(SceneName.Menu);
        }
        if (WhiteKing.activeSelf == false)
        {
            Debug.Log("menu�ɂ��ǂ�?");
            winImage.SetActive(true);
            winImage.GetComponent<Image>().enabled = true;
            winImage.transform.GetChild(0).GetComponent<Text>().enabled = true;
            winImage.transform.GetChild(0).GetComponent<Text>().text = "Black Win!";
            yield return new WaitForSeconds(3);
            GameManager.Compo.LoadScene(SceneName.Menu);
        }
    }

    public void NextPlayer()
    {
        whiteturn = !whiteturn;
    }
}

#endregion 1�y�Q�[���V�[���̃n���h���[�z =====================================================================




    


#region 1�y�ݒ�V�[���̃n���h���[�z ========================================================================

public class SceneHandler_Setting : SingletonCompo<SceneHandler_Setting>, ISceneHandler
{
    public event System.Action OnInitScene;
    public event System.Action OnExitScene;

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName.Setting);
    }

    public IEnumerator InitScene()
    {
        yield return new WaitForSeconds(0);
        OnInitScene?.Invoke();

        //AudioManager.Compo.SetSlider();
    }
    public IEnumerator ExitScene()
    {
        yield return new WaitForSeconds(0.1f);
        OnExitScene?.Invoke();
    }
}

#endregion 1�y�ݒ�V�[���̃n���h���[�z =====================================================================