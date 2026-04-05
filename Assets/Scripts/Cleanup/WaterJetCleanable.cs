using System;
using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class WaterJetCleanable : WaterJetable
    {
        private static readonly int Cleanness = Shader.PropertyToID("_CleanAmount");
        
        [SerializeField] private float requiredJetTime = 1f;
        [SerializeField, Range(0f, 0.99f)] private float startCleanAmount;
        [SerializeField] private new Renderer renderer;

        private float _cleanTime;

        private void Awake()
        {
            UpdateMaterial();
        }

        public override void OnJetted()
        {
            _cleanTime += Time.deltaTime;
            _cleanTime = Mathf.Clamp(_cleanTime, 0f, requiredJetTime);
            UpdateMaterial();
        }

        public override bool IsComplete()
        {
            return _cleanTime >= requiredJetTime;
        }

        [Button]
        private void UpdateMaterial()
        {
            float cleanAmount = _cleanTime / requiredJetTime;
            float maxCleanAmount = 1f - startCleanAmount;
            
            var mpb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(mpb);
            mpb.SetFloat(Cleanness, startCleanAmount + cleanAmount * maxCleanAmount);
            renderer.SetPropertyBlock(mpb);
        }
    }
}
