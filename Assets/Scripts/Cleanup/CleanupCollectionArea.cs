using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class CleanupCollectionArea : MonoBehaviour
    {
        [SerializeField, Tag] private string cleanupTag;
        [SerializeField] private int requiredAmount;

        private int _collectedAmount;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(cleanupTag))
                return;

            _collectedAmount++;
            Destroy(other.gameObject);

            if (_collectedAmount >= requiredAmount)
            {
                Debug.LogError("Collected everything!");
            }
            else
            {
                Debug.LogError($"{_collectedAmount}/{requiredAmount} collected");
            }
        }
    }
}
