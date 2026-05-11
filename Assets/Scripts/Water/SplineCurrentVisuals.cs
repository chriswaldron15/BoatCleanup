using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using NaughtyAttributes;

namespace BoatGame
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(LineRenderer))]
    public class SplineCurrentVisuals : MonoBehaviour
    {
        [SerializeField] private SplineCurrent splineCurrent;
        [SerializeField] private float visualSpeedMultiplier = 2.0f;
        [SerializeField] private float pointsPerUnit = 2.0f;
        [SerializeField] private bool useSplineWidth = true;
        [SerializeField] private float yOffset;
        
        private LineRenderer _lineRenderer;
        private Material _material;
        private float _scrollOffset;

        private void OnEnable()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            RefreshMaterial();
            UpdateLineRenderer();
            
            Spline.Changed += OnSplineChanged;
        }

        private void OnDisable()
        {
            Spline.Changed -= OnSplineChanged;
        }

        private void RefreshMaterial()
        {
            if (_lineRenderer == null) return;
            
            if (Application.isPlaying)
            {
                if (_material == null) _material = _lineRenderer.material;
            }
            else
            {
                _material = _lineRenderer.sharedMaterial;
            }
        }

        private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modification)
        {
            UpdateLineRenderer();
        }

        private void OnValidate()
        {
            if (_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();
            RefreshMaterial();
            UpdateLineRenderer();
        }

        private void Update()
        {
            if (splineCurrent == null) return;
            
            if (Application.isPlaying)
            {
                if (_material == null) RefreshMaterial();
                if (_material == null) return;

                float scrollSpeed = splineCurrent.Force * visualSpeedMultiplier;
                _scrollOffset += scrollSpeed * Time.deltaTime;
                
                if (_material.HasProperty("_BaseMap"))
                    _material.SetTextureOffset("_BaseMap", new Vector2(-_scrollOffset, 0));
                else
                    _material.mainTextureOffset = new Vector2(-_scrollOffset, 0);
            }
        }

        [ContextMenu("Update Line Renderer"), Button]
        public void UpdateLineRenderer()
        {
            if (splineCurrent == null) 
            {
                splineCurrent = GetComponentInParent<SplineCurrent>();
                if (splineCurrent == null) return;
            }
            
            var splineContainer = splineCurrent.GetComponent<SplineContainer>();
            if (splineContainer == null) return;

            var spline = splineContainer.Spline;
            if (spline == null) return;

            if (_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();

            float length = spline.GetLength();
            int resolution = Mathf.Max(2, Mathf.CeilToInt(length * pointsPerUnit));

            var offset = Vector3.up * yOffset;
            
            Vector3[] positions = new Vector3[resolution];
            for (int i = 0; i < resolution; i++)
            {
                float t = (float)i / (resolution - 1);
                positions[i] = splineContainer.transform.TransformPoint(spline.EvaluatePosition(t)) + offset;
            }

            _lineRenderer.positionCount = resolution;
            _lineRenderer.SetPositions(positions);
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.loop = spline.Closed;
            _lineRenderer.textureMode = LineTextureMode.Tile;
            
            if (useSplineWidth)
            {
                float width = splineCurrent.SplineWidth;
                _lineRenderer.startWidth = width;
                _lineRenderer.endWidth = width;
            }

            // Set tiling based on length to maintain aspect ratio
            // Assuming texture is square, we tile it 'length' times divided by width if we want it square?
            // Actually, usually we just want it to tile N times per world unit.
            if (_material != null)
            {
                float tiling = length / (_lineRenderer.startWidth > 0 ? _lineRenderer.startWidth : 1f);
                if (_material.HasProperty("_BaseMap"))
                    _material.SetTextureScale("_BaseMap", new Vector2(tiling, 1));
                else
                    _material.mainTextureScale = new Vector2(tiling, 1);
            }
        }
    }
}
