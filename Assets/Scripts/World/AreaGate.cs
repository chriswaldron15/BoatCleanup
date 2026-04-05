using UnityEngine;

namespace BoatGame
{
    public class AreaGate : MonoBehaviour
    {
        [SerializeField] private GameObject lockedState, unlockedState;

        private void Awake()
        {
            lockedState.SetActive(true);
            unlockedState.SetActive(false);
        }

        public void Unlock()
        {
            lockedState.SetActive(false);
            unlockedState.SetActive(true);
        }
    }
}
