using System;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuManager : MonoBehaviour, IRegister
    {
        [SerializeField] private PlayerMovementController playerMovementController;
        [SerializeField] private Transform mainMenuCameraTransform;
        [SerializeField] private float cameraMoveSpeed;

        [Header("Menu Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button quitButton;
        
        [Header("Menu Panels")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject creditsPanel;

        private Button[] _menuMainButtons = new Button[4];
        
        private Vector3 _playerCameraPosition;
        private bool _gameStarted;
        private Camera _playerCamera;

        public Action OnGameStarted;

        private void Awake()
        {
            playButton.onClick.AddListener(StartGame);
            _menuMainButtons[0] = playButton;
            settingsButton.onClick.AddListener(GoToSettings);
            _menuMainButtons[1] = settingsButton;
            creditsButton.onClick.AddListener(GoToCredits);
            _menuMainButtons[2] = creditsButton;
            quitButton.onClick.AddListener(QuitGame);
            _menuMainButtons[3] = quitButton;
            
            Register();
        }
        

        private void Start()
        {
            playerMovementController.enabled = false;
            
            _playerCamera = playerMovementController.gameObject.GetComponentInChildren<Camera>();
            _playerCameraPosition = _playerCamera.transform.position;
            
            _playerCamera.transform.position = mainMenuCameraTransform.position;
        }

        private void OnDestroy()
        {
            Unregister();
        }

        private void Update()
        {
            if (!_gameStarted) return;

            SmoothGameplayTransition();
        }

        private void SmoothGameplayTransition()
        {
            var step = cameraMoveSpeed * Time.deltaTime;
                
            _playerCamera.transform.position = 
                Vector3.MoveTowards(
                    _playerCamera.transform.position,
                    _playerCameraPosition,
                    step
                );

            if (Vector3.Distance(_playerCamera.transform.position, _playerCameraPosition) < 0.1f)
            {
                playerMovementController.enabled = true;
                enabled = false;
            }
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
            OnGameStarted?.Invoke();
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
            
        }


        public void Register()
        {
            Registry.Instance.Register(this);
        }

        public void Unregister()
        {
            Registry.Instance.Unregister(this);
        }
    }
}
