using System;

[Serializable]
public struct PassengerEntry
{
    public string PassengerName;
    public string GeneralEntry;
    public string EncounterEntry;
    public int ID;

    public PassengerEntry(string passengerName, string generalEntry, string encounterEntry, int id)
    {
        PassengerName = passengerName;
        GeneralEntry = generalEntry;
        EncounterEntry = encounterEntry;
        ID = id;
    }
}