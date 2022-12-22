using Controllers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

namespace Multiplayer
{
    public class NetworkPlayer : MonoBehaviour, IPunInstantiateMagicCallback
    {
        public int currentScore;
        public int reference;
        
        private GameObject _gameManager;
        private PhotonView _photonView;
        
        private void Start()
        {
            _gameManager = GameObject.Find("Game Manager"); 
            _photonView = GetComponent<PhotonView>();
            
            if (PhotonNetwork.IsConnectedAndReady) // If connected to the photon servers
            {
                // Photon has a sorted list of all players in a room. Check if the local player is the first one in the 
                // room and set their variables according to that 
                reference = PhotonNetwork.PlayerList[0].IsLocal ? 1 : 2;
                gameObject.name = PhotonNetwork.PlayerList[0].IsLocal ? "Player One" : "Player Two";
            }
            
            if (_photonView.IsMine)
            {
                // TODO: There is definitely a better way to look for the fade screen. Look for it and remove this comment
                _gameManager.transform.GetChild(3).GetComponent<SceneTransitionManager>().fadeScreen 
                    = gameObject.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<FadeScreen>();
            }
            else
            {
                // Turns off the audio listeners in the scene for players that aren't local. 
                GetComponent<AudioListener>().enabled = false;
            }
        }

        /// <summary>
        /// I am using it to send the game object of the player over the network.  This allows other players to access
        /// the current player's game object
        /// </summary>
        /// <param name="info"></param>
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            info.Sender.TagObject = gameObject;
        }
        
        [PunRPC]
        public void UpdateScore()
        {
            if (reference == 1)
            {
                _gameManager.GetComponent<GameManager>().p1Score = currentScore;
            }
            if (reference == 2)
            {
                _gameManager.GetComponent<GameManager>().p2Score = currentScore;
            }

            _gameManager.GetComponent<GameManager>().SetScoreText();
        }

        /// <summary>
        /// Takes the teleport area of the floor and sets the provider for it to the player
        ///
        /// Note: This is now obsolete as teleportation areas are no longer used. 
        /// </summary>
        public void SetAreaProvider()
        {
            GameObject.Find("Ground").GetComponent<TeleportationArea>().teleportationProvider =
                GetComponent<TeleportationProvider>();
        }
    }
}
