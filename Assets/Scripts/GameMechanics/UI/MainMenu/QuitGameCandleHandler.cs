using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMechanics.UI.MainMenu
{
    public class QuitGameCandleHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Candle click");
            Application.Quit();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Candle enter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Candle exit");
        }
    }
}
