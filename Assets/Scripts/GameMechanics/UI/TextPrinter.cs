using DG.Tweening;
using TMPro;

public class TextPrinter
{
    private const float CHAR_PRINT_INTERVAL = 0.05f;
    public bool IsPrinting
    {
        get;
        private set;
    }

    private int _charCount;
    private Tween _printTween;

    public void Print(TMP_Text displayTarget, string text)
    {
        IsPrinting = true;

        displayTarget.SetText(text);
        displayTarget.ForceMeshUpdate();
        displayTarget.maxVisibleCharacters = 0;

        _charCount = displayTarget.textInfo.characterCount;
        float totalTime = CHAR_PRINT_INTERVAL * _charCount;
        int visible = 0;

        _printTween = DOTween.To(() => visible, x =>
        {
            visible = x;
            displayTarget.maxVisibleCharacters = visible;

        }, endValue: _charCount, duration: totalTime)
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
        displayTarget.maxVisibleCharacters = _charCount;
        IsPrinting = false;
        _charCount = 0;
    }
}
