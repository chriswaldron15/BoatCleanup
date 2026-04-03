using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class BoatThrottle : MonoBehaviour
    {
        [SerializeField] private float topSpeed;
        [SerializeField] private float topReverseSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private ParticleSystem smokeParticles;

        [SerializeField, MinMaxSlider(0, 100)]
        private Vector2 particleEmissionRateMinMax; 
        
        private BoatInput _boatInput;
        private float _force;
        private float _inputValue;

        private void Awake()
        {
            _boatInput = new();
            _boatInput.Enable();
        }

        private void Update()
        {
            _inputValue = _boatInput.BoatControls.Throttle.ReadValue<float>();
            
            var targetSpeed = _inputValue < 0 ? -topReverseSpeed : _inputValue * topSpeed;
            _force = Mathf.MoveTowards(_force, targetSpeed, Time.deltaTime * acceleration);
            
            UpdateParticles(targetSpeed);

            if (Mathf.Approximately(_force, 0f))
                return;

            var target = PlayerBoat.Instance.Rigidbody;
            target.AddForce(target.transform.forward * (_force * Time.deltaTime * target.mass), ForceMode.Acceleration);

        }

        private void UpdateParticles(float targetSpeed)
        {
            // var n = Mathf.InverseLerp(0f, targetSpeed, _force);
            //
            // var emission = smokeParticles.emission;
            // var rate = emission.rateOverTime;
            //
            // rate.constant = Mathf.Lerp(particleEmissionRateMinMax.x, particleEmissionRateMinMax.y, n);
            // emission.rateOverTime = rate;
        }
    }
}
