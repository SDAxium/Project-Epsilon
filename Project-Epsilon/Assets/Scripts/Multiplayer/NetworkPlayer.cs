using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Multiplayer
{
    public class NetworkPlayer : MonoBehaviour
    {
        public int reference;

        // Start is called before the first frame update
        void Start()
        {
        
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
