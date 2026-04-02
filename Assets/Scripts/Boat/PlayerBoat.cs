using System;
using UnityEngine;

namespace BoatGame
{
    public class PlayerBoat : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Instance = null;
            OnBoatTeleported = null;
        }
        
        public static PlayerBoat Instance { get; private set; }
        public static Action<bool> OnBoatTeleported;
        
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Transform Transform { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public void SpawnAt(Transform spawnLocation, bool immediateCamera)
        {
            Rigidbody.linearVelocity = Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.position = spawnLocation.position;
            Rigidbody.rotation = spawnLocation.rotation;
            Rigidbody.PublishTransform();
            OnBoatTeleported?.Invoke(immediateCamera);
        }
    }
}
