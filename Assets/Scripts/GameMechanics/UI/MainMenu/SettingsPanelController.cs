using Core;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;
using System;

namespace GameMechanics.UI.MainMenu
{
    public class SettingsPanelController : UIElement, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public float offset;
        [SerializeField] private Button goBackButton;

        [Header("Arrows")]
        [SerializeField] private RectTransform musicArrow;
        [SerializeField] private RectTransform sfxArrow;
        [SerializeField] private Image musicFill, sfxFill;

        private VCA _musicVCA, _sfxVCA;
        private bool _clicked;

        public static Action OnSettingsQuit;

        void Awake()
        {
            RuntimeManager.StudioSystem.getVCA("vca:/Music", out _musicVCA);
            RuntimeManager.StudioSystem.getVCA("vca:/SFX", out _sfxVCA);
        }

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
            SetVisualVisibility(false);
        }
        
        public void OnPointerDown(PointerEventData eventData) => _clicked = true;

        public void OnPointerUp(PointerEventData eventData) => _clicked = false;

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_clicked) return;

            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>() == musicFill)
            {
                RotateArrowTowardCursor(musicArrow, eventData);
            }

            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>() == sfxFill)
            {
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
                UpdateFill(out float audioLevel, musicFill, eventData.position);
                SetAudioLevel(_musicVCA, audioLevel);
            }

            if (arrow == sfxArrow)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                arrow.localRotation = Quaternion.Euler(0f, 0f, angle);
                UpdateFill(out float audioLevel, sfxFill, eventData.position);
                SetAudioLevel(_sfxVCA, audioLevel);
            }
        }


        public bool UpdateFill(out float value, Image image, Vector2 screenPosition, Camera uiCamera = null)
        {
            RectTransform rt = image.rectTransform;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPosition, uiCamera, out Vector2 localPoint))
            {
                value = 1;
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

            value = image.fillAmount;
            return isInside;
        }


        private void SetAudioLevel(VCA vca, float value)
        {
            vca.setVolume(value);
        }
    }
}
