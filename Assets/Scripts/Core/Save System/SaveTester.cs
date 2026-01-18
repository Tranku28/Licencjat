using System.Linq;
using Core;
using Core.Save_System;
using Core.Scriptable_Objects;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.UI;

//TODO: Scrap later
public class SaveTester : MonoBehaviour, ISaveElement
{
    [SerializeField] int saveToLoad;
    [SerializeField] GameObject prefabTest;
    public int currentDay, saveIndex, ticketsAccepted, ticketRejected;
    public int[] souvenirs = {1,2,3,4};
    public string[] diaryEntries = {"ala", "ola", "ela"};
    public string[] tutorialNotes = {"note one", "note two", "note three"};

    public Button saveButton;
    private SaveSystem _saveSystem;

    public void Awake()
    {
        Register();
        saveButton.onClick.AddListener(SaveGameInvoke);
    }

    public void OnDestroy()
    {
        saveButton.onClick.RemoveListener(SaveGameInvoke);
    }

    public void SaveGameInvoke()
    {
        _saveSystem.SaveGame();
    }

    public void LoadGameInvoke()
    {
        _saveSystem.LoadSave(saveToLoad);
    }

    public void LoadSave(GameSaveData gameSaveData)
    {
        currentDay = gameSaveData.CurrentDay;
        souvenirs = gameSaveData.CollectedSouvenirIdList.ToArray();
    }

    public void Register()
    {
        _saveSystem = DependencyResolver.Instance.GetType<SaveSystem>();

        if (!_saveSystem) 
        {
            Debug.LogError("SaveSystem does not exists");
            return;
        }

        _saveSystem.RegisterToSaveSystem(this);
        Debug.Log("Registered to save system");
    }

    public void SaveData(GameSaveData gameSaveData)
    {
        gameSaveData.CurrentDay = currentDay;
        gameSaveData.CollectedSouvenirIdList = souvenirs;
        gameSaveData.DiaryEntries = diaryEntries;
        gameSaveData.SaveIndex = saveIndex;
        gameSaveData.TicketsAccepted = ticketsAccepted;
        gameSaveData.TicketsRejected = ticketRejected;
        gameSaveData.TutorialNotes = tutorialNotes;
    }
}
