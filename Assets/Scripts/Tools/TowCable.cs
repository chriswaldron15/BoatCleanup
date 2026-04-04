using UnityEngine;

namespace BoatGame
{
    public class TowCable : Tool
    {
        private const string TowableTag = "Towable";

        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private SpringJoint spring;
        [SerializeField] private TowRopeRenderer towRopeRenderer;
        
        private Rigidbody _towTarget;

        private void FixedUpdate()
        {
            towRopeRenderer.UpdateRenderer(rigidbody, spring.connectedBody);
        }

        private void LateUpdate()
        {
            towRopeRenderer.UpdateRenderer(rigidbody, spring.connectedBody);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(TowableTag))
                return;

            _towTarget = other.attachedRigidbody;
            spring.connectedBody = _towTarget;
            spring.connectedAnchor = Vector3.zero;
        }

        public override void OnActivate()
        {
            gameObject.SetActive(true);
        }

        public override void OnDeactivate()
        {
            if (_towTarget != null)
            {
                spring.connectedBody = null;
                _towTarget = null;
            }
            
            gameObject.SetActive(false);
        }
    }
}
