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
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Button nextPageButton, previousPageButton;
    [SerializeField] private Image stamp;
    private List<TutorialEntry> _tutorialEntries = new();
    private int _currentPage;
    private int _pageCount;
    private UnityAction _nextAction, _previousAction;

    private void Awake()
    {
        stamp.enabled = false;

        _tutorialEntries.Clear();
        if (tutorialEntriesContainer != null && tutorialEntriesContainer.tutorialEntries != null)
            _tutorialEntries.AddRange(tutorialEntriesContainer.tutorialEntries);

        _pageCount = _tutorialEntries.Count;
        _currentPage = 0;
        DisplayPage(_currentPage);

        _nextAction = () => FlipPage(1);
        _previousAction = () => FlipPage(-1);
    }

    private void OnEnable()
    {
        if (nextPageButton != null) nextPageButton.onClick.AddListener(_nextAction);
        if (previousPageButton != null) previousPageButton.onClick.AddListener(_previousAction);
    }

    private void OnDisable()
    {
        if (nextPageButton != null) nextPageButton.onClick.RemoveListener(_nextAction);
        if (previousPageButton != null) previousPageButton.onClick.RemoveListener(_previousAction);
    }

    private void FlipPage(int direction)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.creditsOpen, transform.position);

        if (_pageCount <= 0) return;

        _currentPage += direction;
        _currentPage = ((_currentPage % _pageCount) + _pageCount) % _pageCount;

        DisplayPage(_currentPage);
    }

    public void DisplayPage(int index)
    {
        _currentPage = index;
        textField.text = _tutorialEntries[_currentPage].tutorialEntryText;
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        int entriesCount = _tutorialEntries.Count-1;

        if (_currentPage == 0)
        {
            stamp.enabled = false;
            previousPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(true);
            return;
        }

        if (_currentPage == entriesCount)
        {
            stamp.enabled = true;
            nextPageButton.gameObject.SetActive(false);
            previousPageButton.gameObject.SetActive(true);
            return;
        }
        
        stamp.enabled = false;
        nextPageButton.gameObject.SetActive(true);
        previousPageButton.gameObject.SetActive(true);
    }
}
