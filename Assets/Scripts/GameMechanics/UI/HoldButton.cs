using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action<bool> OnHoldStateChanged;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnHoldStateChanged?.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnHoldStateChanged?.Invoke(false);
    }
}