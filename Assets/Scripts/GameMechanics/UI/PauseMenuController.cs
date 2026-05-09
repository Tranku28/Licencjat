using System;
using Core;
using Core.Scriptable_Objects;
using GameMechanics.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GameMechanics
{
    public class PauseMenuController : UIElement
    {
        [SerializeField] private RectTransform arrow;
        [SerializeField] private GameObject pauseMenuVisual;
        private Canvas _canvas;

        [SerializeField] private TMP_Text resume, backToMenu, settings;
        [SerializeField] private GameObject pauseSettingsPanel;
        private PauseOptions _currentOption;
        public bool opened {get; private set;}

        public static Action OnResume;
        public static Action OnSettingsOpened;
        public static Action OnMenuReturned;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        public override void SetVisualVisibility(bool show)
        {
            base.SetVisualVisibility(show);

            opened = show;
        }

        private void OnEnable()
        {
            PlayerControls.OnClickEvent += OnMenuClick;
        }

        private void OnDisable()
        {
            PlayerControls.OnClickEvent -= OnMenuClick;
        }

        public void ShowPauseMenu()
        {
            pauseMenuVisual.SetActive(true);
        }

        private void OnMenuClick()
        {
            if (!pauseMenuVisual.activeSelf) return;

            InvokeOption();
        }

        private void InvokeOption()
        {
            var gameStateMachine = DependencyResolver.Instance.GetType<GameStateMachine>();
            
            
            switch (_currentOption)
            {
                case PauseOptions.MainMenu:
                    gameStateMachine.OnMainMenuReturn();
                    pauseMenuVisual.SetActive(false);
                    OnMenuReturned?.Invoke();
                    break;
                
                case PauseOptions.Settings:
                    pauseSettingsPanel.SetActive(true);
                    OnSettingsOpened?.Invoke();
                    break;
                
                case PauseOptions.Resume:
                    
                    gameStateMachine.ChangeGameState(GameState.Gameplay);
                    pauseMenuVisual.SetActive(false);
                    OnResume?.Invoke();
                    
                    break;
                case PauseOptions.None:
                default:
                    break;
            }
        }

        private void Update()
        {
            if (!pauseMenuVisual.activeSelf) return;

            var mousePos = Mouse.current.position.ReadValue();

            var localPoint = GetCursorPosition(mousePos);

            RotateArrow(localPoint);

            _currentOption = GetPauseOption(arrow.localEulerAngles.z);
        }

        private Vector2 GetCursorPosition(Vector2 mousePos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                mousePos,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out Vector2 localPoint
            );
            return localPoint;
        }

        private void RotateArrow(Vector2 localPoint)
        {
            Vector2 dir = (localPoint - (Vector2)arrow.localPosition).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            arrow.localRotation = Quaternion.Euler(0, 0, angle - 90f);
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
    }
    
    public enum PauseOptions
    {
        None,
        Resume,
        Settings,
        MainMenu
    }
}
