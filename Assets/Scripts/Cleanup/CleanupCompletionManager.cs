using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace BoatGame
{
    public class CleanupCompletionManager : MonoBehaviour
    {
        public List<WaterJetable> targets = new();
        public UnityEvent onAllComplete;

        [Header("UI PopUp")]
        public ProgressPopUp popUpPrefab;

        private int _completedCount = 0;
        private HashSet<WaterJetable> _completedTargets = new();

        private void Start()
        {
            foreach (var target in targets)
            {
                if (target != null)
                {
                    target.onComplete.AddListener(() => OnTargetComplete(target));
                    
                    if (target.IsComplete())
                        OnTargetComplete(target);
                }
            }
        }

        private void OnTargetComplete(WaterJetable target)
        {
            if (!_completedTargets.Add(target))
                return;

            _completedCount++;

            SpawnPopUp(target.transform.position);

            if (_completedCount >= targets.Count && targets.Count > 0)
                onAllComplete?.Invoke();
        }

        private void SpawnPopUp(Vector3 position)
        {
            if (popUpPrefab == null) return;

            ProgressPopUp popup = Instantiate(popUpPrefab, position + Vector3.up * 1f, Quaternion.identity);
            popup.Setup($"{_completedCount}/{targets.Count}");
        }
    }
}
