using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BoatGame
{
    public class InWaterStateDetector : MonoBehaviour
    {
        public UnityAction OnWaterExited { get; set; }
        public UnityAction<WaterVolume> OnWaterEntered { get; set; }
        
        public WaterVolume CurrentWaterVolume { get; private set; }
        private static int? _waterLayerMask;

        private readonly Dictionary<WaterVolume, WaterCount> _colliderCounts = new(4);

        private void Awake()
        {
            if (!_waterLayerMask.HasValue)
                _waterLayerMask = LayerMask.NameToLayer("Water");
        }

        private void OnTriggerEnter(Collider other)
        {
            // ReSharper disable once PossibleInvalidOperationException
            if (other.gameObject.layer != _waterLayerMask.Value)
                return;

            if (other.transform.parent.TryGetComponent(out WaterVolume waterVolume))
            {
                if (_colliderCounts.TryGetValue(waterVolume, out var count))
                    count.Increment();
                else _colliderCounts.Add(waterVolume, new WaterCount());
                
                if (CurrentWaterVolume != waterVolume)
                {
                    EnterWaterVolume(waterVolume);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // ReSharper disable once PossibleInvalidOperationException
            if (other.gameObject.layer != _waterLayerMask.Value)
                return;

            if (other.transform.parent.TryGetComponent(out WaterVolume waterVolume))
            {
                if (_colliderCounts.TryGetValue(waterVolume, out var count))
                {
                    count.Decrement();

                    if (CurrentWaterVolume == waterVolume && count.Count == 0)
                    {
                        EnterNextRelevantWater();
                    }
                }
            }
        }

        private void EnterNextRelevantWater()
        {
            foreach (var c in _colliderCounts)
            {
                if (c.Value.Count > 0)
                {
                    EnterWaterVolume(c.Key);
                    return;
                }
            }
                
            CurrentWaterVolume = null;
            OnWaterExited?.Invoke();
        }

        private void EnterWaterVolume(WaterVolume waterVolume)
        {
            CurrentWaterVolume = waterVolume;
            OnWaterEntered?.Invoke(CurrentWaterVolume);
        }

        private class WaterCount
        {
            public int Count { get; private set; } = 1;

            public void Increment() => Count++;
            public void Decrement() => Count--;
        }
    }
}
