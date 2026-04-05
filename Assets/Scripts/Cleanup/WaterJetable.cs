using UnityEngine;

namespace BoatGame
{
    public abstract class WaterJetable : MonoBehaviour
    {
        public float Score()
        {
            return Vector3.Distance(transform.position, PlayerBoat.Instance.Transform.position);
        }

        public abstract void OnJetted();
        public abstract bool IsComplete();
    }
}
