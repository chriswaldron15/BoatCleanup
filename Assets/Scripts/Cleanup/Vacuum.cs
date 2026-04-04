using UnityEngine;

namespace BoatGame
{
    public class Vacuum : MonoBehaviour
    {
        private const string VacuumTag = "VacuumCollectable";
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(VacuumTag))
                return;

            if (!other.TryGetComponent(out Vacuumable vacuumable))
                return;
            
            vacuumable.OnVacuumed();
        }
    }
}
