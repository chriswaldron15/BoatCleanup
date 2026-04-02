using UnityEngine;

namespace BoatGame
{
    public class CameraLookAtPoint : NormalizedPuzzleReceiver
    {
        [field: SerializeField] public Transform Target { get; private set; }
        [field: SerializeField] public float OrthoSize { get; private set; }
        [field: SerializeField] public float PlayerPositionInfluence { get; private set; }

        private bool _isDisabled, _isTriggered;
        private short _triggerCount;
        
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
                Debug.LogError("Trigger count went below zero", this);

            if (_triggerCount <= 0)
            {
                _isTriggered = false;
                PlayerCamera.Instance.ClearLookAtOverride(this);
            }
        }

        public override void SetStateImmediate(float normalizedCompletionValue)
        {
            UpdateState(normalizedCompletionValue);
        }

        public override void UpdateState(float normalizedCompletionValue)
        {
            if (normalizedCompletionValue >= 1f)
            {
                _isDisabled = true;
                gameObject.SetActive(false);
                
                if (_isTriggered)
                    OnTriggerExit(null);
            }
        }
    }
}
