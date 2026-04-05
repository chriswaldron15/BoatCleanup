using UnityEngine;

namespace BoatGame
{
    public class WaterJetRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private GameObject reticule;
        [SerializeField] private ParticleSystem particles;

        private void Awake()
        {
            lineRenderer.enabled = false;
            reticule.SetActive(false);
            
            reticule.transform.SetParent(null);
            particles.transform.SetParent(null);
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public void StopJet()
        {
            lineRenderer.enabled = false;
            particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        public void ShootAt(WaterJetable bestTarget)
        {
            var myPos = transform.position;
            var targetPos = bestTarget.transform.position;
            
            lineRenderer.SetPosition(0, myPos);
            lineRenderer.SetPosition(1, targetPos);
            lineRenderer.enabled = true;

            var particleTransform = particles.transform;
            particleTransform.position = targetPos;
            particleTransform.forward = myPos - targetPos;
            
            if (!particles.isPlaying)
                particles.Play();
        }

        public void AimAt(WaterJetable bestTarget)
        {
            var targetPosition = bestTarget.transform.position;
            var forward = (transform.position - targetPosition).normalized;
            
            reticule.transform.forward = forward;
            reticule.transform.position = targetPosition + forward * 0.5f;
            reticule.SetActive(true);
        }

        public void StopAiming()
        {
            reticule.SetActive(false);
        }
    }
}
