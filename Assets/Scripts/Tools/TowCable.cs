using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoatGame
{
    public class TowCable : Tool
    {
        private const string TowableTag = "Towable";

        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private SpringJoint spring;
        [SerializeField] private float detachDistance;
        [SerializeField] private TowRopeRenderer towRopeRenderer;
        [SerializeField] private ToolTarget toolTarget;
        
        private Towable _towTarget;
        private readonly HashSet<Towable> _targets = new HashSet<Towable>(4);
        private BoatInput _boatInput;

        private void Awake()
        {
            _boatInput = new BoatInput();
            _boatInput.Enable();
        }

        private void OnDestroy()
        {
            _boatInput.Disable();
            _boatInput.Dispose();
        }

        private void Update()
        {
            if (_towTarget != null)
            {
                CheckForDetach();
                return;
            }

            Towable bestTarget = null;
            float bestScore = float.MaxValue;

            foreach (var target in _targets)
            {
                var score = target.Score();

                if (score < bestScore)
                {
                    bestScore = score;
                    bestTarget = target;
                }   
            }

            if (bestTarget == null)
            {
                toolTarget.Hide();
                return;
            }
            
            toolTarget.PositionAndShow(bestTarget.AttachPoint.position);
            
            if (_boatInput.BoatControls.Fire.WasPressedThisFrame())
                AttachTo(bestTarget);
        }

        private void FixedUpdate()
        {
            towRopeRenderer.UpdateRenderer(rigidbody, spring.connectedBody);
        }

        private void LateUpdate()
        {
            towRopeRenderer.UpdateRenderer(rigidbody, spring.connectedBody);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(TowableTag))
                return;

            if (!other.attachedRigidbody.TryGetComponent(out Towable towable))
                return;

            _targets.Add(towable);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(TowableTag))
                return;

            if (!other.attachedRigidbody.TryGetComponent(out Towable towable))
                return;

            _targets.Remove(towable);
        }

        private void AttachTo(Towable target)
        {
            _towTarget = target;
            spring.connectedBody = _towTarget.Rigidbody;
            spring.connectedAnchor = _towTarget.AttachPoint.localPosition;
            toolTarget.Hide();
        }

        private void DetachFromCurrentTarget()
        {
            if (_towTarget != null)
            {
                spring.connectedBody = null;
                _towTarget = null;
            }
        }

        private void CheckForDetach()
        {
            if (_boatInput.BoatControls.Fire.WasPressedThisFrame() || Vector3.Distance(transform.position, _towTarget.AttachPoint.position) >= detachDistance)
                DetachFromCurrentTarget();
        }

        public override void OnActivate()
        {
            gameObject.SetActive(true);
        }

        public override void OnDeactivate()
        {
            DetachFromCurrentTarget();
            toolTarget.Hide();
            gameObject.SetActive(false);
        }
    }
}
