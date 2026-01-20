using System.Collections.Generic;
using Core.Scriptable_Objects;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour, ISaveElement
{
    [SerializeField] private List<PassengerData> passengerDataList = new();

    private void Awake()
    {
        (this as ISaveElement).Register(this);
    }

    public void LoadSave(GameSaveData gameSaveData)
    {
        foreach(PassengerData passengerData in passengerDataList)
        {
            Debug.Log("Spawning NPC's");
            if (passengerData.dayAppears == gameSaveData.CurrentDay)
            {
                GameObject passenger = Instantiate(passengerData.prefab);
                passenger.transform.SetParent(transform);
            }
        }
    }

    public void SaveData(GameSaveData gameSaveData)
    {
        // Empty, becase it only loads
    }
}
