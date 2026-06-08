
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
    public static Action OnSummaryDisplayFinalDay;
    private bool _finalNotesDisplayed;

    private void Awake()
    {
        _finalNotesDisplayed = false;
        GameStateMachine.OnMenuReturned += OnMenuReturned;
    }


    private void OnDestroy()
    {
        _current = null;
        GameStateMachine.OnMenuReturned -= OnMenuReturned;
    }

    private void OnMenuReturned()
    {
        _finalNotesDisplayed = false;
    }

    public void OnNextDayButtonClick()
    {
        GameSaveData currentSave = DependencyResolver.Instance.GetType<SaveSystem>().GetCurrentSave();

        if (currentSave.CurrentDay == 5 && !_finalNotesDisplayed)
        {
            _finalNotesDisplayed = true;
            OnSummaryDisplayFinalDay?.Invoke();
            return;
        }

        DependencyResolver.Instance.GetType<ApplicationGlobalSettings>().CursorActive(false);
        summaryPanel.SetActive(false);
        showDaySummary = false;
        OnNextDayButtonClicked?.Invoke();

        _finalNotesDisplayed = false;
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
            saveSystem.LoadSave(saveSystem.GetCurrentSave());
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
