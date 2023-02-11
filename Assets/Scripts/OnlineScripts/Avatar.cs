using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Avatar : MonoBehaviourPunCallbacks
{
    public void LockAction()
    {
        photonView.RPC(nameof(LockAction_RPC), RpcTarget.All);
    }
    [PunRPC]
    private void LockAction_RPC()
    {
        Debug.Log("あああ");　//入ってない
        if (RoomDoor.ins.IsOnline)
        {
            Debug.Log("LockAction");
            GetComponent<PieceSelector>().enabled = false;
        }
    }

    public void AllowAction()
    {
        photonView.RPC(nameof(AllowAction_RPC), RpcTarget.All);
    }
    [PunRPC]
    private void AllowAction_RPC()
    {
        GetComponent<PieceSelector>().enabled = true;
    }
}
