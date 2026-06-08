using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using Core;

public class PostProcessController : MonoBehaviour
{
    [SerializeField] private Volume gameplayVolume;
    [SerializeField] private Volume rewindVolume;
    [SerializeField] private Volume gameOverVolume;

    [SerializeField] private float blendDuration = 1f;
    [SerializeField] private float pauseDuration = 1f;
    [SerializeField] private Ease blendEase = Ease.InOutSine;


    private Tween blendTween;

    private void Awake()
    {
        gameplayVolume.weight = 1f;
        rewindVolume.weight = 0f;

        SouvenirEffectResolver.OnRewind += OnRewind;
        GameStateMachine.OnGameOver += OnGameOver;
        GameStateMachine.OnMenuReturned += OnMenuReturned;
    }

    private void OnDestroy()
    {
        SouvenirEffectResolver.OnRewind -= OnRewind;
        GameStateMachine.OnGameOver -= OnGameOver;
        GameStateMachine.OnMenuReturned -= OnMenuReturned;
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

    private void OnGameOver(){
        blendTween = DOTween.Sequence()
        .Append(gameOverVolume.DOWeight(1f, blendDuration))
        .Join(gameplayVolume.DOWeight(0f, blendDuration))
        .SetEase(blendEase);
    }

    private void OnMenuReturned()
    {
        gameplayVolume.weight = 1;
        gameOverVolume.weight = 0;
    }
}