using System;
using GameMechanics.Interactions;
using UnityEngine;

public class Diary : MonoBehaviour, IInteractable
{
    public static Action OnDiaryInteracted;
    
    public void Interact()
    {
        OnDiaryInteracted?.Invoke();
    }
}
