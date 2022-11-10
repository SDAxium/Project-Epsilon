using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Multiplayer
{
    public class NetworkPlayer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void setAreaProvider()
        {
            GameObject.Find("Ground").GetComponent<TeleportationArea>().teleportationProvider =
                this.GetComponent<TeleportationProvider>();
        }
    }
}
