using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Controllers
{
    public class LocomotionControllerLocal : MonoBehaviour
    {
        public XRController leftTeleportRay;

        public XRController rightTeleportRay;

        public InputHelpers.Button teleportActivationButton;

        public float activationThreshold = 0.1f;
        // Start is called before the first frame update
        void Start()
        {
        
        }
    
        void Update()
        {
            if (leftTeleportRay)
            {
                leftTeleportRay.gameObject.SetActive(CheckIfActivated(leftTeleportRay));
            }
            if (rightTeleportRay)
            {
                rightTeleportRay.gameObject.SetActive(CheckIfActivated(rightTeleportRay));
            }
        }

        private bool CheckIfActivated(XRController controller)
        {
            controller.inputDevice.IsPressed(teleportActivationButton, out bool isActivated,
                activationThreshold);
            return isActivated;
        }
    }
}
