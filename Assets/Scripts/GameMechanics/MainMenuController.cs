using System;
using Core;
using GameMechanics.Player;
using GameMechanics.UI;
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

        [Header("Menu Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button quitButton;
        
        [Header("Menu Panels")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject creditsPanel;

        private Button[] _menuMainButtons = new Button[4];
        
        private Vector3 _playerHeadTargetPosition;
        private Quaternion _playerHeadTargetRotation;
        private bool _gameStarted;
        private Camera _playerCamera;
        private bool _moveDone;
        
        public static Action PlayButtonPressed;

        private void Awake()
        {
            AssignButtons();
        }

        private void AssignButtons()
        {
            playButton.onClick.AddListener(StartGame);
            _menuMainButtons[0] = playButton;
            settingsButton.onClick.AddListener(GoToSettings);
            _menuMainButtons[1] = settingsButton;
            creditsButton.onClick.AddListener(GoToCredits);
            _menuMainButtons[2] = creditsButton;
            quitButton.onClick.AddListener(QuitGame);
            _menuMainButtons[3] = quitButton;
        }

        private void Start()
        {
            _moveDone = false;
            cameraRotateSpeed = cameraMoveSpeed * 36;
            
            CameraSetup();
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

        public void ButtonVisibilitySwitch(bool switchFlag)
        {
            foreach (var button in _menuMainButtons)
            {
                button.gameObject.SetActive(switchFlag);
            }
        }
        
        private void StartGame()
        {
            PlayButtonPressed?.Invoke();
            _gameStarted = true;
        }


        private void GoToSettings()
        {
            ButtonVisibilitySwitch(false);

            settingsPanel.SetActive(true);
        }

        private void GoToCredits()
        {
            ButtonVisibilitySwitch(false);
            
            creditsPanel.SetActive(true);
        }
        private void QuitGame()
        {
            Application.Quit();
        }
    }
}
