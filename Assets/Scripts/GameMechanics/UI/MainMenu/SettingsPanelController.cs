using Core;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMechanics.UI.MainMenu
{
    public class SettingsPanelController : UIElement, IPointerMoveHandler, IPointerClickHandler, IPointerUpHandler
    {
        [SerializeField] private Button goBackButton;

        [Header("Arrows")]
        [SerializeField] private RectTransform musicArrow;
        [SerializeField] private RectTransform sfxArrow;

        [Header("Config")]
        [SerializeField] private Canvas canvas;

        private void OnEnable()
        {
            goBackButton.onClick.AddListener(BackToMainMenu);
        }

        private void OnDisable()
        {
            goBackButton.onClick.RemoveListener(BackToMainMenu);
        }

        private void BackToMainMenu()
        {
            gameObject.SetActive(false);
        }
        
        public void OnPointerMove(PointerEventData eventData)
        {
            
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            
        }
        
        private void RotateArrowTowardCursor(RectTransform arrow, PointerEventData eventData, float minZ, float maxZ)
        {
            RectTransform parent = arrow.parent as RectTransform;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent,
                eventData.position,
                canvas.worldCamera,
                out Vector2 cursorPos);
            
            Vector2 arrowPos = arrow.anchoredPosition;

            Vector2 dir = cursorPos - arrowPos;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            float clamped = Mathf.Clamp(angle, minZ, maxZ);

            arrow.localRotation = Quaternion.Euler(0f, 0f, clamped);
        }
    }
}
