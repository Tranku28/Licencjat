using System;
using GameMechanics.Interactions;
using UnityEngine;

public class TutorialNotes : MonoBehaviour, IInteractable
{
    public static event Action OnTutorialNotesInteracted;
    public void Interact()
    {
        OnTutorialNotesInteracted?.Invoke();
    }
}
