using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Unity.XR.CoreUtils;

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
                transform.GetChild(0).GetChild(0).GetComponent<Camera>().enabled = false;
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
