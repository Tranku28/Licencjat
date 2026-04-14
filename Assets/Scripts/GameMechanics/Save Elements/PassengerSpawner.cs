using System.Collections.Generic;
using Core;
using Core.Scriptable_Objects;
using Interactions;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour, ISaveElement
{
    [SerializeField] private List<PassengerData> passengerDataList = new();
    private List<int> spawnedDays = new();
    private List<Passenger> currentPassengers = new();

    private void Awake()
    {
        (this as ISaveElement).Register(this);
    }

    public void LoadSave(GameSaveData gameSaveData)
    {
        foreach(PassengerData passengerData in passengerDataList)
        {
            if (spawnedDays.Contains(passengerData.dayAppears)) continue;

            if (passengerData.dayAppears == gameSaveData.CurrentDay)
            {
                Passenger passenger = Instantiate(passengerData.prefab).GetComponent<Passenger>();
                currentPassengers.Add(passenger);
                spawnedDays.Add(passengerData.dayAppears);
            }
        }

        foreach (Passenger passenger in currentPassengers)
        {
            if (passenger.PassengerData.dayAppears != gameSaveData.CurrentDay)
            {
                passenger.gameObject.SetActive(false);
                continue;
            }

            passenger.ResetInteractable();
            passenger.gameObject.SetActive(true);
        }
    }

    public void SaveData(GameSaveData gameSaveData)
    {
        
    }
}
