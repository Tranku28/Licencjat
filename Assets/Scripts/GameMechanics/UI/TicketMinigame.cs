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
            Debug.Log(_canScan);
            if (_canScan) MakeHole();
        }
        
        private void MakeHole()
        {
            var cursorPos = Mouse.current.position.ReadValue();
        
            var hole = Instantiate(holeImage,cursorPos, Quaternion.identity);
            hole.transform.SetParent(visual.transform);
        }
    }
}
