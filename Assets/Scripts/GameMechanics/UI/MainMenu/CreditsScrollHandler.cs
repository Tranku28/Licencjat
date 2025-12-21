using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Core;

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
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.creditsOpen, transform.position);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            scrollAnimator.SetBool(Selected, true);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.creditsHover, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            scrollAnimator.SetBool(Selected, false);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.creditsHover, transform.position);
        }
    }
}
