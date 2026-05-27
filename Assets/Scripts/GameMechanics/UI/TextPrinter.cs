using System;
using Core;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using GameMechanics.UI;
using TMPro;
using UnityEngine;

public class TextPrinter
{
    private const float CHAR_PRINT_INTERVAL = 0.025f;

    private EventInstance _typewriterInstance;
    private bool _instanceInitialized = false;

    public bool IsPrinting { get; private set; }

    private int _charCount;
    private Tween _printTween;
    private Sequence _sequence;

    public event Action OnSinglePrintFinished;
    public event Action OnChainFinished;

    public void Print(TMP_Text displayTarget, string text)
    {
        _printTween?.Kill();
        IsPrinting = true;

        displayTarget.enabled = true;
        displayTarget.SetText(text);

        displayTarget.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);

        _charCount = displayTarget.textInfo.characterCount;

        if (_charCount <= 0)
        {
            displayTarget.maxVisibleCharacters = int.MaxValue;
            IsPrinting = false;
            _charCount = 0;
            OnSinglePrintFinished?.Invoke();
            return;
        }

        displayTarget.maxVisibleCharacters = 0;

        float totalTime = Mathf.Max(CHAR_PRINT_INTERVAL * _charCount, 0.0001f);
        int visible = 0;

        _printTween = DOTween.To(
                () => visible,
                x => {
                    visible = x;
                    displayTarget.maxVisibleCharacters = visible;
                },
                endValue: _charCount,
                duration: totalTime
            )
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                IsPrinting = false;
                _charCount = 0;
                OnSinglePrintFinished?.Invoke();
            });
    }

    public void ForcePrintEnd(TMP_Text displayTarget)
    {
        _printTween?.Kill();
        displayTarget.maxVisibleCharacters = int.MaxValue;
        IsPrinting = false;
        _charCount = 0;
    }

    public TextPrinter BeginChain()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence().SetAutoKill(true);
        IsPrinting = true;
        return this;
    }

    public TextPrinter ThenPrint(TMP_Text target, string text, DaySummaryHandler daySummaryHandler)
    {
        if (!_instanceInitialized)
        {
            _instanceInitialized = true;
            _typewriterInstance = RuntimeManager.CreateInstance(FMODEvents.Instance.dayEndTypewriter);
        }

        _typewriterInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _typewriterInstance.start();

        _sequence ??= DOTween.Sequence().SetAutoKill(true);

        target.enabled = true;
        target.SetText(text);
        target.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);

        int charCount = target.textInfo.characterCount;
        target.maxVisibleCharacters = 0;
        int visible = 0;

        float duration = Mathf.Max(CHAR_PRINT_INTERVAL * Mathf.Max(charCount, 0), 0.0001f);

        _sequence.Append(
            DOTween.To(
                    () => visible,
                    x => {
                        visible = x;
                        target.maxVisibleCharacters = visible;
                    },
                    endValue: charCount,
                    duration: duration
                )
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    OnSinglePrintFinished?.Invoke();
                })
        );

        return this;
    }

    public TextPrinter OnChainComplete(Action action)
    {
        if (_sequence == null)
            _sequence = DOTween.Sequence().SetAutoKill(true);

        _sequence.OnComplete(() =>
        {
            IsPrinting = false;
            _typewriterInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            action?.Invoke();
            OnChainFinished?.Invoke();
        });
        return this;
    }

    public Tween PlayChain()
    {
        return _sequence?.Play();
    }
}