using System;
using Core;
using GameMechanics;
using GameMechanics.UI;
using GameMechanics.UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class CloseUIButton : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private StartGameBoardHandler startGameBoardHandler;
    private Button _button;
    private Image _buttonImage;
    private bool _startBoardMode = false;
    public event Action OnCloseUI;

    private void Awake()
    {
        uiManager.OnUIOpened += EnableCloseButton;
        uiManager.OnUIQuit += DisableCloseButton;

        startGameBoardHandler.OnStartBoardZoomed += StartBoardModeChange;
        startGameBoardHandler.InvokeCloseButtonDissapear += DisableStartBoardModeAndButton;

        DialogueManager.CloseButtonEnableEvent += EnableCloseButton;

        _button = GetComponent<Button>();
        _buttonImage = GetComponent<Image>();

        _buttonImage.enabled = false;
    }

    private void OnDestroy()
    {
        uiManager.OnUIOpened -= EnableCloseButton;
        uiManager.OnUIQuit -= DisableCloseButton;

        startGameBoardHandler.OnStartBoardZoomed -= StartBoardModeChange;
        startGameBoardHandler.InvokeCloseButtonDissapear -= DisableStartBoardModeAndButton;

        DialogueManager.CloseButtonEnableEvent -= EnableCloseButton;
    }

    private void OnEnable() => _button.onClick.AddListener(Close);

    private void OnDisable() => _button.onClick.RemoveListener(Close);

    private void EnableCloseButton()
    {
        _buttonImage.enabled = true;
    }

    private void DisableCloseButton()
    {
        _buttonImage.enabled = false;
    }
    
    private void Close()
    {
        if (_startBoardMode)
        {
            startGameBoardHandler.GoBackButtonClicked();
            StartBoardModeChange(false);
        }

        _buttonImage.enabled = false;
        uiManager.ForceEscape();
    }

    private void StartBoardModeChange(bool value)
    {
        _startBoardMode = value;
        
        EnableCloseButton();
    }

    private void DisableStartBoardModeAndButton()
    {
        _startBoardMode = false;
        _buttonImage.enabled = false;
    }
}
