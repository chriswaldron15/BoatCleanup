using UnityEngine;

namespace BoatGame
{
    public class RotationalGate : MonoBehaviour
    {
        [SerializeField] private HingeJoint hinge;
        [SerializeField] private bool flipDirection;
        [SerializeField] private NormalizedPuzzleReceiver[] receivers;

        private float _wrapAroundMultiplier;
        private float _prevAngle;
        private float _totalRotation;

        private Vector2 _hingeMinMax;

        private void Start()
        {
            _prevAngle = hinge.angle;

            if (float.IsNaN(_prevAngle))
                _prevAngle = 0f;
            
            var limits = hinge.limits;
            _hingeMinMax = new Vector2(flipDirection ? limits.max : limits.min, flipDirection ? limits.min : limits.max);
            UpdateNormalizedAmount(true);
        }

        private void FixedUpdate()
        {
            var newAngle = hinge.angle;

            if (float.IsNaN(newAngle))
                newAngle = 0f;

            if (newAngle > _prevAngle && _prevAngle < -45f && newAngle > 45f)
                _wrapAroundMultiplier -= 360f;
            else if (newAngle < _prevAngle && _prevAngle > 45f && newAngle < -45f)
                _wrapAroundMultiplier += 360f;

            _totalRotation = newAngle + _wrapAroundMultiplier;
            _prevAngle = newAngle;
            
            UpdateNormalizedAmount(false);
        }

        private void UpdateNormalizedAmount(bool immediate)
        {
            var normalizedAmount = Mathf.InverseLerp(_hingeMinMax.x, _hingeMinMax.y, _totalRotation);
            normalizedAmount = Mathf.Clamp01(normalizedAmount);
            
            for (int i = 0, iMax = receivers.Length; i < iMax; i++)
            {
                if (immediate)
                    receivers[i].SetStateImmediate(normalizedAmount);
                else receivers[i].UpdateState(normalizedAmount);
            }
        }
    }
}
