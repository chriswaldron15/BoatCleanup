using System;
using TMPro;
using UnityEngine;

namespace BoatGame
{
    [DefaultExecutionOrder(1000)]
    public class SceneTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Vector2 screenLocation;
        [SerializeField] private float depthOffset;

        private DateTime startTime;

        private void Start()
        {
            startTime = DateTime.Now;
        }

        private void LateUpdate()
        {
            text.text = $"{DateTime.Now - startTime:mm\\:ss}";
            var pos = PlayerCamera.Instance.Camera.ViewportToWorldPoint(new Vector3(screenLocation.x, screenLocation.y, PlayerCamera.Instance.Camera.nearClipPlane));
            var camFwd = PlayerCamera.Instance.Camera.transform.forward;
            pos += camFwd * depthOffset;
            transform.position = pos;
            transform.forward = camFwd;
        }
    }
}
