using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.Save_System
{
    [InitializeSystem("Save System")]
    public class SaveSystem : BaseSystem
    {
        //TODO: make multiple saving possible
        private int _saveIndex, _currentDay;
        private static string _savePath, _filePath;
        public List<int> souvenirIDs = new();
        public int ticketsAccepted, ticketsRejected;
        public List<string> tutorialNotes = new();
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
            GameSaveData saveData = new(_saveIndex, _currentDay, ticketsAccepted, ticketsRejected, souvenirIDs.ToArray(), diaryEntries.ToArray(), tutorialNotes.ToArray());

            string jsonText = JsonUtility.ToJson(saveData, prettyPrint: true);
            Directory.CreateDirectory(_savePath);
            _filePath = Path.Combine(_savePath, $"save_{_saveIndex}.json");
            
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
        public int SaveIndex;
        public int CurrentDay;
        public int TicketsAccepted;
        public int TicketsRejected;
        public int[] CollectedSouvenirIdList;
        public string[] DiaryEntries;
        public string[] TutorialNotes;

        public GameSaveData(int saveIndex, int currentDay,  int ticketsAccepted, int ticketsRejected, int[] collectedSouvenirIdList, string[] diaryEntries, string[] tutorialNotes)
        {
            SaveIndex = saveIndex;
            CurrentDay = currentDay;
            TicketsAccepted  = ticketsAccepted;
            TicketsRejected = ticketsRejected;
            CollectedSouvenirIdList = collectedSouvenirIdList;
            DiaryEntries = diaryEntries;
            TutorialNotes = tutorialNotes;
        }
    }
}