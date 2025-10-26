using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private RectTransform arrow;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        void Update()
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

            GetPauseOption(angle);
        }

        private static PauseOptions GetPauseOption(float angle)
        {
            return angle switch
            {
                >= 260 and <= 335 => PauseOptions.Resume,
                >= 155 and <= 205 => PauseOptions.Settings,
                >= 25 and <= 90 => PauseOptions.MainMenu,
                _ => PauseOptions.None
            };
        }
        
        private enum PauseOptions
        {
            None,
            Resume,
            Settings,
            MainMenu
        }
    }
}
