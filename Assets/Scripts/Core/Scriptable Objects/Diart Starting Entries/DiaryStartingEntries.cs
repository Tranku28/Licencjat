using UnityEngine;

[CreateAssetMenu(fileName = "DiaryStartingEntries", menuName = "Scriptable Objects/DiaryStartingEntries")]
public class DiaryStartingEntries : ScriptableObject
{
    [field: SerializeField] public PassengerEntry[] entries {get; private set;}
}
