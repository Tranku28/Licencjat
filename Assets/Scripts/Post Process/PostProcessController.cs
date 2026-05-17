using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class PostProcessController : MonoBehaviour
{
    [SerializeField] private Volume gameplayVolume;
    [SerializeField] private Volume rewindVolume;

    [SerializeField] private float blendDuration = 1f;
    [SerializeField] private float pauseDuration = 1f;
    [SerializeField] private Ease blendEase = Ease.InOutSine;

    private Tween blendTween;

    private void Awake()
    {
        gameplayVolume.weight = 1f;
        rewindVolume.weight = 0f;

        SouvenirEffectResolver.OnRewind += OnRewind;
    }

    private void OnDestroy()
    {
        SouvenirEffectResolver.OnRewind -= OnRewind;
        blendTween?.Kill();
    }

    private void OnRewind()
    {
        blendTween = DOTween.Sequence()
        .Append(rewindVolume.DOWeight(1f, blendDuration * 0.7f))
        .Join(gameplayVolume.DOWeight(0f, blendDuration))
        .AppendInterval(pauseDuration)
        .Append(rewindVolume.DOWeight(0f, 0.5f))
        .Join(gameplayVolume.DOWeight(1f, 0.5f))
        .SetEase(blendEase);
    }
}