using System;
using UnityEngine;
using UnityEngine.Events;

namespace BoatGame
{
    public class StaticTowable : Towable
    {
        [SerializeField] private SpringJoint spring;
        [SerializeField] private float requiredMagnitude = 20f;
        [SerializeField] private float windupTime = 1f;
        [SerializeField] private float windDownSpeed;

        public UnityEvent<float> towTick;
        public UnityEvent onCompleted;

        private float _windupAmount;

        private void Update()
        {
            if (IsComplete())
                return;
            
            var appliedMagnitude = spring.currentForce.magnitude;
            var windupBefore = _windupAmount;

            if (appliedMagnitude >= requiredMagnitude)
                _windupAmount = Mathf.Clamp(_windupAmount + Time.deltaTime, 0f, windupTime);
            else _windupAmount = Mathf.Clamp(_windupAmount - Time.deltaTime * windDownSpeed, 0f, windupTime);
            
            if (!Mathf.Approximately(windupBefore, _windupAmount))
                towTick?.Invoke(_windupAmount / windupTime);
            
            if (IsComplete())
                onCompleted?.Invoke();
        }

        public override void OnAttached()
        {
            base.OnAttached();
            spring.connectedBody = PlayerBoat.Instance.Rigidbody;
        }

        public override void OnDetached()
        {
            base.OnDetached();
            spring.connectedBody = null;
        }

        public override bool IsComplete()
        {
            return _windupAmount >= windupTime;
        }
    }
}
