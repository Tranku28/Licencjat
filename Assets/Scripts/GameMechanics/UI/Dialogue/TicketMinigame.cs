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
        [SerializeField] private Image holeImage;
        [SerializeField] private GameObject visual;

        [SerializeField] private TMP_Text passengerFullNameText;
        [SerializeField] private TMP_Text passengerDestinationText;
        [SerializeField] private TMP_Text passengerExpireDateText;
        [SerializeField] private TMP_Text passengerCarNumber;
        [SerializeField] private TMP_Text passengerSeatNumber;
        [SerializeField] private TMP_Text ticketNumberText;
        [SerializeField] private Image passengerPortrait;
        [SerializeField] private PuncherMover puncher;

        private List<Image> holesList = new();

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
        }

        public void SetupTicketUI(PassengerData data)
        {
            passengerFullNameText.text = $"{data.passengerName} {data.passengerSurname}";
            passengerCarNumber.text = data.car.ToString();
            passengerSeatNumber.text = data.seat.ToString();
            ticketNumberText.text = $"No. {data.ticketNumber}";
            passengerDestinationText.text = data.destination;
            passengerExpireDateText.text = data.GetDate();
            passengerPortrait.sprite = data.passengerPortrait;

            _canScan = false;

            if (holesList.Count == 0) return;

            foreach (Image hole in holesList)
            {
                Destroy(hole.gameObject);
            }

            holesList.Clear();
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
            
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    visual.transform as RectTransform,
                    cursorPos,
                    Camera.main,
                    out Vector2 localPoint))
            {
                Image hole = Instantiate(holeImage, visual.transform);
                holesList.Add(hole);
                
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
