using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace Multiplayer
{
    public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
    {
        public GameObject spawnedPlayerPrefab;
        public Transform  player1Spawn,player2Spawn;

        private Vector3 playerSpawn;
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            playerSpawn = PhotonNetwork.PlayerList[0].IsLocal ? player1Spawn.position : player2Spawn.position;
            
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Player",playerSpawn,Quaternion.identity);
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            if (spawnedPlayerPrefab != null)
            {
                print("DESTROYING");
                PhotonNetwork.Destroy(spawnedPlayerPrefab);
            }
        }
    }
}
