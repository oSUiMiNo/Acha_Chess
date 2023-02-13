using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
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


    // プレイヤー名とプレイヤーIDを表示する
    public void SetAvaterNameDisplay()
    {
        photonView.RPC(nameof(SetAvaterNameDisplay_RPC), RpcTarget.All);
    }
    [PunRPC]
    public void SetAvaterNameDisplay_RPC()
    {
        TextMeshProUGUI nameLabel = transform.Find("NameDisplay/Text").GetComponent<TextMeshProUGUI>();
        Debug.Log(nameLabel);
        Debug.Log("photonView.Owner.NickName " + photonView.Owner.NickName);
        Debug.Log("photonView.OwnerActorNr " + photonView.OwnerActorNr);

        var players = PhotonNetwork.PlayerList;
        Player masterPlayer = null;
        Player nomalPlayer = null;
        foreach (var a in players)
        {
            if (a.IsMasterClient)
            {
                masterPlayer = a;
            }
            else
            {
                nomalPlayer = a;
            }
        }

        if (masterPlayer.NickName == nomalPlayer.NickName)
        {
            nameLabel.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr})";
        }
        else
        {
            nameLabel.text = $"{photonView.Owner.NickName}";
        }
    }
}
