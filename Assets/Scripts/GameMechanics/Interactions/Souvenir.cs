using System;
using Core.Scriptable_Objects;
using GameMechanics.Interactions;
using UnityEngine;

public class Souvenir : MonoBehaviour, IInteractable
{
    [SerializeField] private PassengerData souvenirData;

    public static event Action<PassengerData> OnSouvenirInteracted;

    public string GetName()
    {
        return souvenirData.souvenirName;
    }

    public void Interact()
    {
        OnSouvenirInteracted?.Invoke(souvenirData);
    }
}
