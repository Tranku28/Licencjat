using System;
using System.Collections.Generic;

[Serializable]
public class PreviousDaySnapshot
{
    public int SaveIndex;
    public string DateSaved;
    public int CurrentDay;
    public int HarmonyStatus;
    public int TicketsAccepted;
    public int TicketsRejected;
    public List<int> CollectedSouvenirIdList;
    public List<PassengerEntry> PassengerEntries;
}