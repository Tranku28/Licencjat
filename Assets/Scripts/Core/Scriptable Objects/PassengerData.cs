using System;
using UnityEngine;

namespace Core.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "PassengerData", menuName = "Scriptable Objects/PassengerData")]
    public class PassengerData : ScriptableObject
    {
        public TextAsset inkJSON;
        
        public string passengerName;
        public string passengerSurname;

        public string destination;
        public ValidUntil validUntil;

        public string GetDate()
        {
            return $"{validUntil.Day:D2} / {validUntil.Month:D2} / {validUntil.Hour}:{validUntil.Minute}";
        }
    }
    
    [Serializable]
    public struct ValidUntil
    {
        public int Day;
        public int Month;

        public int Hour;
        public int Minute;
    } 
}
