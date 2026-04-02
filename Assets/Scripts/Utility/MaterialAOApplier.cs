using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class MaterialAOApplier : MonoBehaviour
    {
        private static readonly string OcclusionKeyword = "_OCCLUSIONMAP";
        private static readonly int OcclusionMap = Shader.PropertyToID("_OcclusionMap");
        private static readonly int OcclusionStrength = Shader.PropertyToID("_OcclusionStrength");

        [SerializeField] private new Renderer renderer;
        [SerializeField, Range(0f, 1f)] private float aoStrength = 1f;
        [SerializeField] private Texture2D ambientOcclusionMap;

        private void Awake()
        {
            Apply();
        }

        [Button]
        private void Apply()
        {
            var mpb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(mpb);
            mpb.SetTexture(OcclusionMap, ambientOcclusionMap);
            mpb.SetFloat(OcclusionStrength, aoStrength);
            renderer.SetPropertyBlock(mpb);
            renderer.sharedMaterial.EnableKeyword(OcclusionKeyword);
        }
    }
}
