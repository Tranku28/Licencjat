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
        private float cameraRotateSpeed;

        [Header("Menu Elements")]
        [SerializeField] private StartGameBoardHandler startBoard;
        [SerializeField] private SettingsWatchHandler settingsWatch;
        [SerializeField] private CreditsScrollHandler creditsScroll;
        [SerializeField] private QuitGameCandleHandler candleQuit;
        
        [Header("Menu Panels")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject creditsPanel;
        
        private Vector3 _playerHeadTargetPosition;
        private Quaternion _playerHeadTargetRotation;
        private bool _gameStarted;
        private Camera _playerCamera;
        private bool _moveDone;
        
        public static Action PlayButtonPressed;
        
        private void Start()
        {
            _moveDone = false;
            cameraRotateSpeed = cameraMoveSpeed * 36;

            creditsScroll.ScrollClicked += GoToCredits;
            settingsWatch.WatchClicked += GoToSettings;
            startBoard.OnStartClicked += StartGame;
            
            CameraSetup();
        }

        private void OnDestroy()
        {
            creditsScroll.ScrollClicked -= GoToCredits;
            settingsWatch.WatchClicked -= GoToSettings;
            startBoard.OnStartClicked -= StartGame;
        }
        

        private void CameraSetup()
        {
            _playerCamera = playerMovementController.gameObject.GetComponentInChildren<Camera>();
            _playerHeadTargetPosition = _playerCamera.transform.position;
            _playerHeadTargetRotation = _playerCamera.transform.rotation;
            
            _playerCamera.transform.position = mainMenuCameraTransform.position;
            _playerCamera.transform.rotation = mainMenuCameraTransform.rotation;
        }

        private void Update()
        {
            if (!_gameStarted) return;

            switch (_moveDone)
            {
                case false:
                    SmoothTransform();
                    return;
                case true:
                    SmoothRotate();
                    break;
            }
        }

        private void SmoothTransform()
        {
            var step = cameraMoveSpeed * Time.deltaTime;
                
            _playerCamera.transform.position = 
                Vector3.MoveTowards(
                    _playerCamera.transform.position,
                    _playerHeadTargetPosition,
                    step
                );

            var distance = Vector3.Distance(_playerCamera.transform.position, _playerHeadTargetPosition);
                
            if (distance < 0.1f)
            {
                _moveDone = true;
            }
        }
        
        private void SmoothRotate()
        {
            var step = cameraRotateSpeed * Time.deltaTime;

            _playerCamera.transform.rotation = Quaternion.RotateTowards(
                _playerCamera.transform.rotation, 
                _playerHeadTargetRotation,
                step
                );

            if (!(Quaternion.Angle(_playerCamera.transform.rotation, _playerHeadTargetRotation) < 0.1f)) return;

            var gameStateMachine = DependencyResoler.Instance.GetType<GameStateMachine>();
            gameStateMachine.StartGame();
            
            enabled = false;
        }
        
        
        private void StartGame()
        {
            PlayButtonPressed?.Invoke();
            _gameStarted = true;
        }


        private void GoToSettings()
        {
            settingsPanel.SetActive(true);
        }

        private void GoToCredits()
        {
            creditsPanel.SetActive(true);
        }
    }
}
