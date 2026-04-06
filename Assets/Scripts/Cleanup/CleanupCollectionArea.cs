using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace BoatGame
{
    public class CleanupCollectionArea : MonoBehaviour
    {
        [SerializeField, Tag] private string cleanupTag;
        [SerializeField] private int requiredAmount;
        [SerializeField] private GameObject[] enabledOnCollect;
        [SerializeField] private TMP_Text amountRemainingText;
        [SerializeField] private Transform uiParent;
        
        [Header("Icon")]
        [SerializeField] private Transform icon;
        [SerializeField] private float rotationSpeed;
        
        [Header("Completion")]
        [SerializeField] private GameObject[] disableOnComplete;
        [SerializeField] private UnityEvent onComplete;
        [SerializeField] private float tweenTime;
        [SerializeField] private Ease finishEase;

        private int _collectedAmount;

        private void Awake()
        {
            UpdateText();
        }

        private void LateUpdate()
        {
            var pos = uiParent.transform.position;
            pos.y = 0f;

            var camPos = PlayerCamera.Instance.Camera.transform.position;
            camPos.y = 0f;
            
            uiParent.transform.forward = pos - camPos;
            
            icon.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_collectedAmount >= requiredAmount)
                return;
            
            if (!other.CompareTag(cleanupTag))
                return;
            
            if (_collectedAmount < enabledOnCollect.Length)
                enabledOnCollect[_collectedAmount].SetActive(true);

            _collectedAmount++;
            UpdateText();
            Destroy(other.gameObject);

            if (_collectedAmount >= requiredAmount)
            {
                Finish();
            }
        }

        private void Finish()
        {
            enabled = false;
                
            for (int i = 0, iMax = disableOnComplete.Length; i < iMax; i++)
                disableOnComplete[i].SetActive(false);
                
            onComplete?.Invoke();

            transform.DOScale(Vector3.zero, tweenTime).SetEase(finishEase);
        }

        private void UpdateText()
        {
            amountRemainingText.text = $"x{requiredAmount - _collectedAmount}";
        }
    }
}
