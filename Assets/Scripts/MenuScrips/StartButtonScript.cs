using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StartButtonScript : MonoBehaviour
{
    public void OnClickStratButton()
    {
        string nameText = SceneHandler_Menu.Compo.playerNameField.GetComponent<InputField>().text;
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
            SceneHandler_Menu.Compo.warningMessage.SetActive(true);
        }
        else
        if (RoomDoor.ins.IsOnline == false)
        {
            GameManager.Compo.LoadScene(SceneName.Game);
        }
        //Vector3 a = transform.position; //ここではtransform.pojitionではなくて、transforn.pojitionの(0, 0, 0)
    }

    ////roomDoorWay = RoomDoorWay.instanceができない理由
    //transform.position = Vector3.zero;という数値を代入している

    //Debug.Log(a);   ←(0,0,0)
    //a = new Vector3(a.x + 3, a.y, a.z);
    //Debug.Log(a);  ←(3,0,0)
    //Debug.Log(transform.position);  (0,0,0)
    //roomDoorWay = RoomDoorWay.instance 
    //理由① roomDoorWayは新しいインスタンスであって、RoomDoorWay.instanceはRoomDoorWayクラスのinstance関数、つまりRoomDoorWayクラスにあるインスタンスである。
    //roomDoorWayとRoomDoorWay.instanceは代入したとしても別物。*分かり易くinstanceという名前にしてるが、これは関数だからIとか別の関数名にもできる
    //理由② RoomDoorWayクラスの関数を使う時にわざわざGetComponentしなくても
    //RoomDoorWay.instance.関数名で使うことは出来るので便利だが、このように代入しようとすると壊されちゃう。シングルトンだから。
}