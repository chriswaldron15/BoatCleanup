using UnityEngine;

namespace BoatGame
{
    public class WaterJetRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private GameObject reticule;

        private void Awake()
        {
            lineRenderer.enabled = false;
            reticule.SetActive(false);
            
            reticule.transform.SetParent(null);
        }

        public void StopJet()
        {
            lineRenderer.enabled = false;
        }

        public void ShootAt(WaterJetable bestTarget)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, bestTarget.transform.position);
            lineRenderer.enabled = true;
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
