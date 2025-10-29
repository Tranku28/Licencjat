using System;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour, IRegister
{
    [SerializeField] private GameObject mainMenuCanvas;
    private MainMenuManager _mainMenuManager;
    
    private InputAction _pauseInput;

    public static Action<GameManager> OnPause;
    
    public GameState CurrentGameState { get; set; }

    private void Awake()
    {
        Register();
        
        _pauseInput = InputSystem.actions.FindAction("Pause");
        _pauseInput.performed += (_) =>
        {
            OnPause?.Invoke(this);
            Debug.Log("Pause");
        };
    }

    private void Start()
    {
        CurrentGameState = GameState.MainMenu;
        
        _mainMenuManager = Registry.Instance.Get<MainMenuManager>();
        if (!_mainMenuManager) throw new Exception("GameManager not initialized");
        
        _mainMenuManager.OnGameStarted += StartGame;
    }

    private void StartGame()
    {
        CurrentGameState = GameState.Game;
        mainMenuCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        _mainMenuManager.OnGameStarted -= StartGame;
        Unregister();
    }

    public void Register()
    {
        Registry.Instance.Register(this);
    }

    public void Unregister()
    {
        Registry.Instance.Unregister(this);
    }
    
}

public enum GameState
{
    MainMenu,
    Game,
    Paused
}
