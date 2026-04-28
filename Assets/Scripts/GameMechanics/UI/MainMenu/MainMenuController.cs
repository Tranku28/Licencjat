using Core;
using GameMechanics.Player;
using GameMechanics.UI;
using GameMechanics.UI.MainMenu;
using UnityEngine;

namespace UI.MainMenu
{
    public class MainMenuController : UIElement
    {
        [SerializeField] private PlayerMovementController playerMovementController;
        [SerializeField] private Transform mainMenuCameraTransform;
        [SerializeField] private float cameraMoveSpeed;

        [Header("Menu Elements")]
        [SerializeField] private StartGameBoardHandler startBoard;
        [SerializeField] private SettingsWatchHandler settingsWatch;
        [SerializeField] private CreditsScrollHandler creditsScroll;
        [SerializeField] private QuitGameCandleHandler candleQuitHandler;
        
        [Header("Menu Panels")]
        [SerializeField] private SettingsPanelController settingsPanel;
        [SerializeField] private GameObject creditsPanel;
        
        private Camera _playerCamera;
        
        private void Start()
        {
            creditsScroll.ScrollClicked += GoToCredits;
            settingsWatch.WatchClicked += GoToSettings;
            startBoard.OnGameplayEntered += StartGame;

            GameStateMachine.OnMenuReturned += EnableMenu;
        }

        private void OnDestroy()
        {
            creditsScroll.ScrollClicked -= GoToCredits;
            settingsWatch.WatchClicked -= GoToSettings;
            startBoard.OnGameplayEntered -= StartGame;

            GameStateMachine.OnMenuReturned -= EnableMenu;
        }
        
        
        private void StartGame(bool isNewGame)
        {
            DisableMenu();
            
            GameStateMachine gameStateMachine = DependencyResolver.Instance.GetType<GameStateMachine>();
            
            gameStateMachine.StartGame(isNewGame);
        }


        private void GoToSettings()
        {
            settingsPanel.SetVisualVisibility(true);
        }

        private void GoToCredits()
        {
            creditsPanel.SetActive(true);
        }

        private void DisableMenu()
        {
            settingsWatch.enabled = false;
            candleQuitHandler.enabled = false;
            creditsScroll.enabled = false;
            startBoard.enabled = false;
        }

        private void EnableMenu()
        {
            settingsWatch.enabled = true;
            candleQuitHandler.enabled = true;
            creditsScroll.enabled = true;
            startBoard.enabled = true;
            //TODO: Prettify it
            startBoard.MenuReturned();
        }
    }
}
