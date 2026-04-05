using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace BoatGame
{
    public class CleanupCollectionArea : MonoBehaviour
    {
        [SerializeField, Tag] private string cleanupTag;
        [SerializeField] private int requiredAmount;
        [SerializeField] private GameObject[] enabledOnCollect;
        [SerializeField] private GameObject[] disableOnComplete;
        [SerializeField] private UnityEvent onComplete; 

        private int _collectedAmount;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(cleanupTag))
                return;
            
            if (_collectedAmount < enabledOnCollect.Length)
                enabledOnCollect[_collectedAmount].SetActive(true);

            _collectedAmount++;
            Destroy(other.gameObject);

            if (_collectedAmount >= requiredAmount)
            {
                enabled = false;
                
                for (int i = 0, iMax = disableOnComplete.Length; i < iMax; i++)
                    disableOnComplete[i].SetActive(false);
                
                onComplete?.Invoke();
            }
        }
    }
}
