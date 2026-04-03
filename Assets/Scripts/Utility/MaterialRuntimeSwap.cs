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
    }
}
