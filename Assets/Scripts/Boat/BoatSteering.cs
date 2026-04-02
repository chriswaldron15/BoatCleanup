using UnityEngine;

namespace BoatGame
{
    public class BoatSteering : MonoBehaviour
    {
        [SerializeField] private float maxTurnAngle;
        [SerializeField] private float turnSpeed;
        
        private BoatInput _boatInput;
        private float _turnForce;
        private float _inputValue;

        private void Awake()
        {
            _boatInput = new();
            _boatInput.Enable();
        }

        private void FixedUpdate()
        {
            _inputValue = _boatInput.BoatControls.Steer.ReadValue<float>();
            
            _turnForce = Mathf.MoveTowards(_turnForce, _inputValue * maxTurnAngle, Time.deltaTime * turnSpeed);
            
            if (Mathf.Approximately(_turnForce, 0f))
                return;

            PlayerBoat.Instance.Rigidbody.MoveRotation(PlayerBoat.Instance.Rigidbody.rotation * Quaternion.Euler(0f, _turnForce * Time.fixedDeltaTime, 0f));
        }
    }
}
