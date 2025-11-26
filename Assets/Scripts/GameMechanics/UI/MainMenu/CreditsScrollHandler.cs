using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsScrollHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action ScrollClicked;
    public void OnPointerClick(PointerEventData eventData)
    {
        ScrollClicked?.Invoke();
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
