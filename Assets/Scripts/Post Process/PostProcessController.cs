using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PostProcessController : MonoBehaviour
{
    [Header("Overlay volume for rewind")]
    [SerializeField] private Volume rewindVolume;

    [Header("Animation")]
    [SerializeField] private AnimationCurve weightCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [SerializeField] private float fadeInTime = 0.2f;
    [SerializeField] private float fadeOutTime = 0.25f;

    [Header("Targets")]
    [SerializeField] private float targetChromatic = 0.6f;
    [SerializeField] private float targetGrain = 0.45f;
    [SerializeField] private float targetDistortion = 0.25f;
    [SerializeField] private float targetVignette = 0.35f;
    [SerializeField] private float targetMotionBlur = 0.15f;
    [SerializeField] private float targetSaturation = -80f;

    private VolumeProfile runtimeProfile;

    private ChromaticAberration chromatic;
    private FilmGrain grain;
    private LensDistortion distortion;
    private Vignette vignette;
    private MotionBlur motionBlur;
    private ColorAdjustments colorAdjustments;

    private Coroutine currentRoutine;

    private void Awake()
    {
        // runtime copy - nie ruszamy oryginalnego assetu
        runtimeProfile = Instantiate(rewindVolume.profile);
        rewindVolume.profile = runtimeProfile;

        GetOrAdd(ref chromatic);
        GetOrAdd(ref grain);
        GetOrAdd(ref distortion);
        GetOrAdd(ref vignette);
        GetOrAdd(ref motionBlur);
        GetOrAdd(ref colorAdjustments);

        ForceOverride(chromatic.intensity);
        ForceOverride(grain.intensity);
        ForceOverride(distortion.intensity);
        ForceOverride(vignette.intensity);
        ForceOverride(motionBlur.intensity);
        ForceOverride(colorAdjustments.saturation);

        SetNormalized(0f);
        rewindVolume.weight = 0f;

        //EnableRewind();
    }

    public void EnableRewind()
    {
        StartBlend(1f, fadeInTime);
    }

    public void DisableRewind()
    {
        StartBlend(0f, fadeOutTime);
    }

    /// <summary>
    /// Jeśli chcesz sterować ręcznie suwakiem 0..1 np. z innego systemu czasu.
    /// </summary>
    public void SetNormalized(float t)
    {
        t = Mathf.Clamp01(t);

        chromatic.intensity.value = Mathf.Lerp(0f, targetChromatic, t);
        grain.intensity.value = Mathf.Lerp(0f, targetGrain, t);

        // Możesz lekko “szarpać” distortion, np. przez sinusa
        distortion.intensity.value = Mathf.Lerp(0f, targetDistortion, t)
                                   + Mathf.Sin(Time.time * 18f) * 0.02f * t;

        vignette.intensity.value = Mathf.Lerp(0f, targetVignette, t);
        motionBlur.intensity.value = Mathf.Lerp(0f, targetMotionBlur, t);
        colorAdjustments.saturation.value = Mathf.Lerp(0f, targetSaturation, t);
    }

    private void StartBlend(float target, float duration)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(BlendRoutine(target, duration));
    }

    private System.Collections.IEnumerator BlendRoutine(float target, float duration)
    {
        float startWeight = rewindVolume.weight;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float curved = weightCurve.Evaluate(t);

            float weight = Mathf.Lerp(startWeight, target, curved);
            rewindVolume.weight = weight;
            SetNormalized(weight);

            yield return null;
        }

        rewindVolume.weight = target;
        SetNormalized(target);
        currentRoutine = null;

        //TODO: Improve logic
        yield return new WaitForSeconds(0.5f);

        DisableRewind();
    }

    private void GetOrAdd<T>(ref T component) where T : VolumeComponent
    {
        if (!runtimeProfile.TryGet(out component))
            component = runtimeProfile.Add<T>(false);
    }

    private void ForceOverride<T>(VolumeParameter<T> parameter)
    {
        parameter.overrideState = true;
    }
}