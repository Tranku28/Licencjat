using System;
using System.Collections.Generic;
using GameMechanics.DayHandling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMechanics.UI
{
    public class DiaryController : UIElement, ISaveElement
    {
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;
        [SerializeField] private TMP_Text leftEntryField, rightEntryField;

        private int _pageIndex;

        private void Awake()
        {
            (this as ISaveElement).Register(this);
        }

        private List<PassengerEntry> _passengerEntries = new();

        private void Start()
        {
            DialogueManager.OnDialogueQuitEvent += AddEntry;
        }

        private void OnDestroy()
        {
            DialogueManager.OnDialogueQuitEvent += AddEntry;
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
            if (_passengerEntries.Count == 0) return;
            leftEntryField.text = _passengerEntries[index].GeneralEntry;
            rightEntryField.text = _passengerEntries[index].EncounterEntry;
        }

        private void AddEntry(object sender, DialogueEndEventArgs e)
        {
            _passengerEntries.Add(new PassengerEntry(e.GeneralEntry, e.EncounterEntry));
        }

        public void SaveData(GameSaveData gameSaveData)
        {
            gameSaveData.PassengerEntries.AddRange(_passengerEntries);

            _passengerEntries.Clear();
        }

        public void LoadSave(GameSaveData gameSaveData)
        {
            _passengerEntries.AddRange(gameSaveData.PassengerEntries);

            gameSaveData.PassengerEntries.Clear();
        }
    }
}