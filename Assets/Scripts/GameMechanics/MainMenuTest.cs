using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuTest : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
    }

    public void Clicked()
    {
        Debug.Log("Clicked");
    }
}
