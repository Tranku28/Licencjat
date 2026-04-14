using System;
using System.Collections.Generic;
using FMOD;

[Serializable]
public class GameSaveData
{
    public string SaveID;
    public string DateSaved;
    public int CurrentDay = 1;
    public int HarmonyStatus = 100;
    public int TicketsAccepted;
    public int TicketsRejected;
    public List<int> CollectedSouvenirIdList = new();
    public List<PassengerEntry> PassengerEntries = new();
    public PreviousDaySnapshot LastDayData;
}