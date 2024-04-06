using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class createAndJoinRooms : MonoBehaviourPunCallbacks
{

    public InputField roomIdField;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomIdField.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomIdField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
