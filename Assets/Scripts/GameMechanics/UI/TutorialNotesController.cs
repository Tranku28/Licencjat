using System.Linq;
using System.Collections.Generic;
using GameMechanics.UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Core;
using System;

public class TutorialNotesController : UIElement
{
    [SerializeField] private TutorialEntriesContainer tutorialEntriesContainer;
    [SerializeField] private List<TMP_Text> textFields = new();
    [SerializeField] private Button nextPageButton, previousPageButton;
    [SerializeField] private Image stamp;

    private int _entriesPerPage;
    private List<TutorialEntry> tutorialEntries = new();
    private int _currentPage;
    private int _pageCount;
    private UnityAction _nextAction, _previousAction;

    private void Awake()
    {
        stamp.enabled = false;

        tutorialEntries.Clear();
        if (tutorialEntriesContainer != null && tutorialEntriesContainer.tutorialEntries != null)
            tutorialEntries.AddRange(tutorialEntriesContainer.tutorialEntries);

        _entriesPerPage = Mathf.Max(0, textFields?.Count ?? 0);

        RecalculatePageCount();

        _currentPage = 0;
        DisplayPage(_currentPage);
        
        _nextAction = () => FlipPage(1);
        _previousAction = () => FlipPage(-1);
    }

    private void OnEnable()
    {
        if (nextPageButton != null) nextPageButton.onClick.AddListener(_nextAction);
        if (previousPageButton != null) previousPageButton.onClick.AddListener(_previousAction);
        UpdateButtons();
    }

    private void OnDisable()
    {
        if (nextPageButton != null) nextPageButton.onClick.RemoveListener(_nextAction);
        if (previousPageButton != null) previousPageButton.onClick.RemoveListener(_previousAction);
    }

    private void RecalculatePageCount()
    {
        if (_entriesPerPage <= 0 || tutorialEntries.Count <= 0)
        {
            _pageCount = 0;
            return;
        }

        _pageCount = (tutorialEntries.Count + _entriesPerPage - 1) / _entriesPerPage;
    }

    private void FlipPage(int direction)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.creditsOpen, transform.position);

        if (_pageCount <= 0) return;

        _currentPage += direction;
        _currentPage = ((_currentPage % _pageCount) + _pageCount) % _pageCount;

        DisplayPage(_currentPage);
        UpdateButtons();
    }

    public void DisplayPage(int index)
    {
        if (_entriesPerPage <= 0) return;

        if (_pageCount == 0)
        {
            for (int i = 0; i < _entriesPerPage; i++)
                textFields[i].text = string.Empty;
            return;
        }

        index = Mathf.Clamp(index, 0, _pageCount - 1);

        int entryListStartPoint = index * _entriesPerPage;
        int remaining = Mathf.Max(0, tutorialEntries.Count - entryListStartPoint);
        int countThisPage = Mathf.Min(_entriesPerPage, remaining);

        for (int i = 0; i < countThisPage; i++)
        {
            textFields[i].text = tutorialEntries[entryListStartPoint + i].tutorialEntryText;
        }

        for (int i = countThisPage; i < _entriesPerPage; i++)
        {
            textFields[i].text = string.Empty;
        }
    }

    private void UpdateButtons()
    {
        if (nextPageButton == null || previousPageButton == null) return;

        bool canFlip = _pageCount > 1;

        nextPageButton.interactable = canFlip;
        previousPageButton.interactable = canFlip;
    }
}
