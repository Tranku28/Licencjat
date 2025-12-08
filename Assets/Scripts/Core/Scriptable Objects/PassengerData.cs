using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "PassengerData", menuName = "Scriptable Objects/PassengerData")]
    public class PassengerData : ScriptableObject
    {
        public Sprite passengerPortrait;
        
        public TextAsset inkJSON;
        
        public string passengerName;
        public string passengerSurname;

        public int car;
        public int seat;
        
        public string destination;
        public ValidUntil validUntil;

        [Header("Consequences")]
        public GameObject souvenirPrefab;
        [TextArea(minLines: 5, maxLines: 10)]
        public string newspaperText, souvenirNote, diaryContent;
        public string souvenirName;

        public string GetDate()
        {
            return $"{validUntil.Day}.{validUntil.Month:D2}.{validUntil.Year} {validUntil.Hour}:{validUntil.Minute:D2}";
        }

        public int ticketNumber => Random.Range(000000000, 999999999);
    }
    
    [Serializable]
    public struct ValidUntil
    {
        public int Day;
        public int Month;
        public int Year;

        public int Hour;
        public int Minute;
    } 
}
