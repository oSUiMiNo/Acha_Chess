using UnityEngine;
using Photon.Pun;
using TMPro;

/// <summary>
/// アバターの子オブジェクトのTextのオブジェクトにアタッチしてある。
/// </summary>

// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class AvatarNameDisplay : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        TextMeshProUGUI nameLabel = GetComponent<TextMeshProUGUI>();
        Debug.Log(nameLabel);
        Debug.Log("photonView.Owner.NickName " + photonView.Owner.NickName);
        Debug.Log("photonView.OwnerActorNr " + photonView.OwnerActorNr);
        // プレイヤー名とプレイヤーIDを表示する
        nameLabel.text = $"({photonView.OwnerActorNr}){photonView.Owner.NickName}";
    }
}