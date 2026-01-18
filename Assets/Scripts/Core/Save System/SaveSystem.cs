using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
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
        //TODO: Deserialize it
        [SerializeField] private int _loadedSaveIndex;
        private static string _savePath, _filePath;
#region Fields To Save
        private int _currentDay;
        public List<int> souvenirIDs = new();
        public int ticketsAccepted, ticketsRejected;
        public List<string> tutorialNotes = new();
        public List<string> diaryEntries = new();
        private List<GameSaveData> _saves = new ();
        private List<ISaveElement> _saveElements = new();
#endregion

        protected override void Awake()
        {
            base.Awake();

            _savePath = Path.Combine(Application.persistentDataPath, SAVE_FOLDER);
            _loadedSaveIndex = 0;

            ReadAllSaveData();
        }
        
        public void SaveGame()
        {
            if (_loadedSaveIndex > SAVES_COUNT) return;
            if (_saves.Count-1 < _loadedSaveIndex) _saves.Add(new GameSaveData());

            try
            {
                Directory.CreateDirectory(_savePath);

                foreach(ISaveElement saveElement in _saveElements)
                {
                    saveElement.SaveData(_saves[_loadedSaveIndex]);
                }

                string saveData = JsonUtility.ToJson(_saves[_loadedSaveIndex], true);

                string fullPath = Path.Combine(_savePath, $"{SAVE_FILE_BASE_FORMAT}{_loadedSaveIndex}");

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
            if (_saves.Count == 0) return;

            _loadedSaveIndex = saveIndex;

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

        public GameSaveData[] GetSaves => _saves.ToArray();
    }
}