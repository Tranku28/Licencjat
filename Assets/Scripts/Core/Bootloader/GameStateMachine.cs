using System;
using UnityEngine;

namespace Core
{
    [InitializeSystem("GameStateMachine")]
    public class GameStateMachine : BaseSystem
    {
        GameState _gameState;
        public static Action OnMenuReturned;

        protected override void Awake()
        {
            base.Awake();
            
            _gameState = GameState.MainMenu;
        }

        public static Action<GameState> OnGameStateChanged;
        public static Action<bool> OnGameStarted;

        public GameState GetCurrentState() => _gameState;

        public void StartGame(bool isNewGame)
        {
            if (isNewGame)
            {
                ChangeGameState(GameState.UIOpened);
                OnGameStarted?.Invoke(true);
                return;
            }

            ChangeGameState(GameState.Gameplay);
            OnGameStarted?.Invoke(false);
        }
        
        public void OnMainMenuReturn()
        {
            OnMenuReturned?.Invoke();
            ChangeGameState(GameState.MainMenu);
        }

        public GameState GetGameState() => _gameState;
        
        public void ChangeGameState(GameState gameState)
        {
            _gameState = gameState;
            
            OnGameStateChanged?.Invoke(_gameState);
        }
    }
    
    public enum GameState
    {
        MainMenu,
        Gameplay,
        Paused,
        UIOpened
    }
}