using UnityEngine;
using TMPro;
using DG.Tweening;

namespace BoatGame
{
    public class ProgressPopUp : MonoBehaviour
    {
        public TMP_Text text;
        
        [Header("Animation Settings")]
[SerializeField] private float floatDistance = 1.5f;
        [SerializeField] private float duration = 2.5f;
        [SerializeField] private float fadeDelay = 1.5f;
        [SerializeField] private Ease moveEase = Ease.OutQuad;
        [SerializeField] private Ease scaleEase = Ease.OutBack;

        public void Setup(string message)
        {
            text.text = message;
            
            // Initial state
            transform.localScale = Vector3.zero;
            Color color = text.color;
            color.a = 1f;
            text.color = color;

            Sequence sequence = DOTween.Sequence();
            
            // Pop up and move
            sequence.Append(transform.DOScale(Vector3.one, 0.5f).SetEase(scaleEase));
            sequence.Join(transform.DOMove(transform.position + Vector3.up * floatDistance, duration).SetEase(moveEase));
            
            // Fade out after delay
            sequence.Insert(fadeDelay, text.DOFade(0f, duration - fadeDelay));
            
            sequence.OnComplete(() => Destroy(gameObject));
        }
    }
}
