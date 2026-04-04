using UnityEngine;

namespace BoatGame
{
    public class TowRopeRenderer : MonoBehaviour
    {
        [SerializeField] private new LineRenderer renderer;

        public void UpdateRenderer(Rigidbody from, Rigidbody to)
        {
            if (to == null)
            {
                renderer.enabled = false;
                return;
            }

            renderer.SetPosition(0, from.position);
            renderer.SetPosition(1, to.position);
            renderer.enabled = true;
        }
    }
}
