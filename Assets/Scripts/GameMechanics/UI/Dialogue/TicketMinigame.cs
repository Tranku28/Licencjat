using System;
using System.Collections.Generic;
using Core;
using Core.Scriptable_Objects;
using GameMechanics.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameMechanics
{
    public class TicketMinigame : UIElement, IPointerClickHandler, IPointerMoveHandler
    {
        [SerializeField] private Material holeMaterial;
        private Material _defaultMaterial;
        [SerializeField] private GameObject visual;

        [SerializeField] private TMP_Text passengerFullNameText;
        [SerializeField] private TMP_Text passengerDestinationText;
        [SerializeField] private TMP_Text passengerExpireDateText;
        [SerializeField] private TMP_Text passengerCarNumber;
        [SerializeField] private TMP_Text passengerSeatNumber;
        [SerializeField] private TMP_Text ticketNumberText;
        [SerializeField] private Image passengerPortrait;
        [SerializeField] private PuncherMover puncher;

        [Header("Ticket Rendering")]
        [SerializeField] private RawImage ticketBackgroundImage;
        [SerializeField] private Camera ticketRenderCamera;

        public event Action OnTicketScanned;
        
        public class OnTicketPointerMovedEventArgs : EventArgs
        {
            public PuncherOrientation Orientation;
            public Quaternion Rotation;

            public OnTicketPointerMovedEventArgs(PuncherOrientation orientation, Quaternion ticketRotation)
            {
                Orientation = orientation;
                Rotation = ticketRotation;
            }
        }

        public event EventHandler<OnTicketPointerMovedEventArgs> OnTicketMoved;
        private Camera _camera;

        private bool _canScan;

        public bool canScan
        {
            get => _canScan;
            set => _canScan = value;
        }

        private void Awake()
        {
            if (Camera.main == null) throw new Exception("Camera not found");
            _camera = Camera.main;

            _defaultMaterial = new Material(ticketBackgroundImage.material);
            ticketBackgroundImage.material = _defaultMaterial;
        }

        public void SetupTicketUI(PassengerData data)
        {
            RenderTicket();

            passengerFullNameText.text = $"{data.passengerName} {data.passengerSurname}";

            if (data is InvalidPassengerData)
            {
                passengerFullNameText.text = $"{(data as InvalidPassengerData).invalidName} {(data as InvalidPassengerData).invalidSurname}";
            }

            passengerCarNumber.text = data.car.ToString();
            passengerSeatNumber.text = data.seat.ToString();
            ticketNumberText.text = $"No. {data.ticketNumber}";
            passengerDestinationText.text = data.destination;
            passengerExpireDateText.text = data.GetDate();
            passengerPortrait.sprite = data.passengerPortrait;

            _canScan = false;

            ticketBackgroundImage.material = _defaultMaterial;
        }

        public void ShowUI() => visual.SetActive(!visual.activeSelf);
        public void HideUI() => visual.SetActive(false);
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_canScan) MakeHole();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != visual)
            {
                puncher.gameObject.SetActive(false);
                return;
            }

            puncher.gameObject.SetActive(true);

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
            float distBottom = Mathf.Abs(localPoint.y - rect.yMin);
            float distTop = Mathf.Abs(localPoint.y - rect.yMax);
            
            if (distLeft <= distRight && distLeft <= distTop && distLeft <= distBottom)
            {
                OnTicketMoved?.Invoke(this, new OnTicketPointerMovedEventArgs(PuncherOrientation.Left, visual.transform.rotation));
            }
            else if (distRight <= distLeft && distRight <= distTop && distRight <= distBottom)
            {
                OnTicketMoved?.Invoke(this, new OnTicketPointerMovedEventArgs(PuncherOrientation.Right, visual.transform.rotation));
            }
            else if (distTop <= distLeft && distTop <= distRight && distTop <= distBottom)
            {
                OnTicketMoved?.Invoke(this, new OnTicketPointerMovedEventArgs(PuncherOrientation.Bottom, visual.transform.rotation));
            }
            else
            {
                OnTicketMoved?.Invoke(this, new OnTicketPointerMovedEventArgs(PuncherOrientation.Top, visual.transform.rotation));
            }
        }


        private void MakeHole()
        {
            OnTicketScanned?.Invoke();

            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.puncherSound, transform.position);

            Vector2 cursorPos = Mouse.current.position.ReadValue();

            RectTransform rectTransform = visual.transform as RectTransform;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform,
                    cursorPos,
                    Camera.main,
                    out Vector2 localPoint))
            {
                Rect rect = rectTransform.rect;
                Vector2 pivot = rectTransform.pivot;
                
                float normalizedX = (localPoint.x + rect.width * pivot.x) / rect.width;
                float normalizedY = (localPoint.y + rect.height * pivot.y) / rect.height;

                Vector2 holeUV = new(normalizedX, normalizedY);


                holeUV.x = Mathf.Clamp01(holeUV.x);
                holeUV.y = Mathf.Clamp01(holeUV.y);

                ticketBackgroundImage.material = holeMaterial;
                ticketBackgroundImage.material.SetVector("_HoleCenter", holeUV);
            }

            _canScan = false;
        }

        private void RenderTicket()
        {
            ticketRenderCamera.Render();
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
