using TMPro;
using UnityEngine;

public class DayCounterBoard : MonoBehaviour, ISaveElement
{
    [SerializeField] private TMP_Text dayDisplay;
    private int _currentDay;

    private void Awake()
    {
        (this as ISaveElement).Register(this);
    }

    public void LoadSave(GameSaveData gameSaveData)
    {
        _currentDay = gameSaveData.CurrentDay == 0 ? 1 : gameSaveData.CurrentDay;
        dayDisplay.text = _currentDay.ToString();
    }

    public void SaveData(GameSaveData gameSaveData)
    {
        //TODO: Update it in saveSystem to have single source of truth
        _currentDay++;
        gameSaveData.CurrentDay = _currentDay;
        dayDisplay.text = _currentDay.ToString();
    }
}
