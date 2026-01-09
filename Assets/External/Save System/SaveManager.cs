using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace External.Save_System
{
    public class SaveManager : MonoBehaviour
    {
        public SaveManager Instance {get; private set;}

        private List<ISaveSystemElement> _saveSystemElements = new();
        private SaveData _saveData;
        [SerializeField] private Button saveButton;
        [SerializeField] private TMP_Text dayText;
        [SerializeField] private TMP_Text harmonyText;

        public bool useEncryption;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            _saveData = new SaveData(1, 75);
            _saveSystemElements = FindObjectsOfType<MonoBehaviour>().OfType<ISaveSystemElement>().ToList();
            
            saveButton.onClick.AddListener(SaveGame);
        }

        private void OnDestroy()
        {
            saveButton.onClick.RemoveListener(SaveGame);
        }

        private void NewGame()
        {
            
        }

        private void LoadGame()
        {
            
        }

        private void SaveGame()
        {
            Debug.Log(_saveSystemElements.Count);
            foreach (var saveSystemElement in _saveSystemElements)
            {
                saveSystemElement.SaveData(_saveData);
            }
            
            try
            {
                string saveString = JsonUtility.ToJson(_saveData, true);

                if (useEncryption)
                    saveString = EncryptDecrypt(saveString);
                
                string fullPath = Application.persistentDataPath + "/SaveData.json";
                Directory.CreateDirectory(Application.persistentDataPath + "GameSaveData");
                
                FileStream fileStream = new FileStream(fullPath, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(saveString);
                //ALWAYS REMEMBER TO CLOSE TO AVOID MEMORY LEAKS
                streamWriter.Close();
                
                //Nie trzeba zamykać StreamWriter jeśli użyjemy using
                /*
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    StreamWriter streamWriter = new StreamWriter(fileStream);
                    streamWriter.Write(saveString);
                }
                */
            }
            catch (Exception e)
            {
                Debug.LogError("Error while saving game " + e);
            }
        }

        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            string encryptionKey = "hello";
            
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)data[i] ^ encryptionKey[i %  encryptionKey.Length];
            }
            
            return modifiedData;
        }
    }
}