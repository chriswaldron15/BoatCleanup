using UnityEngine;

namespace BoatGame
{
    public class Towable : MonoBehaviour
    {
        [field: SerializeField] public Transform AttachPoint { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

        public float Score()
        {
            return Vector3.Distance(AttachPoint.position, PlayerBoat.Instance.Transform.position);
        }
    }
}
