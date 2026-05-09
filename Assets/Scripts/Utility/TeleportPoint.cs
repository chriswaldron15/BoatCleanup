using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BoatGame
{
    public class TeleportPoint : MonoBehaviour
    {
        [SerializeField] private InputActionReference teleportAction;

        private void Awake()
        {
            teleportAction.action.performed += OnTeleportPerformed;
            teleportAction.action.Enable();
        }

        private void OnDestroy()
        {
            teleportAction.action.performed -= OnTeleportPerformed;
            teleportAction.action.Disable();
        }

        private void OnTeleportPerformed(InputAction.CallbackContext obj)
        {
            if (!obj.performed)
                return;
            
            PlayerBoat.Instance.SpawnAt(transform, true);
        }
    }
}
