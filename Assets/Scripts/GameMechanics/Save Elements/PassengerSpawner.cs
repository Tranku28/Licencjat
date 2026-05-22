using System.Collections.Generic;
using Core.Scriptable_Objects;
using Interactions;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour, ISaveElement
{
    [SerializeField] private List<PassengerData> passengerDataList = new();
    private List<Passenger> _allPassengers = new();

    private void Awake()
    {
        (this as ISaveElement).Register(this);

        InstantiatePassengers();
    }

    public void LoadSave(GameSaveData gameSaveData)
    {
        EnablePassengers(gameSaveData.CurrentDay);
    }

    public void SaveData(GameSaveData gameSaveData)
    {
        
    }

    private void InstantiatePassengers()
    {
        foreach (PassengerData data in passengerDataList)
        {
            Passenger passenger = Instantiate(data.prefab).GetComponent<Passenger>();
            _allPassengers.Add(passenger);
            passenger.gameObject.SetActive(false);
        }
    }

    private void EnablePassengers(int day)
    {
        foreach(Passenger passenger in _allPassengers)
        {
            bool passengerStay = passenger.StaysNextDay && passenger.PassengerData.dayAppears + 1 == day;

            if (passenger.PassengerData.dayAppears == day || passengerStay)
            {
                passenger.gameObject.SetActive(true);
                continue;
            }

            passenger.gameObject.SetActive(false);
        }
    }
}
