using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.Save_System
{
    [InitializeSystem("Save System")]
    public class SaveSystem : BaseSystem
    {
        private const string SAVE_FOLDER = "EnchantedExpress";
        private const string SAVE_FILE_BASE_FORMAT = "EnchantedExpress_";
        private const int MAX_SAVES_SLOTS = 5;
        private int _loadedSaveIndex = 0;
        private static string _savePath;
        private List<GameSaveData> _saves = new ();
        private List<ISaveElement> _saveElements = new();

        public event Action OnSaveDeleted;

        public int RuntimeSaveIndex => _loadedSaveIndex;
        public GameSaveData GetCurrentSave() => _saves[_loadedSaveIndex];

        protected override void Awake()
        {
            base.Awake();

            _savePath = Path.Combine(Application.persistentDataPath, SAVE_FOLDER);

            ReadAllSaveData();
        }

        public void SaveGame(bool isNewGame = false)
        {
            if (_loadedSaveIndex > MAX_SAVES_SLOTS) return;
            
            if (isNewGame) 
            {
                GameSaveData newGameSaveData = new GameSaveData
                {
                    SaveIndex = _saves.Count
                };

                _saves.Add(newGameSaveData);
                _loadedSaveIndex = _saves.Count-1;
            }

            try
            {
                Directory.CreateDirectory(_savePath);

                string dateOfSave = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                _saves[_loadedSaveIndex].DateSaved = dateOfSave;

                PreviousDaySave(_saves[_loadedSaveIndex]);

                if (isNewGame)
                {
                    _saves[_loadedSaveIndex].CurrentDay = 0;
                }

                _saves[_loadedSaveIndex].CurrentDay++;

                foreach(ISaveElement saveElement in _saveElements)
                {
                    saveElement.SaveData(_saves[_loadedSaveIndex]);
                }

                string saveData = JsonUtility.ToJson(_saves[_loadedSaveIndex], true);

                string fullPath = Path.Combine(_savePath, $"{SAVE_FILE_BASE_FORMAT}{_loadedSaveIndex}");

                using FileStream fileStream = new FileStream(fullPath, FileMode.Create);
                using StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(saveData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception while saving game: {e}");
            }
        }

        /// <summary>
        /// Must be used before any other ISaveElement performs save
        /// </summary>
        /// <param name="saveData"></param>
        private void PreviousDaySave(GameSaveData saveData)
        {
            PreviousDaySnapshot previousDayData = new PreviousDaySnapshot
            {
                SaveIndex = saveData.SaveIndex,
                DateSaved = saveData.DateSaved,
                CurrentDay = saveData.CurrentDay,
                HarmonyStatus = saveData.HarmonyStatus,
                TicketsAccepted = saveData.TicketsAccepted,
                TicketsRejected = saveData.TicketsRejected,
                CollectedSouvenirIdList = saveData.CollectedSouvenirIdList,
                PassengerEntries = saveData.PassengerEntries,
            };
            
            saveData.LastDayData = previousDayData;
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

        public void LoadPreviousDay(PreviousDaySnapshot snapshotSaveData)
        {
            Debug.Log("Save Index: " + snapshotSaveData.SaveIndex);

            if (_saves.Count == 0) return;

            foreach (ISaveElement saveElement in _saveElements)
            {
                saveElement.LoadSave(_saves[snapshotSaveData.SaveIndex]);
            }

            Debug.Log($"Current Day: {snapshotSaveData.CurrentDay}");
        }

        public void ReloadSaveOnNewJourney()
        {
            foreach (ISaveElement saveElement in _saveElements)
            {
                saveElement.LoadSave(_saves[_loadedSaveIndex]);
            }
        }

        public void DeleteSave(int index)
        {
            string savePath = Path.Combine(_savePath, $"{SAVE_FILE_BASE_FORMAT}{index}");
            
            try
            {
                File.Delete(savePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete save: {e}");
            }

            ReloadSaveData();
        }

        private void ReloadSaveData()
        {
            ReadAllSaveData();
            OnSaveDeleted?.Invoke();
        }

        private void ReadAllSaveData()
        {
            _saves.Clear();

            for(int i=0; i < MAX_SAVES_SLOTS; i++)
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

        public void ForceDeleteSaveSouvenirData(int id)
        {
            try
            {
                GameSaveData runtimeSave = GetCurrentSave();
                runtimeSave.CollectedSouvenirIdList.Remove(id);
            } catch (Exception e)
            {
                Debug.Log($"Błąd usunięcia itemu z zapisu: {e}");
            }
        }

        public void RegisterToSaveSystem(ISaveElement saveElement)
        {
            if (_saveElements.Contains(saveElement)) return;

            _saveElements.Add(saveElement);
        }

        private void OnDestroy()
        {
            _saveElements.Clear();
        }

        public GameSaveData[] GetSaves => _saves.ToArray();
    }
}