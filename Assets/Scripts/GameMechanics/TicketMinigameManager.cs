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
    public class TicketMinigameManager : UIElement, IPointerClickHandler
    {
        [SerializeField] private Image holeImage;
        [SerializeField] private GameObject holeParentObject;

        [SerializeField] private TMP_Text passengerFullNameText;
        [SerializeField] private TMP_Text passengerDestinationText;
        [SerializeField] private TMP_Text passengerExpireDateText;
        
        private GameStateMachine _gameManager;

        private void Awake()
        {
            if (Camera.main == null) throw new Exception("Camera not found");
        }

        private void Start()
        {
            Passenger.OnPassengerInteracted += UpdateTicketUI;
            
            _gameManager = DependencyResoler.Instance.GetType<GameStateMachine>();
        }

        private void OnDestroy()
        {
            Passenger.OnPassengerInteracted -= UpdateTicketUI;
        }

        private void UpdateTicketUI(PassengerData data)
        {
            Debug.Log($"UpdateTicketUI: {data}");
            
            passengerFullNameText.text = $"{data.passengerName} {data.passengerSurname}";
            passengerDestinationText.text = data.destination;
            passengerExpireDateText.text = data.GetDate();
            
            ShowUI();
        }

        private void ShowUI()
        {
            _gameManager.ChangeGameState(GameState.UIOpened);
            holeParentObject.SetActive(true);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!gameObject.activeInHierarchy) return;
            
            MakeHole();
        }
        
        private void MakeHole()
        {
            var cursorPos = Mouse.current.position.ReadValue();
        
            var hole = Instantiate(holeImage,cursorPos, Quaternion.identity);
            hole.transform.SetParent(holeParentObject.transform);
        }
    }
}
