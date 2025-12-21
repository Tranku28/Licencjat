using UnityEngine;
using UnityEngine.EventSystems;
using Core;

namespace GameMechanics.UI.MainMenu
{
    public class QuitGameCandleHandler : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.candleBlow, transform.position);
            Application.Quit();
        }
    }
}
