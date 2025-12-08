using System;
using UnityEngine;

namespace Core
{
    [InitializeSystem("GameStateMachine")]
    public class GameStateMachine : BaseSystem
    {
        GameState _gameState;

        protected override void Awake()
        {
            base.Awake();
            
            _gameState = GameState.MainMenu;
        }

        public static Action<GameState> OnGameStateChanged;
        public static Action OnGameStarted;

        public void StartGame()
        {
            ChangeGameState(GameState.Gameplay);
            OnGameStarted?.Invoke();
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