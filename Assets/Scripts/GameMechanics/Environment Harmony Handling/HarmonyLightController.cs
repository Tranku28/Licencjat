using DG.Tweening;
using UnityEngine;

namespace HarmonyHandling
{
    [RequireComponent(typeof(Light))]
    public class HarmonyLightController : MonoBehaviour
    {
        [SerializeField] private float tweenTime = 0.15f;
        [SerializeField] private float pauseAtMin = 0.05f;
        [SerializeField] private float pauseAtMax = 0.3f;
        [SerializeField] private Renderer lightbulbMaterial;
        [SerializeField] private Color activeEmissionColor;
        [SerializeField] private Color inactiveEmissionColor;
        [SerializeField] private float activeEmissionIntensity;
        [SerializeField] private float inactiveEmissionIntensity;

        private Light _light;
        private Sequence _sequence;

        private float _initialIntensity;
        private float _lowestIntensity;

        private static readonly int EmissiveColorID = Shader.PropertyToID("_EmissiveColor");
        private static readonly int EmissiveIntensityID = Shader.PropertyToID("_EmissiveIntensity");

        private const float LOWEST_INTENSITY_MODIFIER = 0.1f;

        private MaterialPropertyBlock _mpb;

        private void Awake()
        {
            _light = GetComponent<Light>();

            _mpb = new MaterialPropertyBlock();
            lightbulbMaterial.GetPropertyBlock(_mpb);

            _initialIntensity = _light.intensity;
            _lowestIntensity = _initialIntensity * LOWEST_INTENSITY_MODIFIER;
        }


        public Sequence PlaySingleFlicker()
        {
            _sequence?.Kill();
        
            _sequence = DOTween.Sequence();
        
            _sequence.Append(
                DOTween.Sequence()
                    .Join(_light.DOIntensity(_lowestIntensity, tweenTime))
                    .Join(DOEmissionColor(activeEmissionColor, inactiveEmissionColor, tweenTime))
                    .Join(DOEmissionIntensity(activeEmissionIntensity, inactiveEmissionIntensity, tweenTime))
            );

            _sequence.AppendInterval(pauseAtMin);

            _sequence.Append(
                DOTween.Sequence()
                    .Join(_light.DOIntensity(_initialIntensity, tweenTime))
                    .Join(DOEmissionColor(inactiveEmissionColor, activeEmissionColor, tweenTime))
                    .Join(DOEmissionIntensity(inactiveEmissionIntensity, activeEmissionIntensity, tweenTime))
            );
        
            _sequence.AppendInterval(pauseAtMax);
        
            return _sequence;
        }

        private Tween DOEmissionColor(Color from, Color to, float duration)
        {
            Color c = from;

            return DOTween.To(
                () => c,
                v =>
                {
                    c = v;
                    _mpb.SetColor(EmissiveColorID, c);
                    lightbulbMaterial.SetPropertyBlock(_mpb);
                },
                to,
                duration
            );
        }

        private Tween DOEmissionIntensity(float from, float to, float duration)
        {
            float i = from;

            return DOTween.To(
                () => i,
                v =>
                {
                    i = v;
                    _mpb.SetFloat(EmissiveIntensityID, i);
                    lightbulbMaterial.SetPropertyBlock(_mpb);
                },
                to,
                duration
            );
        }

        public void StopFlicker()
        {
            _sequence?.Kill();
            _light.intensity = _initialIntensity;
        }

        private void OnDisable()
        {
            _sequence?.Kill();
        }
    }
}
