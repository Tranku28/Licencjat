using System;
using System.Collections.Generic;
using GameMechanics.DayHandling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMechanics.UI
{
    // TODO: String content on diary pages loading
    public class DiaryController : UIElement
    {
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;
        
        [SerializeField] private TMP_Text leftPageText, rightPageText;
        
        private int _diaryPageIndex;
        
        private List<string> _diaryContents = new();

        private void Start()
        {
            DayHandler.OnDiaryAddContent += AddDiaryContent;
        }

        private void OnDestroy()
        {
            DayHandler.OnDiaryAddContent -= AddDiaryContent;
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

        private void AddDiaryContent(string content)
        {
            _diaryContents.Add(content);
            leftPageText.text = _diaryContents[0];
        }
        
        private void TurnNextPage()
        {
            Debug.Log("TurnNextPage");
        }
        
        private void TurnPreviousPage()
        {
            Debug.Log("TurnPreviousPage");
        }
    }
}