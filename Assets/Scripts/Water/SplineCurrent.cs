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
        [SerializeField] private SplineContainer spline;
        [SerializeField] private float splineWidth;
        [SerializeField] private int splineResolution = 1;
        
        private readonly HashSet<Rigidbody> _targets = new();
        private NativeSpline _nativeSpline;

        private void Awake()
        {
            _nativeSpline = new NativeSpline(spline.Spline, Allocator.Persistent);
        }

        private void FixedUpdate()
        {
            foreach (var rb in _targets)
            {
                var forceThisFrame = force * Time.fixedDeltaTime;
                
                if (SplineUtility.GetNearestPoint(_nativeSpline, new float3(rb.position), out var nearest, out float t, splineResolution, 1) < splineWidth)
                {
                    var nextPos = _nativeSpline.EvaluatePosition(t + 0.01f);
                    var dir = (Vector3)nextPos - rb.position;
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
