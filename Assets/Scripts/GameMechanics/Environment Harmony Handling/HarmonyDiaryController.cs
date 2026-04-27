using System;
using System.Collections.Generic;
using GameMechanics.DayHandling;
using HarmonyHandling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMechanics.UI
{
    public class HarmonyDiaryController : UIElement, ISaveElement
    {
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;
        [SerializeField] private TMP_Text leftEntryField, rightEntryField;
        [SerializeField] private TMP_Text leftPassengerNameField, rightPassengerNameField;
        [SerializeField] private DiaryHarmonyLevel[] harmonyLevels;
        private DiaryHarmonyLevel _selectedDiaryHarmonyLevel;

        private int _pageIndex;

        private void Awake()
        {
            (this as ISaveElement).Register(this);
        }

        private List<PassengerEntry> _passengerEntries = new();

        private void Start()
        {
            DialogueManager.OnDialogueQuitEvent += AddEntry;
            HarmonyIndicator.OnHarmonyValueSet += CalculateHarmonyLevelIndex;
        }

        private void OnDestroy()
        {
            DialogueManager.OnDialogueQuitEvent -= AddEntry;
            HarmonyIndicator.OnHarmonyValueSet -= CalculateHarmonyLevelIndex;
        }

        private void OnEnable()
        {
            nextPageButton.onClick.AddListener(TurnNextPage);
            prevPageButton.onClick.AddListener(TurnPreviousPage);
        }

        private void OnDisable()
        {
            nextPageButton.onClick.RemoveListener(TurnNextPage);
            prevPageButton.onClick.RemoveListener(TurnPreviousPage);
        }

        public void SetDefaultDiaryState()
        {
            _pageIndex = 0;
            DisplayEntry(_pageIndex);
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);

            if (_passengerEntries.Count > 1)
            {
                nextPageButton.gameObject.SetActive(true);
            }
        }
        
        public void TurnNextPage()
        {
            _pageIndex++;
            if (_pageIndex >= _passengerEntries.Count-1)
            {
                nextPageButton.gameObject.SetActive(false);
                _pageIndex = _passengerEntries.Count-1;
            }

            DisplayEntry(_pageIndex);
            prevPageButton.gameObject.SetActive(true);
        }
        
        public void TurnPreviousPage()
        {
            _pageIndex--;
            if (_pageIndex <= -1)
            {
                prevPageButton.gameObject.SetActive(false);
                _pageIndex = 0;
                return;
            }

            DisplayEntry(_pageIndex);
            nextPageButton.gameObject.SetActive(true);
        }

        private void DisplayEntry(int index)
        {
            if (_passengerEntries.Count == 0 || 
                index < 0 || 
                index >= _passengerEntries.Count)
            {
                leftEntryField.text = "";
                rightEntryField.text = "";
                return;
            }

            string leftEntryResult = TextAnimationInjector.InjectAnimatedLinks(
                text: _passengerEntries[index].GeneralEntry,
                linkTag: _selectedDiaryHarmonyLevel.LinkTag,
                minGap: _selectedDiaryHarmonyLevel.MinGap,
                maxGap: _selectedDiaryHarmonyLevel.MaxGap,
                minLength: _selectedDiaryHarmonyLevel.MinLength,
                maxLength: _selectedDiaryHarmonyLevel.MaxLength,
                seed: _selectedDiaryHarmonyLevel.Seed
            );

            string rightEntryResult = TextAnimationInjector.InjectAnimatedLinks(
                text: _passengerEntries[index].EncounterEntry,
                linkTag: _selectedDiaryHarmonyLevel.LinkTag,
                minGap: _selectedDiaryHarmonyLevel.MinGap,
                maxGap: _selectedDiaryHarmonyLevel.MaxGap,
                minLength: _selectedDiaryHarmonyLevel.MinLength,
                maxLength: _selectedDiaryHarmonyLevel.MaxLength,
                seed: _selectedDiaryHarmonyLevel.Seed
            );

            leftEntryField.text = leftEntryResult;
            leftPassengerNameField.text = _passengerEntries[index].PassengerName;
            rightEntryField.text = rightEntryResult;
            rightPassengerNameField.text = _passengerEntries[index].PassengerName;
        }

        private void CalculateHarmonyLevelIndex(int harmonyValue)
        {
            if (harmonyValue >= 75 && harmonyValue < 100)
                _selectedDiaryHarmonyLevel = harmonyLevels[0];
            if (harmonyValue >= 50 && harmonyValue < 75)
                _selectedDiaryHarmonyLevel = harmonyLevels[1];
            if (harmonyValue >= 25 && harmonyValue < 50)
                _selectedDiaryHarmonyLevel = harmonyLevels[2];
        }

        private void AddEntry(object sender, DialogueEndEventArgs e)
        {
            _passengerEntries.Add(new PassengerEntry(e.PassengerName, e.GeneralEntry, e.EncounterEntry, e.EntryId));
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

            SetDefaultDiaryState();
        }
    }
}