using Controllers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem.XR;
using XRController = UnityEngine.XR.Interaction.Toolkit.XRController;

namespace Multiplayer
{
    public class NetworkPlayer : MonoBehaviour, IPunInstantiateMagicCallback
    {
        public int currentScore;
        public int reference;


        private GameObject _gameManager;
        private PhotonView _photonView;
        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameObject.Find("Game Manager");
            _photonView = GetComponent<PhotonView>();
            if (PhotonNetwork.IsConnectedAndReady)
            {
                print($"Player at 0 is local: {PhotonNetwork.PlayerList[0].IsLocal}");
                reference = PhotonNetwork.PlayerList[0].IsLocal ? 1 : 2;
                gameObject.name = PhotonNetwork.PlayerList[0].IsLocal ? "Player One" : "Player Two";
            }
            
            if (_photonView.IsMine)
            {
                _gameManager.transform.GetChild(3).GetComponent<SceneTransitionManager>().fadeScreen 
                    = gameObject.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<FadeScreen>();
            }
            else
            {
                GetComponent<AudioListener>().enabled = false;
                // turn off everything that isnt mine
                
            }
        }

        // Update is called once per frame
        void Update()
        {
        
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

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            info.Sender.TagObject = gameObject;
        }
        
        public void SetAreaProvider()
        {
            GameObject.Find("Ground").GetComponent<TeleportationArea>().teleportationProvider =
                GetComponent<TeleportationProvider>();
        }
    }
}
