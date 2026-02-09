using System;
using System.Text;
using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "PassengerData", menuName = "Scriptable Objects/PassengerData")]
    public class PassengerData : ScriptableObject
    {
        [field: SerializeField] public Sprite passengerPortrait {get; private set;}
        
        [field: SerializeField] public GameObject prefab {get; private set;}
        [field: SerializeField] public TextAsset inkJSON {get; private set;}
        
        [field: SerializeField] public string passengerName {get; private set;}
        [field: SerializeField] public string passengerAction {get; private set;}
        [field: SerializeField] public string passengerSurname {get; private set;}
        [field: SerializeField] public int dayAppears {get; private set;}

        [field: SerializeField] public int car {get; private set;}
        [field: SerializeField] public int seat {get; private set;}
        
        [field: SerializeField] public string destination {get; private set;}
        [field: SerializeField] public ValidUntil validUntil {get; private set;}

        [Header("Card Details")]
        [field: SerializeField] public string species {get; private set;}
        [field: SerializeField] public string causeOfDeath {get; private set;}
        [field: SerializeField, TextArea(3,5)] public string biography {get; private set;}

        [Header("Consequences")]
        [field: SerializeField] public SouvenirData souvenirData {get; private set;}
        [field: SerializeField, TextArea(3, 5)] public string newspaperText {get; private set;}
        [field: SerializeField, TextArea(3, 5)] public string souvenirNote {get; private set;}
        [field: SerializeField, TextArea(3, 5)] public string diaryContent {get; private set;}
        [field: SerializeField] public string souvenirName {get; private set;}

        public string GetDate()
        {
            return $"{validUntil.Day}.{validUntil.Month:D2}.{validUntil.Year} {validUntil.Hour}:{validUntil.Minute:D2}";
        }

        public string GetFullName() => $"{passengerName} {passengerSurname}";

        public int ticketNumber => Random.Range(000000000, 999999999);
    }
    
    [Serializable]
    public struct ValidUntil
    {
        [field: SerializeField] public int Day {get; private set;}
        [field: SerializeField] public int Month {get; private set;}
        [field: SerializeField] public int Year {get; private set;}

        [field: SerializeField] public int Hour {get; private set;}
        [field: SerializeField] public int Minute {get; private set;}
    } 
}
