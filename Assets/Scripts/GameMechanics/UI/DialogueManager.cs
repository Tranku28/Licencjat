using System;
using System.Collections.Generic;
using Core;
using Core.Save_System;
using Core.Scriptable_Objects;
using Ink.Runtime;
using Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMechanics.UI
{
    public class DialogueManager : UIElement, IPointerClickHandler
    {
        [SerializeField] TMP_Text displayedText;
        [SerializeField] GameObject choiceContainer;
        [SerializeField] private float timeBetweenChars;
        [SerializeField] private BlinkPanelUI blinkPanelUI;
        
        [SerializeField] private TMP_Text playerNameText, npcNameText;

        [SerializeField] private TicketMinigame ticketMinigame;
        [SerializeField] private Button showTicketButton;

        private Passenger _currentPassenger;

        private PassengerData _currentPassengerData;
        
        private int _currentSpeakerIndex;
        
        private readonly Dictionary<Button, TMP_Text> _choicesToDisplay = new();
        
        private Story _story;

        private bool _clickedBeforeChoices = false;

        private bool _ticketScanned, _ticketRejected;

        public bool ticketScanned => _ticketScanned;
        public bool ticketRejected => _ticketRejected;

        private Awaitable _dialogueAwaitable;

        private void Awake()
        {
            Button[] buttons = choiceContainer.GetComponentsInChildren<Button>();

            foreach (Button button in buttons)
            {
                TMP_Text buttonTexts = button.GetComponentInChildren<TMP_Text>();
                
                _choicesToDisplay.Add(button, buttonTexts);
            }
        }

        private void OnEnable()
        {
            Passenger.OnPassengerInteracted += LoadTicketDataAndStart;
            
            showTicketButton.onClick.AddListener(ToggleTicketDisplay);
        }

        private void OnDisable()
        {
            Passenger.OnPassengerInteracted -= LoadTicketDataAndStart;
            
            showTicketButton.onClick.RemoveAllListeners();
        }

        private void Start()
        {
            ticketMinigame.OnTicketScanned += SetScanned;
        }

        private void OnDestroy()
        {
            ticketMinigame.OnTicketScanned -= SetScanned;
        }

        private void SetScanned()
        {
            _ticketScanned = true;
        }

        private void LoadTicketDataAndStart(object sender, PassengerInteractedEventArgs passengerArgs)
        {
            _currentPassenger = sender as Passenger;
            
            _currentPassengerData = passengerArgs.PassengerData;
            ticketMinigame.UpdateTicketUI(passengerArgs.PassengerData);
            npcNameText.text = passengerArgs.PassengerData.passengerName;
            _ticketScanned = false;
            _ticketRejected = false;
            
            StartStory(passengerArgs.PassengerData);
        }
        
        private void StartStory(PassengerData data)
        {
            _story = new Story(data.inkJSON.text);
            string text = _story.Continue().Trim();
            displayedText.text = text;
            
            _story.ObserveVariable("speakerIndex", (string varName, object newValue) => {
                UpdateNameDisplays((int)newValue);
            });
            
            _story.ObserveVariable("canScan", (string varName, object newValue) =>
            {
                SetScannable((bool)newValue);
            });
            
            _story.ObserveVariable("ticketRejected", (string varName, object newValue) =>
            {
                Debug.Log(_ticketRejected);
                _ticketRejected = (bool)newValue;
            });
        }

        private void StoryHop()
        {
            if (_story == null) return;
            
            if (_story.canContinue)
            {
                string text = _story.Continue().Trim();
                displayedText.text = text;
                displayedText.enabled = true;
                
                HideChoices();
            }
            
            if (_story.currentChoices.Count > 0)
            {
                if (!_clickedBeforeChoices)
                {
                    _clickedBeforeChoices = true;
                    return;
                }
                
                ShowChoices();
            }
        }

        private void UpdateNameDisplays(int index)
        {
            switch (index)
            {
                case 0:
                    playerNameText.enabled = true;
                    npcNameText.enabled = false;
                    break;
                case 1:
                    playerNameText.enabled = false;
                    npcNameText.enabled = true;
                    break;
                default:
                    playerNameText.enabled = false;
                    npcNameText.enabled = false;
                    break;
            }
        }
        
        private void SetScannable(bool value) => ticketMinigame.canScan = value;
        
        private void ShowChoices()
        {
            _clickedBeforeChoices = false;
            displayedText.enabled = false;
            
            choiceContainer.SetActive(true);

            int index = 0;

            foreach (var pair in _choicesToDisplay)
            {
                Button button = pair.Key;
                TMP_Text text = pair.Value;

                if (index < _story.currentChoices.Count)
                {
                    button.gameObject.SetActive(true);
                    button.onClick.RemoveAllListeners();

                    int capturedIndex = index;
                    button.onClick.AddListener(() => MakeChoice(capturedIndex));

                    text.text = _story.currentChoices[index].text;
                }
                else
                {
                    button.onClick.RemoveAllListeners();
                    button.gameObject.SetActive(false);
                }

                index++;
            }
        }


        private void HideChoices()
        {
            foreach (var pair in _choicesToDisplay)
            {
                Button button = pair.Key;
                
                button.onClick.RemoveAllListeners();
                button.gameObject.SetActive(false);
            }

            choiceContainer.SetActive(false);
        }


        private void MakeChoice(int index)
        {
            HideChoices();
            
            _story.ChooseChoiceIndex(index);
            
            StoryHop();
        }

        
        private void ToggleTicketDisplay() => ticketMinigame.ShowUI();

        public void HideTicketDisplay() => ticketMinigame.HideUI();

        public void OnDialogueQuit()
        {
            _dialogueAwaitable = AwaitableDialogueQuit();
        }
        
        private async Awaitable AwaitableDialogueQuit()
        {
            SaveSystem saveSystem = DependencyResolver.Instance.GetType<SaveSystem>();
            
            if (!_ticketRejected)
            {
                saveSystem.ticketsAccepted++;
                saveSystem.souvenirIDs.Add(_currentPassengerData.souvenirData.souvenirID);
                saveSystem.diaryEntries.Add(_currentPassengerData.diaryContent);
                saveSystem.SaveToJson();
                return;
            }
                
            _currentPassenger.gameObject.GetComponent<Collider>().enabled = false;
            
            saveSystem.ticketsRejected++;
            saveSystem.SaveToJson();

            await blinkPanelUI.ClosePlayerEyes();
            Destroy(_currentPassenger.gameObject);
            await Awaitable.WaitForSecondsAsync(1);
            await blinkPanelUI.OpenPlayerEyes();

            _dialogueAwaitable = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<TMP_Text>() == displayedText)
                StoryHop();
        }
    }
}
