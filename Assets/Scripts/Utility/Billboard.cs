using UnityEngine;

namespace BoatGame
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private bool lockY = true;

        private void LateUpdate()
        {
            var pos = transform.position;
            var camPos = PlayerCamera.Instance.Camera.transform.position;

            if (lockY)
                pos.y = camPos.y = transform.position.y;

            if (pos == camPos)
                return;

            transform.forward = pos - camPos;
        }
    }
}
