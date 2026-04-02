using UnityEngine;

namespace BoatGame
{
    [RequireComponent(typeof(Rigidbody))]
    public class WaterVolume : MonoBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform myTransform;
        
        public float WaterHeight { get; private set; }

        private void Awake()
        {
            RecalculateWaterHeight();
            //ExpandBoxToChildWaterPieces();
        }

        private void Update()
        {
            RecalculateWaterHeight();
        }

        private void RecalculateWaterHeight()
        {
            WaterHeight = boxCollider.bounds.max.y;
        }

        // CW: removed as we want these on the children instead for precise building
        // [Button]
        // private void ExpandBoxToChildWaterPieces()
        // {
        //     var bounds = new Bounds(Vector3.zero, Vector3.one);
        //
        //     foreach (var r in GetComponentsInChildren<Renderer>())
        //         bounds.Encapsulate(r.bounds);
        //
        //     var center = bounds.center;
        //     center -= transform.position;
        //     center.y -= 5f;
        //     boxCollider.center = center;
        //
        //     var size = bounds.size;
        //     size.y = 10f;
        //     boxCollider.size = size;
        // }
    }
}
