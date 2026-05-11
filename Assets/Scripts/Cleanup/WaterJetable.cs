using UnityEngine;
using UnityEngine.Events;

namespace BoatGame
{
    public abstract class WaterJetable : MonoBehaviour
    {
        public UnityEvent onComplete;

        public float Score()
{
            return Vector3.Distance(transform.position, PlayerBoat.Instance.Transform.position);
        }

        public abstract void OnJetted();
        public abstract bool IsComplete();
    }
}
