using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMechanics.UI.MainMenu
{
    public class QuitGameCandleHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Application.Quit();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
    }
}
