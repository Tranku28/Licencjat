using Core;
using GameMechanics.UI;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenuController : UIElement
    {
        [SerializeField] private RectTransform arrow;
        [SerializeField] private GameObject pauseMenuVisual;
        private Canvas _canvas;

        [SerializeField] private TMP_Text resume, backToMenu, settings;
        private PauseOptions _currentOption;

        private InputAction _clickInput;
        
        private PlayerMovementController _playerMovementController;
        private GameManager _gameManager;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _clickInput = InputSystem.actions.FindAction("Click");
        }

        private void ShowPauseMenu(bool isPaused)
        {
            pauseMenuVisual.SetActive(true);
            _playerMovementController.enabled = false;
        }

        private void OnMenuClick(InputAction.CallbackContext obj)
        {
            switch (_currentOption)
            {
                case PauseOptions.MainMenu:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                case PauseOptions.Settings:
                    Debug.Log("Settings");
                    break;
                case PauseOptions.Resume:
                    
                    pauseMenuVisual.SetActive(false);
                    _clickInput.performed -= OnMenuClick;
                    _playerMovementController.enabled = true;
                    _gameManager.CurrentGameState = GameState.Game;
                    
                    break;
                case PauseOptions.None:
                default:
                    Debug.Log("Unrecognized pause option");
                    break;
            }
        }

        private void Update()
        {
            var mousePos = Mouse.current.position.ReadValue();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                mousePos,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out Vector2 localPoint
            );

            Vector2 dir = (localPoint - (Vector2)arrow.localPosition).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            arrow.localRotation = Quaternion.Euler(0, 0, angle - 90f);

            _currentOption = GetPauseOption(arrow.localEulerAngles.z);
        }
        
        private static PauseOptions GetPauseOption(float zAngle)
        {
            return zAngle switch
            {
                >= 260 and <= 335 => PauseOptions.Resume,
                >= 155 and <= 205 => PauseOptions.Settings,
                >= 25 and <= 90 => PauseOptions.MainMenu,
                _ => PauseOptions.None
            };
        }

        private void OnDestroy()
        {
            _clickInput.performed -= OnMenuClick;
        }
    }
    
    public enum PauseOptions
    {
        None,
        Resume,
        Settings,
        MainMenu
    }
}
