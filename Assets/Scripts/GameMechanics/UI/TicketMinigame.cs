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
        [SerializeField] private TMP_Text passengerCarNumber;
        [SerializeField] private TMP_Text passengerSeatNumber;
        [SerializeField] private TMP_Text ticketNumberText;
        [SerializeField] private Image passengerPortrait;

        public class OnTicketClickedEventArgs : EventArgs
        {
            public PuncherOrientation Orientation;
            public bool CanScan;

            public OnTicketClickedEventArgs(PuncherOrientation orientation, bool canScan)
            {
                Orientation = orientation;
                CanScan = canScan;
            }
        }

        public event EventHandler<OnTicketClickedEventArgs> OnTicketClicked;
        private Camera _camera;

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
            _camera = Camera.main;
        }

        public void UpdateTicketUI(PassengerData data)
        {
            passengerFullNameText.text = $"{data.passengerName} {data.passengerSurname}";
            passengerCarNumber.text = data.car.ToString();
            passengerSeatNumber.text = data.seat.ToString();
            ticketNumberText.text = $"No. {data.ticketNumber.ToString()}";
            passengerDestinationText.text = data.destination;
            passengerExpireDateText.text = data.GetDate();
            passengerPortrait.sprite = data.passengerPortrait;
        }

        public void ShowUI()
        {
            visual.SetActive(!visual.activeSelf);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_canScan) MakeHole();

            RectTransform rectTransform = visual.GetComponent<RectTransform>();
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                eventData.position,
                _camera,
                out var localPoint
                );

            Rect rect = rectTransform.rect;
            float distLeft = Mathf.Abs(localPoint.x - rect.xMin);
            float distRight = Mathf.Abs(localPoint.x - rect.xMax);
            float distTop = Mathf.Abs(localPoint.y - rect.yMin);
            float distBottom = Mathf.Abs(localPoint.y - rect.yMax);
            
            float minDist = Mathf.Min(distLeft, distRight, distTop, distBottom);

            if (Mathf.Approximately(minDist, distLeft))
                OnTicketClicked?.Invoke(this, new OnTicketClickedEventArgs(PuncherOrientation.Left, _canScan));
            if (Mathf.Approximately(minDist, distRight))
                OnTicketClicked?.Invoke(this, new OnTicketClickedEventArgs(PuncherOrientation.Right, _canScan));
            if (Mathf.Approximately(minDist, distTop))
                OnTicketClicked?.Invoke(this, new OnTicketClickedEventArgs(PuncherOrientation.Top, _canScan));
            if (Mathf.Approximately(minDist, distBottom))
                OnTicketClicked?.Invoke(this, new OnTicketClickedEventArgs(PuncherOrientation.Bottom, _canScan));
        }
        
        private void MakeHole()
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.puncherSound, transform.position);
            
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

    public enum PuncherOrientation
    {
        Left,
        Right,
        Top,
        Bottom
    }
}
