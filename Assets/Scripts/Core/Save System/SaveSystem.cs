using System.Collections.Generic;
using System.IO;
using Core.Scriptable_Objects;
using UnityEngine;

namespace Core.Save_System
{
    // TODO: Rework it to file save to JSON system later
    [InitializeSystem("Save System")]
    public class SaveSystem : BaseSystem
    {
        private static string _savePath, _filePath;

        private List<SaveData> _cachedDataList = new();

        protected override void Awake()
        {
            base.Awake();
            
            _savePath = Path.Combine(Application.persistentDataPath, "EnchantedExpressSaves");
            
            _cachedDataList = new List<SaveData>();
            SaveToJson();
            Debug.Log(GetSaveData());
        }
        
        public void Save(PassengerData passengerData, bool ticketScanned)
        {
            SaveData saveData = new SaveData(passengerData, ticketScanned);
            _cachedDataList.Add(saveData);
        }

        public SaveData[] GetCachedSaveDataList()
        {
            return _cachedDataList.ToArray();
        }
        
        private void SaveToJson()
        {
            GameSaveData saveData = new GameSaveData(1, 0, 1, new List<int>{1,4,6,12,3,329});

            string jsonText = JsonUtility.ToJson(saveData, prettyPrint: true);
            _savePath = Path.Combine(Application.persistentDataPath, "EnchantedExpressSaves");
            Directory.CreateDirectory(_savePath);
            _filePath = Path.Combine(_savePath, "save.json");
            
            File.WriteAllText(_filePath, jsonText);
        }

        private GameSaveData? GetSaveData()
        {
            if (!File.Exists(_filePath))
            {
                Debug.LogError("Save file not found!");
                return null;
            }
            
            string jsonText = File.ReadAllText(_filePath);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(jsonText);

            Debug.Log(data);

            return data;
        }
    }
    
    public struct SaveData
    {
        public PassengerData passengerData;
        public bool ticketScanned;
            
        public SaveData(PassengerData passengerData, bool ticketScanned)
        {
            this.passengerData = passengerData;
            this.ticketScanned = ticketScanned;
        }
    }

    public struct GameSaveData
    {
        public int currentDay;
        public int ticketsAccepted;
        public int ticketsDeclined;
        public List<int> collectedSouvenirIdList;

        public GameSaveData(int currentDay,  int ticketsAccepted, int ticketsDeclined, List<int> collectedSouvenirIdList)
        {
            this.currentDay = currentDay;
            this.ticketsAccepted  = ticketsAccepted;
            this.ticketsDeclined = ticketsDeclined;
            this.collectedSouvenirIdList = collectedSouvenirIdList;
        }
    }
}