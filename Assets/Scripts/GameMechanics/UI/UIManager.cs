using Core;
using Core.Scriptable_Objects;
using GameMechanics.Interactions;
using GameMechanics.UI;
using Interactions;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace GameMechanics
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private MainMenuController mainMenuController;
        [SerializeField] private PauseMenuController pauseMenu;
        [SerializeField] private DialogueManager dialogueManager;
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
        }

        private void OnDisable()
        {
            PlayerControls.OnEscapePressedEvent -= OnEscapePressed;
            
            PauseMenuController.OnResume -= OnResume;

            Passenger.OnPassengerInteracted -= OnPassengerInteracted;
            Diary.OnDiaryInteracted -= OnDiaryInteracted;
        }

        private void OnPassengerInteracted(PassengerData obj)
        {
            if (_gameStateMachine.GetGameState() == GameState.Paused) return;
            
            if (_gameStateMachine.GetGameState() == GameState.UIOpened)
            {
                dialogueManager.SetVisualVisibility(false);
                _currentElement = null;
                _gameStateMachine.ChangeGameState(GameState.Gameplay);
                
                return;
            }
            
            dialogueManager.SetVisualVisibility(true);
            _currentElement = dialogueManager;
            _gameStateMachine.ChangeGameState(GameState.UIOpened);
        }
        
        private void OnResume()
        {
            _currentElement = null;
        }

        private void OnDiaryInteracted()
        {
            Debug.Log("Diary interacted");
            if (!ReferenceEquals(_currentElement, null)) return;
            
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.diaryOpenSound, transform.position);
            _currentElement = diaryTab;
            diaryTab.SetVisualVisibility(true);
            _gameStateMachine.ChangeGameState(GameState.UIOpened);
        }
        
        private void OnEscapePressed()
        {
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

            if (_currentElement == dialogueManager)
            {
                if (dialogueManager.ticketScanned || dialogueManager.ticketRejected)
                {
                    dialogueManager.SetVisualVisibility(false);
                    dialogueManager.HideTicketDisplay();
                }
                else
                    return;
            }

            if (_currentElement == diaryTab)
            {
                dialogueManager.SetVisualVisibility(false);
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.diaryCloseSound, transform.position);
            }
            
            if (!_currentElement) return;
            
            _currentElement.SetVisualVisibility(false);
            _currentElement = null;
            
            _gameStateMachine.ChangeGameState(GameState.Gameplay);
        }
    }
}
