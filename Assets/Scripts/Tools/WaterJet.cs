using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoatGame
{
    public class WaterJet : Tool
    {
        private const string JetTag = "WaterJetable";

        [SerializeField] private new WaterJetRenderer renderer;
        
        private readonly HashSet<WaterJetable> _jetables = new();
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

        private void FixedUpdate()
        {
            WaterJetable bestTarget = null;
            var bestScore = float.MaxValue;
            
            foreach (var j in _jetables)
            {
                var score = j.Score();

                if (score < bestScore)
                {
                    bestScore = score;
                    bestTarget = j;
                }
            }

            if (bestTarget == null)
            {
                StopJet();
                renderer.StopAiming();
                return;
            }

            AimJet(bestTarget);
            
            if (_boatInput.BoatControls.Fire.IsPressed())
                FireJet(bestTarget);
            else StopJet();
        }

        private void AimJet(WaterJetable bestTarget)
        {
            renderer.AimAt(bestTarget);
        }

        private void FireJet(WaterJetable bestTarget)
        {
            bestTarget.OnJetted();
            renderer.ShootAt(bestTarget);

            if (bestTarget.IsComplete())
                _jetables.Remove(bestTarget);
        }

        private void StopJet()
        {
            renderer.StopJet();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(JetTag))
                return;

            if (!other.TryGetComponent(out WaterJetable jetable))
                return;
            
            if (!jetable.IsComplete())
                _jetables.Add(jetable);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(JetTag))
                return;

            if (!other.TryGetComponent(out WaterJetable jetable))
                return;
            
            _jetables.Remove(jetable);
        }

        public override void OnActivate()
        {
            gameObject.SetActive(true);
        }

        public override void OnDeactivate()
        {
            _jetables.Clear();
            gameObject.SetActive(false);
        }
    }
}
