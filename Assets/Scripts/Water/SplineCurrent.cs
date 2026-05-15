using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace BoatGame
{
    public class SplineCurrent : MonoBehaviour
    {
        [SerializeField] private float force;
        public float Force => force;
        [SerializeField] private SplineContainer spline;
        [SerializeField, Range(0, 10)] private float splineWidth;
        public float SplineWidth => splineWidth;
        [SerializeField] private int splineResolution = 1;
        
        private readonly HashSet<Rigidbody> _targets = new();
        private NativeSpline _nativeSpline;

        private void OnValidate()
        {
            if (spline == null) return;
            var sphere = GetComponent<SphereCollider>();
            if (sphere == null) return;

            if (spline.Spline.Count == 0)
                return;
            
            Bounds bounds = spline.Spline.GetBounds();
            bool first = true;
                sphere.center = bounds.center;
                sphere.radius = bounds.extents.magnitude + splineWidth;
                sphere.isTrigger = true;
        }

        private void Awake()
        {
            _nativeSpline = new NativeSpline(spline.Spline, Allocator.Persistent);
        }

        private void OnDestroy()
        {
            _nativeSpline.Dispose();
        }

        private void FixedUpdate()
        {
            var forceThisFrame = force * Time.fixedDeltaTime;
            var myPos = transform.position;
            var myPosf3 = new float3(myPos);

            foreach (var rb in _targets)
            {
                if (SplineUtility.GetNearestPoint(_nativeSpline, new float3(rb.position - myPos), out var nearest, out float t, splineResolution) < splineWidth && t > 0 && t < 0.99f)
                {
                    var nextPos = _nativeSpline.EvaluatePosition(t + 0.1f) + myPosf3;
                    var nearestWorld = nearest + myPosf3;
                    var dir = (Vector3)(nextPos - nearestWorld);
                    dir.Normalize();
                    rb.AddForce(dir * forceThisFrame, ForceMode.Acceleration);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _targets.Add(other.attachedRigidbody);
        }

        private void OnTriggerExit(Collider other)
        {
            _targets.Remove(other.attachedRigidbody);
        }
    }
}
