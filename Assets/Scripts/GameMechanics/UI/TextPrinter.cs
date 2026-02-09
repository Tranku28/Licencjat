using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextPrinter
{
    private const float CHAR_PRINT_INTERVAL = 0.025f;

    public bool IsPrinting { get; private set; }

    private int _charCount;
    private Tween _printTween;   // do pojedynczego Print
    private Sequence _sequence;  // do łańcucha

    public event Action OnSinglePrintFinished;
    public event Action OnChainFinished;

    // --- POJEDYNCZY PRINT (jak dotychczas), ale z wywołaniem eventu ---
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
            OnSinglePrintFinished?.Invoke(); // <-- ważne
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
                OnSinglePrintFinished?.Invoke(); // <-- ważne
            });
    }

    public void ForcePrintEnd(TMP_Text displayTarget)
    {
        _printTween?.Kill();
        displayTarget.maxVisibleCharacters = int.MaxValue;
        IsPrinting = false;
        _charCount = 0;
    }

    // --- FLUENT CHAIN ---

    /// <summary>Rozpoczyna nowy łańcuch wydruków.</summary>
    public TextPrinter BeginChain()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence().SetAutoKill(true);
        IsPrinting = true;
        return this;
    }

    /// <summary>Dodaje kolejny element do łańcucha.</summary>
    public TextPrinter ThenPrint(TMP_Text target, string text)
    {
        if (_sequence == null)
            _sequence = DOTween.Sequence().SetAutoKill(true);

        // Przygotowanie od razu (bez migania – tekst i tak będzie ukryty)
        target.enabled = true;
        target.SetText(text);
        target.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);

        int charCount = target.textInfo.characterCount;
        target.maxVisibleCharacters = 0;  // ukryj od razu
        int visible = 0;

        float duration = Mathf.Max(CHAR_PRINT_INTERVAL * Mathf.Max(charCount, 0), 0.0001f);

        // Dodajemy tween do sekwencji
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

    /// <summary>Callback po zakończeniu całego łańcucha.</summary>
    public TextPrinter OnChainComplete(Action action)
    {
        if (_sequence == null)
            _sequence = DOTween.Sequence().SetAutoKill(true);

        _sequence.OnComplete(() =>
        {
            IsPrinting = false;
            action?.Invoke();
            OnChainFinished?.Invoke();
        });
        return this;
    }

    /// <summary>Startuje łańcuch.</summary>
    public Tween PlayChain()
    {
        return _sequence?.Play();
    }
}