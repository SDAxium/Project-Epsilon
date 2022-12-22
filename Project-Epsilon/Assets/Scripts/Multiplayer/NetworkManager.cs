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
            
            bool playerExists = GameObject.FindWithTag("Player") != null;
            
            if (!playerExists && SceneManager.GetActiveScene().buildIndex == 0)
            {
                gameObject.GetComponent<NetworkPlayerSpawner>().OnJoinedRoom();
            }

            if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
            {
                ConnectToServer();
            }
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
