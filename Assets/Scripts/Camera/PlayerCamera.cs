using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class PlayerCamera : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Instance = null;
        }
        
        public static PlayerCamera Instance { get; private set; }
        
        [field: SerializeField] public Camera Camera { get; private set; }
        
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float followOffset;
        [SerializeField] private float followDistance;
        [SerializeField] private float lerpSpeed;

        private CameraLookAtPoint _lookAtOverride;
        private float _startOrtho;
        private float _targetOrtho;

        private void Awake()
        {
            Instance = this;
            PlayerBoat.OnBoatTeleported += OnBoatTeleported;

            _startOrtho = Camera.orthographicSize;
            _targetOrtho = _startOrtho;
        }

        private void OnBoatTeleported(bool immediateCamera)
        {
            UpdatePosition(immediateCamera);
        }

        private void Start()
        {
            UpdatePosition(true);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        private void LateUpdate()
        {
            UpdatePosition(false);
        }

        private void UpdatePosition(bool immediate)
        {
            bool hasOverride = _lookAtOverride != null;
            Vector3 lookAt;

            if (hasOverride)
                lookAt = Vector3.Lerp(_lookAtOverride.Target.position, PlayerBoat.Instance.Transform.position, _lookAtOverride.PlayerPositionInfluence);
            else
                lookAt = PlayerBoat.Instance.Transform.position;
            
            var target = lookAt - cameraTransform.forward * followDistance + cameraTransform.up * followOffset;

            Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, _targetOrtho, Time.deltaTime * lerpSpeed);

            if (immediate)
                cameraTransform.position = target;
            else
                cameraTransform.position =
                    Vector3.Slerp(cameraTransform.position, target, Time.deltaTime * lerpSpeed);
        }

        [Button]
        private void UpdateNow()
        {
            var playerBoat = FindFirstObjectByType<PlayerBoat>();
            
            if (playerBoat)
                cameraTransform.position = playerBoat.transform.position - cameraTransform.forward * followDistance;
            else cameraTransform.position = Vector3.zero - cameraTransform.forward * followDistance;
        }

        public void SetLookAtOverride(CameraLookAtPoint point)
        {
            _lookAtOverride = point;
            _targetOrtho = point.OrthoSize;
        }

        public void ClearLookAtOverride(CameraLookAtPoint cameraLookAtPoint)
        {
            if (_lookAtOverride == cameraLookAtPoint)
            {
                _lookAtOverride = null;
                _targetOrtho = _startOrtho;
            }
        }

        [Button]
        private void AlignSceneCamera()
        {
#if UNITY_EDITOR
            UnityEditor.SceneView.lastActiveSceneView.pivot = transform.position;
            UnityEditor.SceneView.lastActiveSceneView.rotation = transform.rotation;
#endif
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Face Camera")]
        private static void FaceCamera()
        {
            if (UnityEditor.Selection.activeGameObject == null)
                return;

            var cam = GameObject.FindFirstObjectByType<PlayerCamera>(FindObjectsInactive.Include);

            if (cam)
            {
                UnityEditor.Undo.RecordObject(UnityEditor.Selection.activeGameObject, "Face camera");
                UnityEditor.Selection.activeGameObject.transform.rotation = cam.transform.rotation;
            }
        }
#endif
    }
}
