using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMechanics.UI.MainMenu
{
    public class CreditsScrollHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private static readonly int Selected = Animator.StringToHash("Selected");
        [SerializeField] private Animator scrollAnimator;
        public event Action ScrollClicked;
        public void OnPointerClick(PointerEventData eventData)
        {
            ScrollClicked?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            scrollAnimator.SetBool(Selected, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            scrollAnimator.SetBool(Selected, false);
        }
    }
}
