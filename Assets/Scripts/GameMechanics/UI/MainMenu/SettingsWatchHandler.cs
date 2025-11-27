using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMechanics.UI.MainMenu
{
    public class SettingsWatchHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private static readonly int Selected = Animator.StringToHash("Selected");
        [SerializeField] private Animator watchAnimator;
        public event Action WatchClicked;
    
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Watch clicked");
            WatchClicked?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            watchAnimator.SetBool(Selected, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            watchAnimator.SetBool(Selected, false);
        }
    }
}
