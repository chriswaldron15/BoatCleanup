using UnityEngine;

namespace BoatGame
{
    public class ToolTarget : MonoBehaviour
    {
        [SerializeField] private Transform t;

        private void Awake()
        {
            t.SetParent(null);
            gameObject.SetActive(false);
        }

        public void PositionAndShow(Vector3 target)
        {
            var forward = (PlayerBoat.Instance.transform.position - target).normalized;
            t.forward = forward;
            t.position = target + forward * 0.5f;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
