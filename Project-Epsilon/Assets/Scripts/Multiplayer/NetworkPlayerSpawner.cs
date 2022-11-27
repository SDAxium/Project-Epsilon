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
            playerSpawn = PhotonNetwork.CurrentRoom.PlayerCount == 1 ? player1Spawn.position : player2Spawn.position;
            
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Player",playerSpawn,Quaternion.identity);
            
            /*
             * There HAS to be a more elegant way of doing this but because I can't figure it out, I'll just have the
             * player's head tracking and audio listeners turned off by default and turned on after they are spawned in
             * Hopefully that works
             */
            // spawnedPlayerPrefab.GetComponent<AudioListener>().enabled = true;
            // spawnedPlayerPrefab.GetComponent<XROrigin>().enabled = true;
            // spawnedPlayerPrefab.transform.GetChild(0).GetChild(0).gameObject
            //     .GetComponent<TrackedPoseDriver>().enabled = true;
            // spawnedPlayerPrefab.transform.GetChild(0).GetChild(0).GetComponent<Camera>().enabled = true;
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            PhotonNetwork.Destroy(spawnedPlayerPrefab);
        }
    }
}
