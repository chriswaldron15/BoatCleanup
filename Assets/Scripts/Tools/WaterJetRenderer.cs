using UnityEngine;

namespace BoatGame
{
    public class WaterJetRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private ToolTarget reticule;
        [SerializeField] private ParticleSystem particles;

        private void Awake()
        {
            lineRenderer.enabled = false;
            
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
            reticule.PositionAndShow(bestTarget.transform.position);
        }

        public void StopAiming()
        {
            reticule.Hide();
        }
    }
}
