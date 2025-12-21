using System;
using Core.Scriptable_Objects;
using GameMechanics.Interactions;
using UnityEngine;

namespace Interactions
{
    public class Passenger : MonoBehaviour, IInteractable
    {
        [SerializeField] private PassengerData passengerData;
        [SerializeField] private TutorialData passengerTutorial;
        private bool _tutorialShown;
        
        public static event EventHandler<PassengerInteractedEventArgs> OnPassengerInteracted;

        public void Interact()
        {
            // TODO: Tutorial single popup handling
            if (!_tutorialShown)
            {
                TutorialInfoLoader.Instance.LoadTutorialPanel(passengerTutorial);
                _tutorialShown = true;
            }
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
