using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class MaterialRuntimeSwap : MonoBehaviour
    {
        [SerializeField] private new Renderer renderer;
        [SerializeField] private Material swapTo;

        private void Awake()
        {
            renderer.sharedMaterial = swapTo;
            Destroy(this);
        }

        #if UNITY_EDITOR
        [Button]
        private void SwapNow()
        {
            UnityEditor.Undo.RecordObject(gameObject, "Swapped normal water");
            renderer.sharedMaterial = swapTo;
        }
        #endif
    }
}
