using System;
using Core;
using GameMechanics.UI;
using Interactions;
using Scriptable_Objects;
using TMPro;
using UI;
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

        private InputAction _quitTicketUIInput;
        private GameManager _gameManager;

        private void Awake()
        {
            if (Camera.main == null) throw new Exception("Camera not found");
            
            _quitTicketUIInput = InputSystem.actions.FindAction("Interact");
            _quitTicketUIInput.performed += QuitTicketUI;
        }

        private void QuitTicketUI(InputAction.CallbackContext obj)
        {
            if (holeParentObject.activeSelf) holeParentObject.SetActive(false);
            _gameManager.CurrentGameState = GameState.Game;
        }

        private void Start()
        {
            Passenger.OnPassengerInteracted += UpdateTicketUI;
            
            _gameManager = Registry.Instance.GetType<GameManager>();
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
            _gameManager.CurrentGameState = GameState.Paused;
            holeParentObject.SetActive(true);
            _quitTicketUIInput.Enable();
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
