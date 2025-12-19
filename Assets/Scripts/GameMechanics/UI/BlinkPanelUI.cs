
using System;
using UnityEngine;
using UnityEngine.UI;

public class BlinkPanelUI : MonoBehaviour
{
    [SerializeField] private float effectDuration = 0.4f;
    [SerializeField] private Image panelImage;
    [SerializeField] private GameObject newspaperPanel;
    
    private Awaitable _current;

    public bool showNewspaper { get; set; }

    public event Action OnNextDayButtonClicked;
    
    private void OnDestroy()
    {
        _current = null;
    }

    public void OnNextDayButtonClick()
    {
        newspaperPanel.SetActive(false);
        showNewspaper = false;
        OnNextDayButtonClicked?.Invoke();
    }
    
    public Awaitable ClosePlayerEyes()
    {
        _current = FadeIn();
        
        return _current;
    }
    
    public Awaitable OpenPlayerEyes()
    {
        _current = FadeOut();
        return _current;
    }

    private async Awaitable FadeIn()
    {
        float currentAlpha = 0f;
        float hop = Mathf.Max(0.01f, effectDuration / 10f);

        while (currentAlpha < 1f)
        {
            await Awaitable.WaitForSecondsAsync(hop);
            currentAlpha += hop / effectDuration;
            panelImage.color = new Color(0f, 0f, 0f, Mathf.Clamp01(currentAlpha));
        }
        
        if (showNewspaper)
            newspaperPanel.SetActive(true);
        
        panelImage.color = new Color(0f, 0f, 0f, 1f);
        _current = null;
    }

    private async Awaitable FadeOut()
    {
        float currentAlpha = 1f;
        float hop = Mathf.Max(0.01f, effectDuration / 10f);

        while (currentAlpha > 0f)
        {
            await Awaitable.WaitForSecondsAsync(hop);
            currentAlpha -= hop / effectDuration;
            panelImage.color = new Color(0f, 0f, 0f, Mathf.Clamp01(currentAlpha));
        }

        panelImage.color = new Color(0f, 0f, 0f, 0f);
        _current = null;
    }
}
