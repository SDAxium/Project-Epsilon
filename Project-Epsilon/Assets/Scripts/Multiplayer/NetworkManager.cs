using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Multiplayer
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        void Start()
        {
            
        }

        public void ConnectToServer()
        {
            Debug.Log("Connecting to server");
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to server");
            base.OnConnectedToMaster();

            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 2,
                IsVisible = true,
                IsOpen = true
            };

            PhotonNetwork.JoinOrCreateRoom("Quick Shot", roomOptions, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("room joined");
            base.OnJoinedRoom();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("A new player has joined");
            base.OnPlayerEnteredRoom(newPlayer);
        }
    }
}
