
using System;
using Core;
using Core.Save_System;
using GameMechanics.UI;
using UnityEngine;
using UnityEngine.UI;

public class BlinkPanelUI : MonoBehaviour
{
    [SerializeField] private float effectDuration = 0.4f;
    [SerializeField] private Image panelImage;
    [SerializeField] private GameObject summaryPanel;
    
    private Awaitable _current;

    public bool showDaySummary { get; set; }

    public event Action OnNextDayButtonClicked;
    public static Action OnSummaryDisplay;
    
    private void OnDestroy()
    {
        _current = null;
    }

    public void OnNextDayButtonClick()
    {
        summaryPanel.SetActive(false);
        showDaySummary = false;
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
        Debug.Log("Fade In");
        GameStateMachine gameStateMachine = DependencyResolver.Instance.GetType<GameStateMachine>();
        gameStateMachine.ChangeGameState(GameState.Paused);

        float currentAlpha = 0f;
        float hop = Mathf.Max(0.01f, effectDuration / 10f);

        while (currentAlpha < 1f)
        {
            await Awaitable.WaitForSecondsAsync(hop);
            currentAlpha += hop / effectDuration;
            panelImage.color = new Color(0f, 0f, 0f, Mathf.Clamp01(currentAlpha));
        }
        
        if (showDaySummary)
        {
            summaryPanel.SetActive(true);
            OnSummaryDisplay?.Invoke();
            
            SaveSystem saveSystem = DependencyResolver.Instance.GetType<SaveSystem>();
            saveSystem.SaveGame();
            int saveIndex = saveSystem.RuntimeSaveIndex;
            saveSystem.LoadSave(saveIndex);
        }

        
        panelImage.color = new Color(0f, 0f, 0f, 1f);
        _current = null;
    }

    private async Awaitable FadeOut()
    {
        Debug.Log("Fade Out");
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

        Debug.Log("Fade Out");
        GameStateMachine gameStateMachine = DependencyResolver.Instance.GetType<GameStateMachine>();
        gameStateMachine.ChangeGameState(GameState.Gameplay);
    }
}
