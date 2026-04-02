using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class VarietyScaler : MonoBehaviour
    {
        [SerializeField] private Vector3 startScale;
        [SerializeField] private Vector3 scaleVariance;

        [Button]
        private void RandomizeScale()
        {
            transform.localScale = new Vector3(
                startScale.x + Random.Range(-scaleVariance.x, scaleVariance.x),
                startScale.y + Random.Range(-scaleVariance.y, scaleVariance.y),
                startScale.z + Random.Range(-scaleVariance.z, scaleVariance.z)
            );
        }

        [Button]
        private void RandomizeAllInScene()
        {
            var all = FindObjectsByType<VarietyScaler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            for (int i = 0, iMax = all.Length; i < iMax; i++)
            {
                all[i].RandomizeScale();
            }
        }
    }
}
