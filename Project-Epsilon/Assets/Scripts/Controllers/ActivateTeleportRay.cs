using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class ActivateTeleportRay : MonoBehaviour
    {
        [SerializeField] private InputActionReference teleportActivationButton;

        public UnityEvent onTeleportActivate, onTeleportCancel;


        private void OnEnable()
        {
            teleportActivationButton.action.performed += ActivateTeleport;
            teleportActivationButton.action.canceled += DeactivateTeleport;
        }
        private void OnDisable()
        {
            teleportActivationButton.action.performed -= ActivateTeleport;
            teleportActivationButton.action.canceled -= DeactivateTeleport;
        }

        private void ActivateTeleport(InputAction.CallbackContext obj)
        {
            onTeleportActivate.Invoke();
        }
    
        private void DeactivateTeleport(InputAction.CallbackContext obj)
        {
            Invoke(nameof(TurnOffTeleport),.1f);
        }

        private void TurnOffTeleport()
        {
            onTeleportCancel.Invoke();
        }
    }
}
