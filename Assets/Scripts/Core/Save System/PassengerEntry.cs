using System;

[Serializable]
public struct PassengerEntry
{
    public string GeneralEntry;
    public string EncounterEntry;
    public int ID;

    public PassengerEntry(string generalEntry, string encounterEntry, int id)
    {
        GeneralEntry = generalEntry;
        EncounterEntry = encounterEntry;
        ID = id;
    }
}