using UnityEngine;

namespace BoatGame
{
    public class CameraLookAtPoint : MonoBehaviour
    {
        [field: SerializeField] public Transform Target { get; private set; }
        [field: SerializeField] public float OrthoSize { get; private set; }
        [field: SerializeField] public float PlayerPositionInfluence { get; private set; }

        private bool _isDisabled, _isTriggered;
        private short _triggerCount;

        private void OnDisable()
        {
            if (_triggerCount > 0)
            {
                _triggerCount = 0;
                Exit();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _triggerCount++;
            
            if (_isDisabled || _isTriggered)
                return;

            _isTriggered = true;
            PlayerCamera.Instance.SetLookAtOverride(this);
        }

        private void OnTriggerExit(Collider other)
        {
            _triggerCount--;

            if (_triggerCount < 0)
            {
                Debug.LogError("Trigger count went below zero", this);
                _triggerCount = 0;
            }

            if (_triggerCount <= 0)
            {
                Exit();
            }
        }

        private void Exit()
        {
            _isTriggered = false;
            PlayerCamera.Instance.ClearLookAtOverride(this);
        }
    }
}
