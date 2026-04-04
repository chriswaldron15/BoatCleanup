using UnityEngine;

namespace BoatGame
{
    public abstract class Tool : MonoBehaviour
    {
        public abstract void OnActivate();
        public abstract void OnDeactivate();
    }
}
