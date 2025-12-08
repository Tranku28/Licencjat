using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// TODO: Set blinking for loading a new day
public class BlinkPanelUI : MonoBehaviour
{
    [SerializeField] private float effectDuration;
    [SerializeField] private Image panelImage;
    private Awaitable _fadeInOut;

    public void ClosePlayerEyes()
    {
        _fadeInOut = FadeIn();
    }

    public void OpenPlayerEyes()
    {
        _fadeInOut = FadeOut();
    }

    private async Awaitable FadeIn()
    {
        float currentAlpha = 0;
        float hop = effectDuration / 10;
        while (currentAlpha <= 1)
        {
            await Awaitable.WaitForSecondsAsync(hop);
            currentAlpha += hop/effectDuration;
            panelImage.color = new Color(0, 0, 0, currentAlpha);
        }

        _fadeInOut = null;
    }

    private async Awaitable FadeOut()
    {
        float currentAlpha = 1;
        float hop = effectDuration / 10;
        while (currentAlpha >= 0)
        {
            await Awaitable.WaitForSecondsAsync(hop);
            currentAlpha -= hop/effectDuration;
            panelImage.color = new Color(0, 0, 0, currentAlpha);
        }
        
        _fadeInOut = null;
    }

    private void OnDestroy() => _fadeInOut = null;
}
