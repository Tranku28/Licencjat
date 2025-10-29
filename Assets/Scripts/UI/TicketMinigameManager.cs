using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class TicketMinigameManager : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image holeImage;
        private RectTransform _transformParent;

        private void Awake()
        {
            if (Camera.main == null) throw new Exception("Camera not found");
            _transformParent = GetComponent<RectTransform>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerClick.name != _transformParent.name) return;
            
            MakeHole();
        }
        
        private void MakeHole()
        {
            var cursorPos = Mouse.current.position.ReadValue();
        
            var hole = Instantiate(holeImage,cursorPos, Quaternion.identity);
            hole.transform.SetParent(_transformParent);
        }
    }
}
