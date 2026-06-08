using System;
using Core;
using Core.Scriptable_Objects.Souvenirs;
using GameMechanics.Interactions;
using GameMechanics.UI;
using GameMechanics.UI.MainMenu;
using Interactions;
using UI.MainMenu;
using UnityEngine;

namespace GameMechanics
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private MainMenuController mainMenuController;
        [SerializeField] private PauseMenuController pauseMenu;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private HarmonyDiaryController diaryTab;
        [SerializeField] private BarkController barkController;
        [SerializeField] private SouvenirViewController souvenirTab;
        [SerializeField] private TutorialNotesController tutorialNotesTab;
        [SerializeField] private SettingsPanelController settingsTab;
        [SerializeField] private CreditsPanelController creditsPanelController;
        private ApplicationGlobalSettings _appSettings;
        private UIElement _settingsReturnTarget;

        public event Action OnUIOpened;
        public event Action OnUIQuit;
        
        private GameStateMachine _gameStateMachine;
        private UIElement _currentElement;

        private UIElement CurrentElement
        {
            get => _currentElement;
            set
            {
                if (_currentElement != value)
                {
                    _currentElement = value;

                    if (_currentElement == dialogueManager) return;

                    if (_currentElement != null)
                    {
                        OnUIOpened?.Invoke();
                        _appSettings.CursorActive(true);
                        return;
                    }

                    OnUIQuit?.Invoke();
                    _appSettings.CursorActive(false);
                }
            }
        }
        

        public void ForceEscape() => OnEscapePressed();

        private void Start()
        {
            _gameStateMachine = DependencyResolver.Instance.GetType<GameStateMachine>();
            _appSettings = DependencyResolver.Instance.GetType<ApplicationGlobalSettings>();
        }

        private void OnEnable()
        {
            PlayerControls.OnEscapePressedEvent += OnEscapePressed;

            GameStateMachine.OnMenuReturned += ClearSettingsReturnTarget;

            PauseMenuController.OnResume += OnResume;
            PauseMenuController.OnSettingsOpened += OpenSettings;
            PauseMenuController.OnMenuReturned += ResetCurrentElement;

            CreditsScrollHandler.ScrollClicked += OpenCredits;

            SettingsPanelController.OnSettingsQuit += QuitPauseSettings;
            SettingsWatchHandler.WatchClicked += OpenSettings;
            
            Passenger.OnPassengerInteracted += OnPassengerInteracted;
            Passenger.OnBark += OnPassengerBarked;
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

            CreditsScrollHandler.ScrollClicked += OpenCredits;

            GameStateMachine.OnMenuReturned += ClearSettingsReturnTarget;

            SettingsPanelController.OnSettingsQuit -= QuitPauseSettings;
            SettingsWatchHandler.WatchClicked -= OpenSettings;

            Passenger.OnPassengerInteracted -= OnPassengerInteracted;
            Passenger.OnBark -= OnPassengerBarked;
            Diary.OnDiaryInteracted -= OnDiaryInteracted;

            Souvenir.OnSouvenirInteracted -= OnSouvenirInteracted;
            Souvenir.OnSouvenirUiQuit -= OnSouvenirUIQuit;

            ConductorGuidelines.OnTutorialNotesInteracted -= OnTutorialNotesInteracted;
        }

        private void OpenCredits()
        {
            creditsPanelController.SetVisualVisibility(true);
            CurrentElement = creditsPanelController;
        }

        private void ClearSettingsReturnTarget()
        {
            _settingsReturnTarget = null;
        }

        private void OnSouvenirInteracted(SouvenirData obj)
        {
            if (_gameStateMachine.GetGameState() == GameState.Paused) return;
            
            if (_gameStateMachine.GetGameState() == GameState.UIOpened)
            {
                souvenirTab.SetVisualVisibility(false);
                CurrentElement = null;
                _gameStateMachine.ChangeGameState(GameState.Gameplay);
                
                return;
            }
            
            souvenirTab.SetVisualVisibility(true);
            CurrentElement = souvenirTab;
            _gameStateMachine.ChangeGameState(GameState.UIOpened);
        }

        private void OnPassengerInteracted(object sender, PassengerInteractedEventArgs obj)
        {
            if (_gameStateMachine.GetGameState() == GameState.Paused) return;
            
            if (_gameStateMachine.GetGameState() == GameState.UIOpened)
            {
                dialogueManager.SetVisualVisibility(false);
                CurrentElement = null;
                _gameStateMachine.ChangeGameState(GameState.Gameplay);
                
                return;
            }
            
            dialogueManager.SetVisualVisibility(true);
            CurrentElement = dialogueManager;
            _gameStateMachine.ChangeGameState(GameState.UIOpened);
        }

        private void OnPassengerBarked(string[] obj, Sprite sprite)
        {
            if (_gameStateMachine.GetGameState() == GameState.Paused) return;
            
            if (_gameStateMachine.GetGameState() == GameState.UIOpened)
            {
                barkController.SetVisualVisibility(false);
                CurrentElement = null;
                _gameStateMachine.ChangeGameState(GameState.Gameplay);
                
                return;
            }
            
            barkController.SetVisualVisibility(true);
            CurrentElement = barkController;
            _gameStateMachine.ChangeGameState(GameState.UIOpened);
        }
        
        private void OnResume()
        {
            CurrentElement = null;
        }

        private void OpenSettings()
        {
            if (CurrentElement == pauseMenu)
            {
                _settingsReturnTarget = pauseMenu;
            }

            CurrentElement = settingsTab;
            settingsTab.SetVisualVisibility(true);
            pauseMenu.SetVisualVisibility(false);
        }

        private void ResetCurrentElement() => CurrentElement = null;

        private void QuitPauseSettings()
        {
            if (CurrentElement != settingsTab) return;

            settingsTab.SetVisualVisibility(false);

            if (_settingsReturnTarget != null)
            {
                _settingsReturnTarget.SetVisualVisibility(true);
                CurrentElement = _settingsReturnTarget;

                if (_settingsReturnTarget == pauseMenu)
                    _gameStateMachine.ChangeGameState(GameState.Paused);
            }
            else
            {
                CurrentElement = null;
                _gameStateMachine.ChangeGameState(GameState.Gameplay);
            }
        }

        private void OnDiaryInteracted()
        {
            if (!ReferenceEquals(CurrentElement, null)) return;
            
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.diaryOpenSound, transform.position);
            CurrentElement = diaryTab;
            diaryTab.SetVisualVisibility(true);
            diaryTab.SetDefaultDiaryState();
            _gameStateMachine.ChangeGameState(GameState.UIOpened);
        }

        private void OnTutorialNotesInteracted()
        {
            if (!ReferenceEquals(CurrentElement, null)) return;
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.creditsOpen, transform.position);
                CurrentElement = tutorialNotesTab;
                tutorialNotesTab.SetVisualVisibility(true);
                tutorialNotesTab.DisplayPage(0);
                _gameStateMachine.ChangeGameState(GameState.UIOpened);
            }
        }
        
        private void OnEscapePressed()
        {
            if (CurrentElement == null && _gameStateMachine.GetGameState() == GameState.Gameplay)
            {
                pauseMenu.SetVisualVisibility(true);
                CurrentElement = pauseMenu;
                _gameStateMachine.ChangeGameState(GameState.Paused);
                return;
            }

            if (CurrentElement == pauseMenu)
            {
                pauseMenu.SetVisualVisibility(false);
                OnUIQuit?.Invoke();
                CurrentElement = null;
                _gameStateMachine.ChangeGameState(GameState.Gameplay);
                return;
            }

            if (CurrentElement == dialogueManager)
            {
                if (dialogueManager.CanCloseDialogueWindow)
                {
                    OnUIQuit?.Invoke();
                    dialogueManager.SetVisualVisibility(false);
                    dialogueManager.HideTicketDisplay();
                    dialogueManager.OnDialogueQuit();
                }
                else
                    return;
            }

            if (CurrentElement == settingsTab)
            {
                settingsTab.SetVisualVisibility(false);
                OnUIQuit?.Invoke();

                if (_settingsReturnTarget != null)
                {
                    _settingsReturnTarget.SetVisualVisibility(true);
                    CurrentElement = _settingsReturnTarget;

                    if (_settingsReturnTarget == pauseMenu)
                        _gameStateMachine.ChangeGameState(GameState.Paused);
                }
                else
                {
                    CurrentElement = null;
                }

                return;
            }

            if (CurrentElement == creditsPanelController)
            {
                creditsPanelController.SetVisualVisibility(false);
                CurrentElement = null;
                return;
            }

            if (CurrentElement == diaryTab)
            {
                OnUIQuit?.Invoke();
                diaryTab.SetVisualVisibility(false);
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.diaryCloseSound, transform.position);
            }
            
            if (CurrentElement == null) return;
            
            CurrentElement.SetVisualVisibility(false);
            CurrentElement = null;
            OnUIQuit?.Invoke();
            
            _gameStateMachine.ChangeGameState(GameState.Gameplay);
        }

        private void OnSouvenirUIQuit()
        {
            souvenirTab.SetVisualVisibility(false);
            CurrentElement = null;
            OnUIQuit?.Invoke();
            _gameStateMachine.ChangeGameState(GameState.Gameplay);
        }
    }
}
