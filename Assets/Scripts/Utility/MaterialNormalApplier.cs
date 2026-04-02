using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class MaterialNormalApplier : MonoBehaviour
    {
        private static readonly string NormalKeyword = "_NORMALMAP";
        private static readonly int NormalMap = Shader.PropertyToID("_BumpMap");
        private static readonly int NormalStrength = Shader.PropertyToID("_BumpScale");

        [SerializeField] private new Renderer renderer;
        [SerializeField, Range(0f, 1f)] private float normalStrength = 1f;
        [SerializeField] private Texture2D normalMap;

        private void Start()
        {
            Apply();
        }

        [Button]
        private void Apply()
        {
            var mpb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(mpb);
            mpb.SetTexture(NormalMap, normalMap);
            mpb.SetFloat(NormalStrength, normalStrength);
            renderer.SetPropertyBlock(mpb);
            renderer.sharedMaterial.EnableKeyword(NormalKeyword);
        }
    }
}
