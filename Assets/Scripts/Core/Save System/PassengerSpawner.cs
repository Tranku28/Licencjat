using System.Collections.Generic;
using Core.Scriptable_Objects;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour, ISaveElement
{
    [SerializeField] private List<PassengerData> passengerDataList = new();

    public void LoadSave(GameSaveData gameSaveData)
    {
        foreach(PassengerData passengerData in passengerDataList)
        {
            if (passengerData.dayAppears == gameSaveData.CurrentDay)
            {
                Instantiate(passengerData.prefab);
            }
        }
    }

    public void SaveData(GameSaveData gameSaveData)
    {
        // Empty, becase it only loads
    }
}
