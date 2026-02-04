using System;
using Core;
using GameMechanics.Player;
using GameMechanics.UI;
using GameMechanics.UI.MainMenu;
using Player;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private QuitGameCandleHandler candleQuit;
        
        [Header("Menu Panels")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject creditsPanel;
        
        private Camera _playerCamera;
        
        private void Start()
        {
            creditsScroll.ScrollClicked += GoToCredits;
            settingsWatch.WatchClicked += GoToSettings;
            startBoard.OnGameplayEntered += StartGame;
        }

        private void OnDestroy()
        {
            creditsScroll.ScrollClicked -= GoToCredits;
            settingsWatch.WatchClicked -= GoToSettings;
            startBoard.OnGameplayEntered -= StartGame;
        }
        
        
        private void StartGame(bool isNewGame)
        {
            DisableMenu();
            
            GameStateMachine gameStateMachine = DependencyResolver.Instance.GetType<GameStateMachine>();
            
            gameStateMachine.StartGame(isNewGame);
        }


        private void GoToSettings()
        {
            settingsPanel.SetActive(true);
        }

        private void GoToCredits()
        {
            creditsPanel.SetActive(true);
        }

        private void DisableMenu()
        {
            settingsWatch.enabled = false;
            candleQuit.enabled = false;
            creditsScroll.enabled = false;
            startBoard.enabled = false;
        }
    }
}
