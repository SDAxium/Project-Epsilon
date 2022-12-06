using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem.XR;
using XRController = UnityEngine.XR.Interaction.Toolkit.XRController;

namespace Multiplayer
{
    public class NetworkPlayer : MonoBehaviour
    {
        public int currentScore;
        public int reference;

        private PhotonView _photonView;
        // Start is called before the first frame update
        void Start()
        {
            _photonView = GetComponent<PhotonView>();
            if (PhotonNetwork.IsConnectedAndReady)
            {
                reference = PhotonNetwork.CurrentRoom.Players[0].IsLocal ? 1 : 2;
                gameObject.name = PhotonNetwork.CurrentRoom.Players[0].IsLocal ? "Player One" : "Player Two";
            }

            if (!_photonView.IsMine)
            {
                GetComponent<AudioListener>().enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetAreaProvider()
        {
            GameObject.Find("Ground").GetComponent<TeleportationArea>().teleportationProvider =
                GetComponent<TeleportationProvider>();
        }
    }
}
