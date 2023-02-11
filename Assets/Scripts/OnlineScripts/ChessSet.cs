using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ChessSet : MonoBehaviourPunCallbacks
{
    public bool whiteturn = true;

    public void NextPlayer()
    {
        whiteturn = !whiteturn;
    }
}
