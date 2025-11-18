using Core;
using Core.Scriptable_Objects;
using GameMechanics.UI;
using Interactions;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameMechanics
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private MainMenuController mainMenuController;
        [SerializeField] private PauseMenuController pauseMenu;
        [SerializeField] private DialogueManager dialogueTab;
        [SerializeField] private DiaryController diaryTab;
        
        private UIElement _currentElement;
        private InputAction _currentAction;
        
        private GameStateMachine _gameStateMachine;

        private void Start()
        {
            _gameStateMachine = DependencyResoler.Instance.GetType<GameStateMachine>();
        }

        private void OnEnable()
        {
            PlayerControls.OnEscapePressedEvent += OnEscapePressed;

            PauseMenuController.OnResume += OnResume;
            
            Passenger.OnPassengerInteracted += OnPassengerInteracted;
            Diary.OnDiaryInteracted += OnDiaryInteracted;
            
            MainMenuController.PlayButtonPressed += QuitMenu;
        }

        private void OnDisable()
        {
            PlayerControls.OnEscapePressedEvent -= OnEscapePressed;
            
            PauseMenuController.OnResume -= OnResume;

            Passenger.OnPassengerInteracted -= OnPassengerInteracted;
            Diary.OnDiaryInteracted -= OnDiaryInteracted;
            
            MainMenuController.PlayButtonPressed -= QuitMenu;
        }

        private void OnPassengerInteracted(PassengerData obj)
        {
            if (_gameStateMachine.GetGameState() == GameState.Paused) return;
            
            if (_gameStateMachine.GetGameState() == GameState.UIOpened)
            {
                dialogueTab.SetVisualVisibility(false);
                _currentElement = null;
                _gameStateMachine.ChangeGameState(GameState.Gameplay);
                
                return;
            }
            
            dialogueTab.SetVisualVisibility(true);
            _currentElement = dialogueTab;
            _gameStateMachine.ChangeGameState(GameState.UIOpened);
        }
        
        private void QuitMenu()
        {
            mainMenuController.SetVisualVisibility(false);
        }
        
        private void OnResume()
        {
            _currentElement = null;
        }

        private void OnDiaryInteracted()
        {
            if (!ReferenceEquals(_currentElement, null)) return;
            
            _currentElement = diaryTab;
            diaryTab.SetVisualVisibility(true);
            _gameStateMachine.ChangeGameState(GameState.UIOpened);
        }
        
        private void OnEscapePressed()
        {
            Debug.Log("Escape pressed");
            if (_currentElement == null && _gameStateMachine.GetGameState() == GameState.Gameplay)
            {
                pauseMenu.SetVisualVisibility(true);
                _currentElement = pauseMenu;
                _gameStateMachine.ChangeGameState(GameState.Paused);
                return;
            }

            if (_currentElement == pauseMenu)
            {
                pauseMenu.SetVisualVisibility(false);
                _currentElement = null;
                _gameStateMachine.ChangeGameState(GameState.Gameplay);
                return;
            }

            if (!_currentElement) return;
            
            _currentElement.SetVisualVisibility(false);
            _currentElement = null;
            
            _gameStateMachine.ChangeGameState(GameState.Gameplay);
        }
    }
}
