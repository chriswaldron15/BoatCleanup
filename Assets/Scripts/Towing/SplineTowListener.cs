using UnityEngine;
using UnityEngine.Splines;

namespace BoatGame
{
    public class SplineTowListener : MonoBehaviour
    {
        [SerializeField] private StaticTowable towable;
        [SerializeField] private Transform towedObject;
        [SerializeField] private SplineContainer spline;
        [SerializeField] private AnimationCurve animationCurve;

        private void Awake()
        {
            towable.onCompleted.AddListener(OnTowCompleted);
            towable.towTick.AddListener(OnTowTick);
            
            UpdatePosition(0f);
        }

        private void OnTowTick(float towAmount)
        {
            UpdatePosition(towAmount);
        }

        private void OnTowCompleted()
        {
            UpdatePosition(1f);
            towable.onCompleted.RemoveListener(OnTowCompleted);
            towable.towTick.RemoveListener(OnTowTick);
        }

        private void UpdatePosition(float f)
        {
            var animatedPosition = animationCurve.Evaluate(f);
            towedObject.position = spline.EvaluatePosition(animatedPosition);
        }
    }
}
