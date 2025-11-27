using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMechanics.UI.MainMenu
{
    public class SettingsWatchHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action WatchClicked;
    
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Watch clicked");
            WatchClicked?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Watch enter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Watch exit");
        }
    }
}
