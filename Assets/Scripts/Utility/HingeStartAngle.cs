using UnityEngine;

namespace BoatGame
{
    public class HingeStartAngle : MonoBehaviour
    {
        [SerializeField] private float angle;
        [SerializeField] private HingeJoint hinge;
        [SerializeField] private Rigidbody rb;

        private void FixedUpdate()
        {
            if (Mathf.Abs(hinge.angle - angle) > 0.1f)
            {
                rb.rotation = Quaternion.Euler(hinge.axis * angle);
            }
            else
            {
                enabled = false;
            }
        }
    }
}
