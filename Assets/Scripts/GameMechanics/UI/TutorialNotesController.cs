using System.Linq;
using System.Collections.Generic;
using GameMechanics.UI;
using UnityEngine;
using TMPro;
using System;

public class TutorialNotesController : UIElement
{
    [SerializeField] private TutorialEntriesContainer tutorialEntriesContainer;
    [SerializeField] private TMP_Text[] tutorialEntriesTextArray = new TMP_Text[4];
    private List<TutorialEntry> tutorialEntries = new();

    void Awake()
    {
        InitializeStartingTutorialNotes();
    }

    private void InitializeStartingTutorialNotes()
    {
        tutorialEntries.AddRange(tutorialEntriesContainer.tutorialEntries.Take(4).ToList());

        for (int i=0; i < tutorialEntriesTextArray.Length; i++)
        {
            try
            {
                tutorialEntriesTextArray[i].text = tutorialEntries[i].tutorialEntryText;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}
