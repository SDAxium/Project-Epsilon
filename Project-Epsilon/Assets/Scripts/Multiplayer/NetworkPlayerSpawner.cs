using Photon.Pun;
using UnityEngine;

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
            playerSpawn = PhotonNetwork.CurrentRoom.PlayerCount == 1 ? player1Spawn.position : player2Spawn.position;
            
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Player",playerSpawn,transform.rotation);
            spawnedPlayerPrefab.GetComponent<NetworkPlayer>().reference = PhotonNetwork.CurrentRoom.PlayerCount == 1 ? 1 : 2;
            print($"Player reference is {spawnedPlayerPrefab.GetComponent<NetworkPlayer>().reference}");
            
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            PhotonNetwork.Destroy(spawnedPlayerPrefab);
        }
    }
}
