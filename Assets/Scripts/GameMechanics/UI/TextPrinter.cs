using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextPrinter
{
    private const float CHAR_PRINT_INTERVAL = 0.025f;
    public bool IsPrinting { get; private set; }

    private int _charCount;
    private Tween _printTween;

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
            return;
        }

        displayTarget.maxVisibleCharacters = 0;

        float totalTime = Mathf.Max(CHAR_PRINT_INTERVAL * _charCount, 0.0001f); // minimalny czas > 0
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
        });
    }

    public void ForcePrintEnd(TMP_Text displayTarget)
    {
        _printTween?.Kill();
        displayTarget.maxVisibleCharacters = int.MaxValue;
        IsPrinting = false;
        _charCount = 0;
    }
}
