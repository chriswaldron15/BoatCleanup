using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class Current : MonoBehaviour
    {
        private const float SpeedParticleMultiplier = 0.02f;
        
        [SerializeField] private float currentStrength;
        [SerializeField] private BoxCollider boxCollider;
        
        [Header("Particles")]
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private float particleCountPerXWorldUnit;
        [SerializeField] private float strengthSpeedMultiplier = 1f;

        private Vector3 _worldSpaceCurrentStrength;
        
        private readonly List<Rigidbody> _affectedObjects = new(32);

        private void Awake()
        {
            _worldSpaceCurrentStrength = transform.forward * currentStrength;
            AlignParticles();
        }

        private void FixedUpdate()
        {
            for (int i = 0, iMax = _affectedObjects.Count; i < iMax; i++)
            {
                _affectedObjects[i].AddForce(_worldSpaceCurrentStrength, ForceMode.Acceleration);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _affectedObjects.Add(other.attachedRigidbody);
        }

        private void OnTriggerExit(Collider other)
        {
            _affectedObjects.Remove(other.attachedRigidbody);
        }

        [Button]
        private void AlignParticles()
        {
            var boxSize = boxCollider.size;
            
            var shape = particles.shape;
            shape.scale = new Vector3(boxSize.x / 2f, 1, 1);

            var emi = particles.emission;
            var rate = emi.rateOverTime; 
            rate.constant = particleCountPerXWorldUnit * boxSize.x * (currentStrength * SpeedParticleMultiplier);
            emi.rateOverTime = rate;

            var main = particles.main;

            var speed = main.startSpeed;
            speed.constant = currentStrength * strengthSpeedMultiplier;
            main.startSpeed = speed;
            
            var life = main.startLifetime;
            life.constant = boxSize.z / main.startSpeed.constant;
            main.startLifetime = life;

            var center = boxCollider.center;
            boxCollider.center = new Vector3(boxSize.x / boxSize.x, center.y, boxSize.z / 2f);
            
            particles.Play();
        }
    }
}
