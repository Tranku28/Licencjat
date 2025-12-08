using System.Collections.Generic;
using Core.Scriptable_Objects;
using UnityEngine;

namespace Core
{
    // TODO: Rework it to file save to XML system later
    [InitializeSystem("Save System")]
    public class SaveSystem : BaseSystem
    {
        private List<SaveData> _cachedDataList = new();

        protected override void Awake()
        {
            base.Awake();
            _cachedDataList = new List<SaveData>();
        }
        
        public void Save(PassengerData passengerData, bool ticketScanned)
        {
            SaveData saveData = new SaveData(passengerData, ticketScanned);
            _cachedDataList.Add(saveData);
            Debug.Log($"Save data added: {saveData.PassengerData} & {saveData.TicketScanned}");
        }

        public SaveData[] GetCachedSaveDataList()
        {
            return _cachedDataList.ToArray();
        }
    }
    
    public struct SaveData
    {
        public PassengerData PassengerData;
        public bool TicketScanned;
            
        public SaveData(PassengerData passengerData, bool ticketScanned)
        {
            PassengerData = passengerData;
            TicketScanned = ticketScanned;
        }
    }
}