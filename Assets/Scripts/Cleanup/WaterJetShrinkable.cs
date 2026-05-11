using UnityEngine;
using UnityEngine.Events;

namespace BoatGame
{
    public class WaterJetShrinkable : WaterJetable
    {
        [SerializeField] private Transform target;
        [SerializeField] private float shrinkRate;
        
        private float _scale = 1f;
        private Vector3 _startScale;

        private void Awake()
        {
            _startScale = target.localScale;
        }

        public override void OnJetted()
        {
            _scale = Mathf.Clamp01(_scale - Time.fixedDeltaTime * shrinkRate);
            target.localScale = _startScale * _scale;

            if (IsComplete())
            {
                target.localScale = Vector3.zero;
                onComplete?.Invoke();
            }
        }

        public override bool IsComplete()
        {
            return _scale <= 0f;
        }
    }
}
