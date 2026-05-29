using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Core.Save_System
{
    [InitializeSystem("Save System")]
    public class SaveSystem : BaseSystem
    {
        private const string SAVE_FOLDER = "EnchantedExpress";
        private const string SAVE_FILE_BASE_FORMAT = "EnchantedExpress_";
        private const string SAVE_FILE_EXTENSION = ".json";
        private const int MAX_SAVES_SLOTS = 5;

        private GameSaveData _loadedSave = null;
        private static string _savePath;

        private readonly List<GameSaveData> _saves = new();
        private readonly List<ISaveElement> _saveElements = new();

        public event Action OnSaveDeleted;

        public GameSaveData GetCurrentSave() => _loadedSave; 
        public GameSaveData[] GetSaves => _saves.ToArray();

        protected override void Awake()
        {
            base.Awake();

            _savePath = Path.Combine(Application.persistentDataPath, SAVE_FOLDER);
            Directory.CreateDirectory(_savePath);

            GameStateMachine.OnMenuReturned += ResetCurrentSave;

            ReadAllSaveData();
        }

        public void SaveGame(bool isNewGame = false)
        {
            try
            {
                Directory.CreateDirectory(_savePath);

                if (isNewGame)
                {
                    if (_saves.Count >= MAX_SAVES_SLOTS)
                    {
                        Debug.LogWarning($"Cannot create new save. Max save slots reached ({MAX_SAVES_SLOTS}).");
                        return;
                    }

                    _loadedSave = new GameSaveData
                    {
                        SaveID = Guid.NewGuid().ToString(),
                        DateSaved = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
                        CurrentDay = 0,
                        HarmonyStatus = 100,
                        PassengerStayNames = new(),
                        CollectedSouvenirIdList = new(),
                        PassengerEntries = new(),
                        LastDayData = new()
                    };
                }

                if (_loadedSave == null)
                {
                    Debug.LogWarning("Cannot save game because no save is currently loaded.");
                    return;
                }

                PreviousDaySave(_loadedSave);

                if (isNewGame)
                {
                    _loadedSave.CurrentDay = 0;
                }

                _loadedSave.CurrentDay++;
                _loadedSave.DateSaved = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

                if (!isNewGame)
                {
                    foreach (ISaveElement saveElement in _saveElements)
                    {
                        if (saveElement == null) continue;
                        saveElement.SaveData(_loadedSave);
                    }
                }

                string saveDataJson = JsonUtility.ToJson(_loadedSave, true);
                string fullPath = GetSaveFilePath(_loadedSave.SaveID);

                File.WriteAllText(fullPath, saveDataJson);

                AddOrReplaceSaveInCache(_loadedSave);
                SortSavesByDateDescending();
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception while saving game: {e}");
            }
        }

        /// <summary>
        /// Must be used before any other ISaveElement performs save.
        /// Creates a snapshot of the previous day data.
        /// </summary>
        private void PreviousDaySave(GameSaveData saveData)
        {
            if (saveData == null)
                return;

            PreviousDaySnapshot previousDayData = new PreviousDaySnapshot
            {
                DateSaved = saveData.DateSaved,
                CurrentDay = saveData.CurrentDay,
                HarmonyStatus = saveData.HarmonyStatus,
                TicketsAccepted = saveData.TicketsAccepted,
                TicketsRejected = saveData.TicketsRejected,

                CollectedSouvenirIdList = saveData.CollectedSouvenirIdList != null
                    ? new List<int>(saveData.CollectedSouvenirIdList)
                    : new List<int>(),

                PassengerEntries = saveData.PassengerEntries != null
                    ? new List<PassengerEntry>(saveData.PassengerEntries)
                    : new List<PassengerEntry>()
            };

            saveData.LastDayData = previousDayData;
        }

        public void LoadSave(GameSaveData gameSaveData)
        {
            if (gameSaveData == null)
            {
                Debug.LogWarning("LoadSave called with null save data.");
                return;
            }

            _loadedSave = gameSaveData;

            foreach (ISaveElement saveElement in _saveElements)
            {
                if (saveElement == null) continue;
                saveElement.LoadSave(gameSaveData);
            }
        }

        public void LoadPreviousDay(PreviousDaySnapshot snapshotSaveData)
        {
            if (snapshotSaveData == null)
            {
                Debug.LogWarning("LoadPreviousDay called with null snapshot.");
                return;
            }

            GameSaveData previousDaySaveData = CreateSaveDataFromSnapshot(snapshotSaveData);
            _loadedSave = previousDaySaveData;

            foreach (ISaveElement saveElement in _saveElements)
            {
                if (saveElement == null) continue;
                saveElement.LoadSave(previousDaySaveData);
            }
        }

        public void ReloadSaveOnNewJourney()
        {
            if (_loadedSave == null)
            {
                Debug.LogWarning("ReloadSaveOnNewJourney called, but no save is currently loaded.");
                return;
            }

            foreach (ISaveElement saveElement in _saveElements)
            {
                if (saveElement == null) continue;
                saveElement.LoadSave(_loadedSave);
            }
        }

        public void DeleteSave(GameSaveData gameSaveData)
        {
            if (gameSaveData == null)
            {
                Debug.LogWarning("DeleteSave called with null save data.");
                return;
            }

            if (string.IsNullOrWhiteSpace(gameSaveData.SaveID))
            {
                Debug.LogWarning("DeleteSave failed because SaveID is null or empty.");
                return;
            }

            try
            {
                string savePath = GetSaveFilePath(gameSaveData.SaveID);

                if (!File.Exists(savePath))
                {
                    string legacyPath = GetLegacySaveFilePath(gameSaveData.SaveID);
                    if (File.Exists(legacyPath))
                    {
                        File.Delete(legacyPath);
                    }
                    else
                    {
                        Debug.LogWarning($"Save file not found for SaveID: {gameSaveData.SaveID}");
                    }
                }
                else
                {
                    File.Delete(savePath);
                }

                if (_loadedSave != null && _loadedSave.SaveID == gameSaveData.SaveID)
                {
                    _loadedSave = null;
                }

                ReloadSaveData();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete save: {e}");
            }
        }

        private void ReloadSaveData()
        {
            ReadAllSaveData();
            OnSaveDeleted?.Invoke();
        }

        private void ResetCurrentSave()
        {
            _loadedSave = null;
        }

        private void ReadAllSaveData()
        {
            _saves.Clear();

            if (string.IsNullOrWhiteSpace(_savePath) || !Directory.Exists(_savePath))
                return;

            string searchPattern = $"{SAVE_FILE_BASE_FORMAT}*";

            foreach (string filePath in Directory.EnumerateFiles(_savePath, searchPattern))
            {
                try
                {
                    string saveJson = File.ReadAllText(filePath);

                    if (string.IsNullOrWhiteSpace(saveJson))
                    {
                        Debug.LogWarning($"Skipped empty save file: {filePath}");
                        continue;
                    }

                    GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(saveJson);

                    if (gameSaveData == null)
                    {
                        Debug.LogWarning($"Failed to deserialize save file: {filePath}");
                        continue;
                    }


                    _saves.Add(gameSaveData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error during game load on file {filePath}: {e}");
                }
            }

            SortSavesByDateDescending();
        }

        public void ForceDeleteSaveSouvenirData(int id)
        {
            try
            {
                if (_loadedSave?.CollectedSouvenirIdList == null)
                    return;

                _loadedSave.CollectedSouvenirIdList.Remove(id);
            }
            catch (Exception e)
            {
                Debug.LogError($"Błąd usunięcia itemu z zapisu: {e}");
            }
        }

        public void RegisterToSaveSystem(ISaveElement saveElement)
        {
            if (saveElement == null)
                return;

            if (_saveElements.Contains(saveElement))
                return;

            _saveElements.Add(saveElement);
        }

        private string GetSaveFilePath(string saveId)
        {
            return Path.Combine(_savePath, $"{SAVE_FILE_BASE_FORMAT}{saveId}{SAVE_FILE_EXTENSION}");
        }

        private string GetLegacySaveFilePath(string saveId)
        {
            return Path.Combine(_savePath, $"{SAVE_FILE_BASE_FORMAT}{saveId}");
        }

        private void AddOrReplaceSaveInCache(GameSaveData saveData)
        {
            if (saveData == null || string.IsNullOrWhiteSpace(saveData.SaveID))
                return;

            int existingIndex = _saves.FindIndex(s => s != null && s.SaveID == saveData.SaveID);

            if (existingIndex >= 0)
            {
                _saves[existingIndex] = saveData;
            }
            else
            {
                _saves.Add(saveData);
            }
        }

        private void SortSavesByDateDescending()
        {
            _saves.Sort((a, b) =>
            {
                string aDate = a?.DateSaved ?? string.Empty;
                string bDate = b?.DateSaved ?? string.Empty;
                return string.Compare(bDate, aDate, StringComparison.Ordinal);
            });
        }

        private GameSaveData CreateSaveDataFromSnapshot(PreviousDaySnapshot snapshot)
        {
            GameSaveData gameSaveData = new GameSaveData
            {
                SaveID = _loadedSave != null ? _loadedSave.SaveID : Guid.NewGuid().ToString(),
                DateSaved = snapshot.DateSaved,
                CurrentDay = snapshot.CurrentDay,
                HarmonyStatus = snapshot.HarmonyStatus,
                TicketsAccepted = snapshot.TicketsAccepted,
                TicketsRejected = snapshot.TicketsRejected,
                CollectedSouvenirIdList = snapshot.CollectedSouvenirIdList != null
                    ? new List<int>(snapshot.CollectedSouvenirIdList)
                    : new List<int>(),
                PassengerEntries = snapshot.PassengerEntries != null
                    ? new List<PassengerEntry>(snapshot.PassengerEntries)
                    : new List<PassengerEntry>()
            };

            return gameSaveData;
        }

        private void OnDestroy()
        {
            _saveElements.Clear();
            GameStateMachine.OnMenuReturned -= ResetCurrentSave;
        }
    }
}