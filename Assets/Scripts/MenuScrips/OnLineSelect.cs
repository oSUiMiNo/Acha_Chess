using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class OnLineSelect : MonoBehaviour
{
    public bool Onlined = false;
    //public void OnClickOnLineButton()
    //{
    //    RoomDoorWay.ins.IsOnline = true;
    //    RoomDoorWay.ins.ConnectToMasterServer();
    //    Onlined = true;
    //    GameManager.ins.StartButton.SetActive(true);
    //    GameManager.ins.InputField.SetActive(true);
    //    GameManager.ins.Smokes.SetActive(true);
    //}

    #region 【まく】提案
    //public void OnClickOnLineButton()
    //{
    //    /// <Summary>
    //    ///【まく】
    //    /// 接続処理中は PhotonNetwork.IsConnectedAndReady が false になってしまい、下のコルーチンが中断してしまう。
    //    /// ので、すでに接続されている場合はむやみに RoomDoorWay.ins.ConnectToMasterServer() 呼ばないほうがよさげ。
    //    /// ちなみに、
    //    /// ネームサーバー(photonサーバー)につながった状態だと PhotonNetwork.IsConnected が true
    //    /// マスターサーバーにつながった状態だと PhotonNetwork.IsConnectedAndReady も true
    //    /// </Summary>
    //    StartCoroutine(SwitchToOnline());
    //    StartCoroutine(ReadyToPlay());
    //}
    //private IEnumerator SwitchToOnline()
    //{
    //    if (!PhotonNetwork.IsConnectedAndReady)
    //    {
    //        RoomDoorWay.ins.IsOnline = true;
    //        RoomDoorWay.ins.ConnectToMasterServer();
    //    }
    //    yield return new WaitForSeconds(0.1f);
    //    //GameManager.ins.onlineButton.GetComponent<Image>().color = Color.red;
    //    //GameManager.ins.offlineButton.GetComponent<Image>().color = Color.white;
    //    //Debug.Log(GameManager.ins.onlineButton.GetComponent<Image>().color);
    //}
    //private IEnumerator ReadyToPlay()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    GameManager.ins.startButton.SetActive(true);
    //    GameManager.ins.playerNameField.SetActive(true);
    //    GameManager.ins.smokes.SetActive(true);
    //    GameManager.ins.warningMessage.SetActive(false);
    //    yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
    //    GameManager.ins.smokes.SetActive(false);
    //}
    #endregion
}