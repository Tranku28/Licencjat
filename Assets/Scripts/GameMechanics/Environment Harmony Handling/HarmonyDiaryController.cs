using System;
using System.Collections.Generic;
using Core;
using EasyTextEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameMechanics.UI
{
    public class HarmonyDiaryController : UIElement, ISaveElement
    {
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;
        [SerializeField] private TMP_Text leftEntryField, rightEntryField;
        [SerializeField] private TextEffect leftEffects, rightEffects;
        [SerializeField] private TMP_Text leftPassengerNameField, rightPassengerNameField;
        [SerializeField] private DiaryHarmonyLevel[] harmonyLevels;
        [SerializeField] private DiaryStartingEntries startingEntries;
        private DiaryHarmonyLevel _selectedHarmonyLevel;

        private int _pageIndex;

        private void Awake()
        {
            (this as ISaveElement).Register(this);
        }

        private List<PassengerEntry> _passengerEntries = new();

        private void Start()
        {
            DialogueManager.OnDialogueQuitEvent += AddEntries;
            HarmonyIndicator.OnHarmonyValueSet += CalculateHarmonyLevelIndex;

            nextPageButton.onClick.AddListener(TurnNextPage);
            prevPageButton.onClick.AddListener(TurnPreviousPage);
        }

        private void OnDestroy()
        {
            DialogueManager.OnDialogueQuitEvent -= AddEntries;
            HarmonyIndicator.OnHarmonyValueSet -= CalculateHarmonyLevelIndex;

            nextPageButton.onClick.RemoveListener(TurnNextPage);
            prevPageButton.onClick.RemoveListener(TurnPreviousPage);
        }
        
        public void SetDefaultDiaryState()
        {
            Debug.Log(_passengerEntries.Count);
            _pageIndex = 0;
            DisplayEntries(_pageIndex);
            UpdateNavigationButtons();
        }

        
        public void TurnNextPage()
        {
            Debug.Log("Turning next page");

            int nextIndex = _pageIndex + 2;

            if (nextIndex < _passengerEntries.Count)
            {
                _pageIndex = nextIndex;
            }

            DisplayEntries(_pageIndex);
            UpdateNavigationButtons();
        }

        public void TurnPreviousPage()
        {
            Debug.Log("Turning previous page");

            int prevIndex = _pageIndex - 2;

            if (prevIndex >= 0)
            {
                _pageIndex = prevIndex;
            }

            DisplayEntries(_pageIndex);
            UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            prevPageButton.gameObject.SetActive(_pageIndex > 0);
            nextPageButton.gameObject.SetActive(_pageIndex + 2 < _passengerEntries.Count);
        }


        private void DisplayEntries(int leftIndex)
        {
            ClearDiaryFields();
            leftEffects.StartManualEffects();
            rightEffects.StartManualEffects();

            if (_passengerEntries.Count == 0)
                return;

            DisplaySingleEntry(
                leftIndex,
                leftEntryField,
                leftPassengerNameField
            );

            int rightIndex = leftIndex + 1;

            if (rightIndex < _passengerEntries.Count)
            {
                DisplaySingleEntry(
                    rightIndex,
                    rightEntryField,
                    rightPassengerNameField
                );
            }
        }

        private void DisplaySingleEntry(int index, TMP_Text entryField, TMP_Text passengerNameField)
        {
            if (index < 0 || index >= _passengerEntries.Count)
                return;

            PassengerEntry entry = _passengerEntries[index];

            string entryText = entry.Entry;

            if (_selectedHarmonyLevel is not null)
            {
                entryText = TextAnimationInjector.InjectAnimatedTexts
                (
                    entry.Entry,
                    _selectedHarmonyLevel.LinkTag,
                    _selectedHarmonyLevel.MinGap,
                    _selectedHarmonyLevel.MaxGap,
                    _selectedHarmonyLevel.MinLength,
                    _selectedHarmonyLevel.MaxLength,
                    Random.Range(0, 20)
                );
            }

            entryField.text = entryText;
            passengerNameField.text = entry.PassengerName;
        }

        private void ClearDiaryFields()
        {
            leftEntryField.text = "";
            rightEntryField.text = "";

            leftPassengerNameField.text = "";
            rightPassengerNameField.text = "";
        }

        private void AddEntries(object sender, DialogueEndEventArgs e)
        {
            foreach (PassengerEntry entry in e.Entries)
            {
                _passengerEntries.Add(entry);
            }
        }

        private void AddEntries(PassengerEntry[] entries)
        {
            foreach (PassengerEntry entry in entries)
            {
                _passengerEntries.Add(entry);
            }
        }

        public void SaveData(GameSaveData gameSaveData)
        {
            gameSaveData.PassengerEntries.Clear();
            List<PassengerEntry> passengerEntries = new();
            passengerEntries.AddRange(_passengerEntries);
            gameSaveData.PassengerEntries.AddRange(passengerEntries);
        }

        public void LoadSave(GameSaveData gameSaveData)
        {
            _passengerEntries.Clear();
            _passengerEntries.AddRange(gameSaveData.PassengerEntries);

            if (gameSaveData.CurrentDay == 1)
            {
                AddEntries(startingEntries.entries);
            }

            SetDefaultDiaryState();
        }

        private void CalculateHarmonyLevelIndex(int harmonyValue)
        {
            if (harmonyValue >= 100)
            {
                _selectedHarmonyLevel = null;
            }
            else if (harmonyValue >= 75)
            {
                _selectedHarmonyLevel = harmonyLevels[0];
            }
            else if (harmonyValue >= 50)
            {
                _selectedHarmonyLevel = harmonyLevels[1];
            }
            else if (harmonyValue >= 25)
            {
                _selectedHarmonyLevel = harmonyLevels[2];
            }
            else
            {
                _selectedHarmonyLevel = harmonyLevels.Length > 3 
                    ? harmonyLevels[3] 
                    : harmonyLevels[^1];
            }
        }
    }
}