using System;
using Core;
using Core.Scriptable_Objects;
using GameMechanics.UI;
using Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameMechanics
{
    public class TicketMinigame : UIElement, IPointerClickHandler
    {
        [SerializeField] private Image holeImage;
        [SerializeField] private GameObject visual;

        [SerializeField] private TMP_Text passengerFullNameText;
        [SerializeField] private TMP_Text passengerDestinationText;
        [SerializeField] private TMP_Text passengerExpireDateText;

        private bool _canScan;

        public bool canScan
        {
            get => _canScan;
            set => _canScan = value;
        }

        private GameStateMachine _gameManager;

        private void Awake()
        {
            if (Camera.main == null) throw new Exception("Camera not found");
        }

        public void UpdateTicketUI(PassengerData data)
        {
            passengerFullNameText.text = $"{data.passengerName} {data.passengerSurname}";
            passengerDestinationText.text = data.destination;
            passengerExpireDateText.text = data.GetDate();
        }

        public void ShowUI()
        {
            visual.SetActive(!visual.activeSelf);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_canScan) MakeHole();
        }
        
        private void MakeHole()
        {
            Vector2 cursorPos = Mouse.current.position.ReadValue();
            
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    visual.transform as RectTransform,
                    cursorPos,
                    Camera.main,
                    out Vector2 localPoint))
            {
                var hole = Instantiate(holeImage, visual.transform);
                
                hole.rectTransform.anchoredPosition = localPoint;
                hole.rectTransform.localRotation = Quaternion.identity;
                hole.rectTransform.localScale = Vector3.one;
            }
        }
    }
}
