using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void ConnectedToServer()
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Connecting to server");
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to server");
            base.OnConnectedToMaster();

            var roomOptions = new RoomOptions
            {
                MaxPlayers = 3,
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
