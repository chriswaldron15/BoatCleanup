using UnityEngine;

namespace BoatGame
{
    public class CollisionIgnore : MonoBehaviour
    {
        [SerializeField] private Collider target;
        [SerializeField] private Collider[] collidersToIgnore;

        private void Awake()
        {
            for (int i = 0, iMax = collidersToIgnore.Length; i < iMax; i++)
            {
                Physics.IgnoreCollision(target, collidersToIgnore[i]);
            }
        }
    }
}
