using UnityEngine;

namespace BoatGame
{
    public class BoatSlowdownController : MonoBehaviour
    {
        private const string SlowdownTag = "Slowdown";
        
        [SerializeField] private BoatThrottle boatThrottle;
        [SerializeField] private float overlapRadius;
        [SerializeField] private LayerMask overlapLayerMask;
        
        private readonly Collider[] results = new Collider[8];

        private void FixedUpdate()
        {
            var hitCount = Physics.OverlapSphereNonAlloc(transform.position, overlapRadius, results, overlapLayerMask, QueryTriggerInteraction.Collide);
            var lowestSpeedMultiplier = 1f;

            for (int i = 0, iMax = hitCount; i < iMax; i++)
            {
                if (!results[i].CompareTag(SlowdownTag))
                    continue;
                
                if (!results[i].TryGetComponent(out SlowdownDebris debris))
                    continue;
                
                lowestSpeedMultiplier = Mathf.Min(lowestSpeedMultiplier, debris.speedModifier);
            }
            
            boatThrottle.SpeedMultiplier = lowestSpeedMultiplier;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, overlapRadius);
        }
    }
}
