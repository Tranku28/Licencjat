using Core;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMechanics.UI.MainMenu
{
    public class SettingsPanelController : UIElement, IPointerClickHandler
    {
        public float offset;
        [SerializeField] private Button goBackButton;

        [Header("Arrows")]
        [SerializeField] private RectTransform musicArrow;
        [SerializeField] private RectTransform sfxArrow;
        [SerializeField] private Image musicFill, sfxFill;


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
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>() == musicFill)
            {
                Debug.Log("Raycast on music");
                RotateArrowTowardCursor(musicArrow, eventData);
            }

            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>() == sfxFill)
            {
                Debug.Log("Raycast on sfx");
                RotateArrowTowardCursor(sfxArrow, eventData);
            }
        }
        
        private void RotateArrowTowardCursor(RectTransform arrow, PointerEventData eventData)
        {
            RectTransform parent = arrow.parent as RectTransform;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent,
                eventData.position,
                null,
                out Vector2 cursorPos);
            
            Vector2 arrowPos = arrow.anchoredPosition;

            Vector2 dir = cursorPos - arrowPos;

            if (arrow == musicArrow)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                arrow.localRotation = Quaternion.Euler(0f, 0f, angle);
                UpdateFill(musicFill, eventData.position);
            }

            if (arrow == sfxArrow)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                arrow.localRotation = Quaternion.Euler(0f, 0f, angle);
                UpdateFill(sfxFill, eventData.position);
                UpdateAudioLevel();
            }
        }


        public bool UpdateFill(Image image, Vector2 screenPosition, Camera uiCamera = null)
        {
            RectTransform rt = image.rectTransform;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPosition, uiCamera, out Vector2 localPoint))
            {
                return false;
            }

            Rect rect = rt.rect;

            float t = Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x);

            if (image.fillMethod == Image.FillMethod.Horizontal && image.fillOrigin == 1)
            {
                t = 1f - t;
            }

            t = Mathf.Clamp01(t);
            image.fillAmount = t;

            bool isInside =
                localPoint.x >= rect.xMin && localPoint.x <= rect.xMax &&
                localPoint.y >= rect.yMin && localPoint.y <= rect.yMax;

            return isInside;
        }


        private void UpdateAudioLevel()
        {
            //TODO: Update audio
        }
    }
}
