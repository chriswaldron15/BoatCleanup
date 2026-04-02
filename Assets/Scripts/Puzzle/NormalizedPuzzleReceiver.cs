using UnityEngine;

namespace BoatGame
{
    public abstract class NormalizedPuzzleReceiver : MonoBehaviour
    {
        public abstract void SetStateImmediate(float normalizedCompletionValue);
        public abstract void UpdateState(float normalizedCompletionValue);
    }
}
