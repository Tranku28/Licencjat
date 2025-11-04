using System;
using GameMechanics.Interactions;
using Scriptable_Objects;
using UnityEngine;

namespace Interactions
{
    public class Passenger : MonoBehaviour, IInteractable
    {
        [SerializeField] private PassengerData passengerData;
        
        public static Action<PassengerData> OnPassengerInteracted;

        public void Interact()
        {
            Debug.Log("Interacted");
            OnPassengerInteracted?.Invoke(passengerData);
        }

        public void DisplayUI()
        {
            
        }
    }
}
