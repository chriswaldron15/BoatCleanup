using UnityEngine;

namespace BoatGame
{
    public class SlowdownDebris : MonoBehaviour
    {
        [Range(0.1f, 1f)] public float speedModifier = 0.5f;
    }
}
