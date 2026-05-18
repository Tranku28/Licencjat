using System;
using UnityEngine;

[Serializable]
public struct PassengerEntry
{
    public string PassengerName;
    [TextArea(3, 5)]
    public string Entry;

    public PassengerEntry(string passengerName, string generalEntry)
    {
        PassengerName = passengerName;
        Entry = generalEntry;
    }
}