using System;
using Player;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class GameManager : MonoBehaviour, IRegister
    {
        [SerializeField] private GameObject mainMenuCanvas;
        private MainMenuManager _mainMenuManager;
    
        private InputAction _pauseInput;
    
        public GameState CurrentGameState { get; set; }

        private void Awake()
        {
            Register();
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
}