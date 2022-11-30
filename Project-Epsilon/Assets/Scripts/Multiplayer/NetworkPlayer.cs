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
        public int reference;

        private PhotonView _photonView;
        // Start is called before the first frame update
        void Start()
        {
            _photonView = GetComponent<PhotonView>();
            reference = PhotonNetwork.CurrentRoom.PlayerCount == 1 ? 1 : 2;
            gameObject.name = PhotonNetwork.CurrentRoom.PlayerCount == 1 ? "Player One" : "Player Two";

            if (!_photonView.IsMine)
            {
                GetComponent<AudioListener>().enabled = false;
                GetComponent<XROrigin>().enabled = false;
                transform.GetChild(0).GetChild(0).GetComponent<Camera>().enabled = false; // Camera
                transform.GetChild(0).GetChild(0).GetComponent<TrackedPoseDriver>().enabled = false; // Head Tracking
                transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ActionBasedController>().enabled = false; // Right Hand Direct interactor
                transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<ActionBasedController>().enabled = false; // Right Hand UI interactor
                transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<ActionBasedController>().enabled = false; // Left Hand Direct interactor
                transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<ActionBasedController>().enabled = false; // Left Hand UI interactor
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
