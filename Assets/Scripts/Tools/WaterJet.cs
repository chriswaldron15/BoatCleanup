using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace BoatGame
{
    public class WaterJet : Tool
    {
        private const string JetTag = "WaterJetable";

        [SerializeField] private new WaterJetRenderer renderer;
        
        private readonly List<(WaterJetable jetable, int triggerCount)> _jetables = new();
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
                var score = j.jetable.Score();

                if (score < bestScore)
                {
                    bestScore = score;
                    bestTarget = j.jetable;
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
            {
                var index = GetIndex(bestTarget);
                
                if (index != -1)
                    _jetables.RemoveAtSwapBack(index);
            }
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
            {
                var index = GetIndex(jetable);

                if (index == -1)
                    _jetables.Add((jetable, 1));
                else
                    _jetables[index] = (jetable, _jetables[index].triggerCount + 1);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(JetTag))
                return;

            if (!other.TryGetComponent(out WaterJetable jetable))
                return;

            var index = GetIndex(jetable);

            if (index == -1)
                return;

            if (_jetables[index].triggerCount == 1)
                _jetables.RemoveAtSwapBack(index);
            else _jetables[index] = (jetable, _jetables[index].triggerCount - 1);
        }

        private int GetIndex(WaterJetable jetable)
        {
            for (int i = 0, iMax = _jetables.Count; i < iMax; i++)
            {
                if (_jetables[i].jetable == jetable)
                    return i;
            }

            return -1;
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
