using System;
using Core.Scriptable_Objects;
using GameMechanics.Interactions;
using UnityEngine;

namespace Interactions
{
    public class Passenger : MonoBehaviour, IInteractable
    {
        [SerializeField] private PassengerData passengerData;
        
        public static Action<PassengerData> OnPassengerInteracted;

        public void Interact()
        {
            OnPassengerInteracted?.Invoke(passengerData);
        }

        public void DisplayUI()
        {
            
        }
    }
}
