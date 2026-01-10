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
        private const string SAVE_FOLDER = "EnchantedExpress";
        private const string SAVE_FILE_BASE_FORMAT = "EnchantedExpress_";
        private const int SAVES_COUNT = 4;
        private int _saveIndex, _currentDay;
        private static string _savePath, _filePath;
        public List<int> souvenirIDs = new();
        public int ticketsAccepted, ticketsRejected;
        public List<string> tutorialNotes = new();
        public List<string> diaryEntries = new();
        private List<GameSaveData> _saves = new ();
        private int _currentLoadedSaveIndex;
        private List<ISaveElement> _saveElements = new();

        protected override void Awake()
        {
            base.Awake();
            
            Debug.Log("Save system Initialized");
            _savePath = Path.Combine(Application.persistentDataPath, SAVE_FOLDER);

            ReadAllSaveData();
        }
        
        public void SaveGame(int saveIndex = 1)
        {
            if (saveIndex > SAVES_COUNT) return;
            if (_saves.Count-1 < saveIndex) _saves.Add(new GameSaveData());

            try
            {
                Directory.CreateDirectory(_savePath);

                foreach(ISaveElement saveElement in _saveElements)
                {
                    saveElement.SaveData(_saves[saveIndex]);
                }

                string saveData = JsonUtility.ToJson(_saves[saveIndex], true);

                string fullPath = Path.Combine(_savePath, $"{SAVE_FILE_BASE_FORMAT}{saveIndex}");

                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(saveData);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception while saving game: {e}");
            }
        }

        //TODO: set this private later
        public void LoadSave(int saveIndex)
        {
            foreach (ISaveElement saveElement in _saveElements)
            {
                saveElement.LoadSave(_saves[saveIndex]);
            }
        }

        private void ReadAllSaveData()
        {
            for(int i=0; i < SAVES_COUNT; i++)
            {
                string currentSavePath = Path.Combine(_savePath, $"{SAVE_FILE_BASE_FORMAT}{i}");

                if (!File.Exists(currentSavePath)) continue;

                Debug.Log($"File {i} found");

                try
                {
                    string saveJson = "";
                    using (FileStream fileStream = new FileStream(currentSavePath, FileMode.Open))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream))
                        {
                            saveJson = streamReader.ReadToEnd();
                        }
                        GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(saveJson);
                        _saves.Add(gameSaveData);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error during game load on file {i}: {e}");
                }
            }

            Debug.Log($"Saves count: {_saves.Count}");
        }

        public void RegisterToSaveSystem(ISaveElement saveElement)
        {
            if (_saveElements.Contains(saveElement)) return;

            _saveElements.Add(saveElement);
        }
    }
}