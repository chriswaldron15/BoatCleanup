using UnityEngine;

namespace BoatGame
{
    public class Vacuum : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Vacuumable vacuumable))
                return;
            
            vacuumable.OnVacuumed();
        }
    }
}
