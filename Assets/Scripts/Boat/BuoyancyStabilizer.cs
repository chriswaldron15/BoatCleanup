using UnityEngine;

namespace BoatGame
{
    public class BuoyancyStabilizer : MonoBehaviour
    {
        [SerializeField] private float uprightStrength = 50f;
        [SerializeField] private Rigidbody target;

        void FixedUpdate()
        {
            Vector3 currentUp = transform.up;
            Vector3 targetUp = Vector3.up;

            Vector3 torque = Vector3.Cross(currentUp, targetUp);

            target.AddTorque(torque * uprightStrength, ForceMode.Acceleration);
        }
    }
}
