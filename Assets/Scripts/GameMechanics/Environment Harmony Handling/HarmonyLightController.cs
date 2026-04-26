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

        [SerializeField] private Renderer lightbulbRenderer;

        [ColorUsage(true, true)]
        [SerializeField] private Color activeEmissionColor = Color.yellow;

        [ColorUsage(true, true)]
        [SerializeField] private Color inactiveEmissionColor = new Color(1f, 0.5f, 0.1f);

        [SerializeField] private float activeEmissionIntensity = 15f;
        [SerializeField] private float inactiveEmissionIntensity = 0.5f;

        private Light _light;
        private Sequence _sequence;

        private float _initialIntensity;
        private float _lowestIntensity;

        private MaterialPropertyBlock _mpb;

        private static readonly int EmissiveColorID = Shader.PropertyToID("_EmissiveColor");

        private const float LOWEST_INTENSITY_MODIFIER = 0.1f;

        private void Awake()
        {
            _light = GetComponent<Light>();

            _mpb = new MaterialPropertyBlock();
            _initialIntensity = _light.intensity;
            _lowestIntensity = _initialIntensity * LOWEST_INTENSITY_MODIFIER;

            ApplyEmission(activeEmissionColor, activeEmissionIntensity);
        }

        public Sequence PlaySingleFlicker()
        {
            _sequence?.Kill();

            _sequence = DOTween.Sequence();

            _sequence.Append(
                DOTween.Sequence()
                    .Join(_light.DOIntensity(_lowestIntensity, tweenTime))
                    .Join(DOEmission(activeEmissionColor, activeEmissionIntensity,
                                     inactiveEmissionColor, inactiveEmissionIntensity,
                                     tweenTime))
            );

            _sequence.AppendInterval(pauseAtMin);

            _sequence.Append(
                DOTween.Sequence()
                    .Join(_light.DOIntensity(_initialIntensity, tweenTime))
                    .Join(DOEmission(inactiveEmissionColor, inactiveEmissionIntensity,
                                     activeEmissionColor, activeEmissionIntensity,
                                     tweenTime))
            );

            _sequence.AppendInterval(pauseAtMax);

            return _sequence;
        }

        private Tween DOEmission(Color fromColor, float fromIntensity, Color toColor, float toIntensity, float duration)
        {
            float t = 0f;

            return DOTween.To(() => t, value =>
            {
                t = value;

                Color color = Color.Lerp(fromColor, toColor, t);
                float intensity = Mathf.Lerp(fromIntensity, toIntensity, t);

                ApplyEmission(color, intensity);

            }, 1f, duration);
        }

        private void ApplyEmission(Color baseColor, float intensity)
        {
            Color finalEmission = baseColor * intensity;

            lightbulbRenderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(EmissiveColorID, finalEmission);
            lightbulbRenderer.SetPropertyBlock(_mpb);
        }

        public void StopFlicker()
        {
            _sequence?.Kill();
            _light.intensity = _initialIntensity;
            ApplyEmission(activeEmissionColor, activeEmissionIntensity);
        }

        private void OnDisable()
        {
            _sequence?.Kill();
            _light.intensity = _initialIntensity;
            ApplyEmission(activeEmissionColor, activeEmissionIntensity);
        }
    }
}