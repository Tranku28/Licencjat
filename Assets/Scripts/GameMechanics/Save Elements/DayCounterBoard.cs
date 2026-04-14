using System;
using Core;
using TMPro;
using UnityEngine;

public class DayCounterBoard : MonoBehaviour, ISaveElement
{
    [SerializeField] private TMP_Text dayDisplay;
    private int _currentDay;

    private void Awake()
    {
        (this as ISaveElement).Register(this);
        GameStateMachine.OnMenuReturned += EraseDayCounter;
    }

    public void LoadSave(GameSaveData gameSaveData)
    {
        _currentDay = gameSaveData.CurrentDay == 0 ? 1 : gameSaveData.CurrentDay;
        dayDisplay.text = _currentDay.ToString();
    }

    public void SaveData(GameSaveData gameSaveData)
    {
        
    }

    private void OnDestroy()
    {
        GameStateMachine.OnMenuReturned -= EraseDayCounter;
    }

    private void EraseDayCounter()
    {
        dayDisplay.text = string.Empty;
    }
}
