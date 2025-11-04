using System;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuManager : MonoBehaviour, IRegister
    {
        [SerializeField] private Transform mainMenuCameraTransform;
        [SerializeField] private Transform playerCameraTransform;
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
        
        private bool _gameStarted;
        [SerializeField] private Camera playerCamera;

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
            playerCameraTransform = playerCamera.transform;
            
            playerCamera.transform.position = mainMenuCameraTransform.position;
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
                
            playerCamera.transform.position = 
                Vector3.MoveTowards(
                    playerCamera.transform.position,
                    playerCameraTransform.position,
                    step
                );

            if (Vector3.Distance(playerCamera.transform.position, playerCameraTransform.position) < 0.1f)
            {
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
