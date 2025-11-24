using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsScrollHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Scroll clicked");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Scroll selected");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Scroll put back");
    }
}
