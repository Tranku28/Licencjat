using System;
using Core.Scriptable_Objects;
using GameMechanics.Interactions;
using UnityEngine;

namespace Interactions
{
    public class Passenger : MonoBehaviour, IInteractable
    {
        [SerializeField] private PassengerData passengerData;
        private bool _alreadyInteracted = false;
        public PassengerData PassengerData => passengerData;
        
        public static event EventHandler<PassengerInteractedEventArgs> OnPassengerInteracted;

        public string GetName()
        {
            return passengerData.passengerName;
        }

        public void Interact()
        {
            if (_alreadyInteracted) return;

            _alreadyInteracted = true;
            //TODO: additional short answer on multiple interactions
            OnPassengerInteracted?.Invoke(this, new PassengerInteractedEventArgs(passengerData));
        }

        public void ResetInteractable() => _alreadyInteracted = false;
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
