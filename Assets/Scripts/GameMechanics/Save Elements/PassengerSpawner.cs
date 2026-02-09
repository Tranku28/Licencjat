using System.Collections.Generic;
using Core.Scriptable_Objects;
using Interactions;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour, ISaveElement
{
    [SerializeField] private List<PassengerData> passengerDataList = new();
    private List<Passenger> currentPassengers = new();

    private void Awake()
    {
        (this as ISaveElement).Register(this);
    }

    void OnDestroy()
    {
        currentPassengers.Clear();
    }

    public void LoadSave(GameSaveData gameSaveData)
    {
        foreach(PassengerData passengerData in passengerDataList)
        {
            if (passengerData.dayAppears == gameSaveData.CurrentDay)
            {
                Passenger passenger = Instantiate(passengerData.prefab).GetComponent<Passenger>();
                currentPassengers.Add(passenger);
            }
        }
    }

    public void SaveData(GameSaveData gameSaveData)
    {
        //TODO: Refactor save logic
        currentPassengers.ForEach(p =>
        {
            Destroy(p.gameObject);
        });

        currentPassengers.Clear();
    }
}
