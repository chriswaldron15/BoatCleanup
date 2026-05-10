using System;
using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class ImpulseApplier : MonoBehaviour
    {
        [SerializeField] private Vector3 worldDirection;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private float force;
        [SerializeField] private ForceMode mode;

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Fire()
        {
            rigidbody.AddForce(worldDirection * force, mode);
        }
        
        private void OnValidate()
        {
            worldDirection.Normalize();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, transform.position + worldDirection * 5f);
        }
    }
}
