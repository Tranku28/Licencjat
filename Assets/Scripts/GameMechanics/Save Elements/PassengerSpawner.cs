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
        foreach(Passenger passenger in _allPassengers)
        {
            if (passenger.PassengerData.dayAppears == gameSaveData.CurrentDay)
            {
                passenger.StaysNextDay = false;
                passenger.SetBarking(false);
                passenger.gameObject.SetActive(true);
                continue;
            }

            bool passengerStay = gameSaveData.PassengerStayNames.Contains(passenger.PassengerData.passengerName);

            if (passengerStay)
            {
                passenger.StaysNextDay = false;
                passenger.SetBarking(true);
                passenger.gameObject.SetActive(true);
                gameSaveData.PassengerStayNames.Remove(passenger.PassengerData.passengerName);
                continue;
            }

            passenger.gameObject.SetActive(false);
        }
    }

    public void SaveData(GameSaveData gameSaveData)
    {
        foreach (Passenger passenger in _allPassengers)
        {
            if (passenger.StaysNextDay)
            {
                Debug.Log("Passenger name saved");
                gameSaveData.PassengerStayNames.Add(passenger.PassengerData.passengerName);
            }
        }
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
}
