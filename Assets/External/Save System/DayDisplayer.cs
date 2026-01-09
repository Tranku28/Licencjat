using System;
using TMPro;
using UnityEngine;

namespace External.Save_System
{
    public class DayDisplayer : MonoBehaviour, ISaveSystemElement
    {
        private TMP_Text _currentDayText;

        private void Awake()
        {
            _currentDayText = GetComponent<TMP_Text>();
        }

        public void LoadData(SaveData saveData)
        {
            _currentDayText.text = saveData.day.ToString();
        }

        public void SaveData(SaveData saveData)
        {
            saveData.day = int.Parse(_currentDayText.text);
        }
    }
}