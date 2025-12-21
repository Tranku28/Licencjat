using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Core;

namespace GameMechanics.UI.MainMenu
{
    public class SettingsWatchHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private static readonly int Selected = Animator.StringToHash("Selected");
        [SerializeField] private Animator watchAnimator;
        public event Action WatchClicked;
    
        public void OnPointerClick(PointerEventData eventData)
        {
            WatchClicked?.Invoke();
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.watchOpen, transform.position);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            watchAnimator.SetBool(Selected, true);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.watchHover, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            watchAnimator.SetBool(Selected, false);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.watchHover, transform.position);
        }
    }
}
