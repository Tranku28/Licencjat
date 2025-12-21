using System;
using GameMechanics.DayHandling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMechanics.UI
{
    public class NewspaperHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text newspaperContent;
        [SerializeField] private Button nextDayButton;

        private void OnEnable()
        {
            nextDayButton.onClick.AddListener(ProceedToNextDay);
        }

        private void OnDisable()
        {
            nextDayButton.onClick.RemoveListener(ProceedToNextDay);
        }

        private void Start()
        {
            DayHandler.OnNewspaperLoadNews += LoadNewspaperContent;
        }

        private void OnDestroy()
        {
            DayHandler.OnNewspaperLoadNews -= LoadNewspaperContent;
        }

        private void LoadNewspaperContent(string content)
        {
            newspaperContent.text = content;
        }

        private void ProceedToNextDay()
        {
            Debug.Log("ProceedToNextDay");
        }
    }
}
