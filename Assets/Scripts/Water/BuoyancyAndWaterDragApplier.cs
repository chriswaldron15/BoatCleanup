using System;
using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class BuoyancyAndWaterDragApplier : MonoBehaviour
    {
        private static readonly int WaveHeightRef = Shader.PropertyToID("_Noise");
        private static readonly int WaveScrollRef = Shader.PropertyToID("_Scroll_Speed");
        private static readonly int WaveScaleRef = Shader.PropertyToID("_Wave_Scale");
        
        [SerializeField] private new Rigidbody rigidbody;

        [SerializeField] private float waterDamping;
        [SerializeField] private float waterAngularDamping;
        
        [SerializeField] private Vector2 pushDepthRange;
        [SerializeField] private float buoyancyPushStrength;

        [SerializeField] private InWaterStateDetector waterStateDetector; 
        [SerializeField] private bool treatAsAlwaysInWater;

        private bool _isInWater;

        private float _originalDamping, _originalAngularDamping;

        [Header("Heightmap")]
        [SerializeField] private Material waterMaterial;

        private Texture2D _heightmap;
        private Vector2 _waveScrollSpeed;
        private Vector2 _heightmapScale;
        private float _waveScale;
        private int _heightmapSize;

        private void Awake()
        {
            waterStateDetector.OnWaterEntered += OnWaterEntered;
            waterStateDetector.OnWaterExited += OnWaterExited;

            _originalDamping = rigidbody.linearDamping;
            _originalAngularDamping = rigidbody.angularDamping;

            _heightmap = waterMaterial.GetTexture(WaveHeightRef) as Texture2D;
            _heightmapScale = waterMaterial.GetTextureScale(WaveHeightRef);
            // ReSharper disable once PossibleNullReferenceException
            _heightmapSize = _heightmap.width;
            var ss = waterMaterial.GetVector(WaveScrollRef);
            _waveScrollSpeed = new Vector2(ss.x, ss.y);
            _waveScale = waterMaterial.GetFloat(WaveScaleRef);
        }

        private void OnDestroy()
        {
            waterStateDetector.OnWaterEntered -= OnWaterEntered;
            waterStateDetector.OnWaterExited -= OnWaterExited;
        }

        private void FixedUpdate()
        {
            if (!_isInWater)
                return;
            
            var yPosition = rigidbody.position.y;
            var actualHeight = waterStateDetector.CurrentWaterVolume.WaterHeight + GetWaveHeight();
            var depth = actualHeight - yPosition;
            
            var modulatedPushStrength = Mathf.InverseLerp(pushDepthRange.x, pushDepthRange.y, depth);
            var force = Vector3.up * (buoyancyPushStrength * modulatedPushStrength);

            rigidbody.AddForce(force, ForceMode.Acceleration);
        }

        private float GetWaveHeight()
        {
            // this needs to be identical to the shader
            var pos = rigidbody.position;
            var pos2d = new Vector2(pos.x, pos.z) + (Time.time * _waveScrollSpeed);
            return _heightmap.GetPixel(Mathf.RoundToInt(pos2d.x * _heightmapSize * _heightmapScale.x), Mathf.RoundToInt(pos2d.y * _heightmapSize * _heightmapScale.y)).g * _waveScale * -1f;
        }

        private void OnWaterEntered(WaterVolume other)
        {
            if (_isInWater)
                return;
            
            _isInWater = true;
            rigidbody.linearDamping = waterDamping;
            rigidbody.angularDamping = waterAngularDamping;
        }

        private void OnWaterExited()
        {
            _isInWater = false;
            
            if (!treatAsAlwaysInWater)
            {
                rigidbody.linearDamping = _originalDamping;
                rigidbody.angularDamping = _originalAngularDamping;
            }
        }

        [Button]
        private void CopyDampingFromRigidbody()
        {
            waterDamping = rigidbody.linearDamping;
            waterAngularDamping = rigidbody.angularDamping;
            
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }
    }
}
