using System;
using Core;
using Core.Scriptable_Objects;
using Core.Scriptable_Objects.Souvenirs;
using GameMechanics.Interactions;
using GameMechanics.UI;
using GameMechanics.UI.MainMenu;
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
        [SerializeField] private SouvenirViewController souvenirTab;
        [SerializeField] private TutorialNotesController tutorialNotesTab;
        [SerializeField] private SettingsPanelController settingsTab;
        
        private UIElement _currentElement;
        
        private GameStateMachine _gameStateMachine;

        private void Start()
        {
            _gameStateMachine = DependencyResolver.Instance.GetType<GameStateMachine>();
        }

        //TODO: make actions non static
        private void OnEnable()
        {
            PlayerControls.OnEscapePressedEvent += OnEscapePressed;

            PauseMenuController.OnResume += OnResume;
            PauseMenuController.OnSettingsOpened += OpenSettings;
            PauseMenuController.OnMenuReturned += ResetCurrentElement;

            SettingsPanelController.OnSettingsQuit += QuitPauseSettings;
            
            Passenger.OnPassengerInteracted += OnPassengerInteracted;
            Diary.OnDiaryInteracted += OnDiaryInteracted;

            Souvenir.OnSouvenirInteracted += OnSouvenirInteracted;
            Souvenir.OnSouvenirUiQuit += OnSouvenirUIQuit;

            ConductorGuidelines.OnTutorialNotesInteracted += OnTutorialNotesInteracted;
        }


        private void OnDisable()
        {
            PlayerControls.OnEscapePressedEvent -= OnEscapePressed;
            
            PauseMenuController.OnResume -= OnResume;
            PauseMenuController.OnSettingsOpened -= OpenSettings;
            PauseMenuController.OnMenuReturned -= ResetCurrentElement;

            SettingsPanelController.OnSettingsQuit -= QuitPauseSettings;

            Passenger.OnPassengerInteracted -= OnPassengerInteracted;
            Diary.OnDiaryInteracted -= OnDiaryInteracted;

            Souvenir.OnSouvenirInteracted -= OnSouvenirInteracted;
            Souvenir.OnSouvenirUiQuit -= OnSouvenirUIQuit;

            ConductorGuidelines.OnTutorialNotesInteracted -= OnTutorialNotesInteracted;
        }
        
        private void OnSouvenirInteracted(SouvenirData obj)
        {
            if (_gameStateMachine.GetGameState() == GameState.Paused) return;
            
            if (_gameStateMachine.GetGameState() == GameState.UIOpened)
            {
                souvenirTab.SetVisualVisibility(false);
                _currentElement = null;
                _gameStateMachine.ChangeGameState(GameState.Gameplay);
                
                return;
            }
            
            souvenirTab.SetVisualVisibility(true);
            _currentElement = souvenirTab;
            _gameStateMachine.ChangeGameState(GameState.UIOpened);
        }

        private void OnPassengerInteracted(object sender, PassengerInteractedEventArgs obj)
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

        private void OpenSettings()
        {
            _currentElement = settingsTab;
            settingsTab.SetVisualVisibility(true);
            pauseMenu.SetVisualVisibility(false);
        }

        private void ResetCurrentElement() => _currentElement = null;

        private void QuitPauseSettings()
        {
            if (_currentElement != settingsTab) return;

            _currentElement = pauseMenu;
            settingsTab.SetVisualVisibility(false);
            pauseMenu.SetVisualVisibility(true);
        }

        private void OnDiaryInteracted()
        {
            if (!ReferenceEquals(_currentElement, null)) return;
            
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.diaryOpenSound, transform.position);
            _currentElement = diaryTab;
            diaryTab.SetVisualVisibility(true);
            diaryTab.SetDefaultDiaryState();
            _gameStateMachine.ChangeGameState(GameState.UIOpened);
        }

        private void OnTutorialNotesInteracted()
        {
            if (!ReferenceEquals(_currentElement, null)) return;
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.creditsOpen, transform.position);
                _currentElement = tutorialNotesTab;
                tutorialNotesTab.SetVisualVisibility(true);
                _gameStateMachine.ChangeGameState(GameState.UIOpened);
            }
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
                    dialogueManager.OnDialogueQuit();
                }
                else
                    return;
            }

            if (_currentElement == settingsTab)
            {
                settingsTab.SetVisualVisibility(false);
                pauseMenu.SetVisualVisibility(true);
                _currentElement = pauseMenu;
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

        private void OnSouvenirUIQuit()
        {
            souvenirTab.SetVisualVisibility(false);
            _currentElement = null;
            _gameStateMachine.ChangeGameState(GameState.Gameplay);
        }
    }
}
