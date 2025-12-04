using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BlinkPanelUI : MonoBehaviour
{
    [SerializeField] private float effectDuration;
    [SerializeField] private Image panelImage;
    private Awaitable _fadeInOut;

    private void Start()
    {
        _fadeInOut = FadeIn();
    }

    private async Awaitable FadeIn()
    {
        float currentAlpha = 0;
        float hop = effectDuration / 10;
        while (currentAlpha < effectDuration)
        {
            Debug.Log("Fade In:" + currentAlpha);
            await Awaitable.WaitForSecondsAsync(hop);
            currentAlpha += hop/effectDuration;
            panelImage.color = new Color(0, 0, 0, currentAlpha);
        }
        
        _fadeInOut = FadeOut();
    }

    private async Awaitable FadeOut()
    {
        float currentAlpha = 0;
        float hop = effectDuration / 10;
        while (currentAlpha < effectDuration)
        {
            Debug.Log("Fade In:" + currentAlpha);
            await Awaitable.WaitForSecondsAsync(hop);
            currentAlpha += hop/effectDuration;
            panelImage.color = new Color(0, 0, 0, 1 - currentAlpha);
        }
        
        _fadeInOut = null;
    }
}
