using System;
using Core.Scriptable_Objects;
using GameMechanics.Interactions;
using UnityEngine;

namespace Interactions
{
    public class Passenger : MonoBehaviour, IInteractable
    {
        [SerializeField] private PassengerData passengerData;
        private bool _tutorialShown;
        public PassengerData PassengerData => passengerData;
        
        public static event EventHandler<PassengerInteractedEventArgs> OnPassengerInteracted;

        public string GetName()
        {
            return passengerData.passengerName;
        }

        public void Interact()
        {
            OnPassengerInteracted?.Invoke(this, new PassengerInteractedEventArgs(passengerData));
        }
    }
    
    public class PassengerInteractedEventArgs : EventArgs
    {
        public PassengerData PassengerData;

        public PassengerInteractedEventArgs(PassengerData passengerData)
        {
            PassengerData = passengerData;
        }
    }
}
