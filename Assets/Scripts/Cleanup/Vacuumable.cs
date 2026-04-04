using DG.Tweening;
using UnityEngine;

namespace BoatGame
{
    public class Vacuumable : MonoBehaviour
    {
        [SerializeField] private float scaleTime;
        [SerializeField] private Ease scaleEase;
        [SerializeField] private Collider trigger;
        
        private bool _hasBeenVacuumed;
        
        public void OnVacuumed()
        {
            if (_hasBeenVacuumed)
                return;

            _hasBeenVacuumed = true;
            trigger.enabled = false;
            
            transform.DOScale(Vector3.zero, scaleTime).SetEase(scaleEase);
        }
    }
}
