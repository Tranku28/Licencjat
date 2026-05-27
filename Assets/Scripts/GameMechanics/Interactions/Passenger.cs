using System;
using Core;
using Core.Save_System;
using Core.Scriptable_Objects;
using GameMechanics.Interactions;
using UnityEngine;

namespace Interactions
{
    public class Passenger : MonoBehaviour, IInteractable
    {
        [SerializeField] private PassengerData passengerData;
        private bool _canBark = false;
        public PassengerData PassengerData => passengerData;

        public bool StaysNextDay {get; set;}
        
        public static event EventHandler<PassengerInteractedEventArgs> OnPassengerInteracted;
        public static event Action<string[], Sprite> OnBark;

        public string GetName()
        {
            return passengerData.passengerName;
        }

        public void Interact()
        {
            if (_canBark) 
            {
                GameSaveData gameSaveData = DependencyResolver.Instance.GetType<SaveSystem>().GetCurrentSave();
                if (passengerData.dayAppears + 1 == gameSaveData.CurrentDay)
                {
                    OnBark?.Invoke(passengerData.additionalBarks, passengerData.passengerPortrait);
                    return;
                }

                OnBark?.Invoke(passengerData.barks, passengerData.passengerPortrait);
                return;
            }

            _canBark = true;
            OnPassengerInteracted?.Invoke(this, new PassengerInteractedEventArgs(passengerData));
        }

        public void ResetInteractable() => _canBark = false;
        public void SetBarking(bool value) => _canBark = value;
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
