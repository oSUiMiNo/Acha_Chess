using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

/// <summary>
/// アバターの子オブジェクトのTextのオブジェクトにアタッチしてある。
/// </summary>

// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class AvatarNameDisplay : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        //TextMeshProUGUI nameLabel = GetComponent<TextMeshProUGUI>();
        //TextMeshProUGUI nameLabel = transform.Find("NameDisplay/Text").GetComponent<TextMeshProUGUI>();
        //Debug.Log(nameLabel);
        //Debug.Log("photonView.Owner.NickName " + photonView.Owner.NickName);
        //Debug.Log("photonView.OwnerActorNr " + photonView.OwnerActorNr);

        //var players = PhotonNetwork.PlayerList;
        //Player masterPlayer = null;
        //Player nomalPlayer = null;
        //foreach (var a in players)
        //{
        //    if (a.IsMasterClient)
        //    {
        //        masterPlayer = a;
        //    }
        //    else
        //    {
        //        nomalPlayer = a;
        //    }
        //}

        //if(masterPlayer.NickName == nomalPlayer.NickName)
        //{
        //    nameLabel.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr})";
        //}
        //else
        //{
        //    // プレイヤー名とプレイヤーIDを表示する
        //    nameLabel.text = $"{photonView.Owner.NickName}";
        //}


    }
}