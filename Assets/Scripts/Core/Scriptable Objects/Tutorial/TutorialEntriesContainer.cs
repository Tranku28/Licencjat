using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialEntriesContainer", menuName = "Scriptable Objects/TutorialEntriesContainer")]
public class TutorialEntriesContainer : ScriptableObject
{
    public List<TutorialEntry> tutorialEntries = new();

    public IEnumerable<GameObject> Range { get; set; }

}

[Serializable]
public struct TutorialEntry
{
        [SerializeField] public int id;
        [TextArea(8, 20)]
        [SerializeField] public string tutorialEntryText;
}
