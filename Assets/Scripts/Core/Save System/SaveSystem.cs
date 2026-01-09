
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.Save_System
{
    [InitializeSystem("Save System")]
    public class SaveSystem : BaseSystem
    {
        //TODO: make multiple saving possible
        private static string _savePath, _filePath;
        public List<int> souvenirIDs = new();
        public int ticketsAccepted, ticketsRejected;
        public List<string> diaryEntries = new();
        private List<GameSaveData> _saves = new();
        private int _currentLoadedSaveIndex;

        protected override void Awake()
        {
            base.Awake();
            
            _savePath = Path.Combine(Application.persistentDataPath, "EnchantedExpressSaves");
        }
        
        public void SaveToJson()
        {
            GameSaveData saveData = new(1, ticketsAccepted, ticketsRejected, souvenirIDs.ToArray(), diaryEntries.ToArray());

            string jsonText = JsonUtility.ToJson(saveData, prettyPrint: true);
            _savePath = Path.Combine(Application.persistentDataPath, "EnchantedExpressSaves");
            Directory.CreateDirectory(_savePath);
            _filePath = Path.Combine(_savePath, "save.json");
            
            Debug.Log("Saved");
            File.WriteAllText(_filePath, jsonText);
        }

        public GameSaveData GetSaveData()
        {
            if (!File.Exists(_filePath))
            {
                Debug.Log("No save data");
            }
            
            string jsonText = File.ReadAllText(_filePath);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(jsonText);

            return data;
        }
    }

    public struct GameSaveData
    {
        public int CurrentDay;
        public int TicketsAccepted;
        public int TicketsRejected;
        public int[] CollectedSouvenirIdList;
        public string[] DiaryEntries;

        public GameSaveData(int currentDay,  int ticketsAccepted, int ticketsRejected, int[] collectedSouvenirIdList, string[] diaryEntries)
        {
            CurrentDay = currentDay;
            TicketsAccepted  = ticketsAccepted;
            TicketsRejected = ticketsRejected;
            CollectedSouvenirIdList = collectedSouvenirIdList;
            DiaryEntries = diaryEntries;
        }
    }
}