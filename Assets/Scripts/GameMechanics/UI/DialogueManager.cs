using System;
using System.Collections.Generic;
using Core.Scriptable_Objects;
using Ink.Runtime;
using Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMechanics.UI
{
    public class DialogueManager : UIElement
    {
        [SerializeField] TMP_Text displayedText;
        [SerializeField] GameObject choiceContainer;
        [SerializeField] private float timeBetweenChars;
        
        [SerializeField] private Image playerNameBackground, npcNameBackground;
        [SerializeField] private TMP_Text playerNameText, npcNameText;

        [SerializeField] private TicketMinigame ticketMinigame;
        [SerializeField] private Button showTicketButton;

        private PassengerData _currentPassengerData;
        
        private int _currentSpeakerIndex;
        
        private readonly Dictionary<Button, TMP_Text> _choicesToDisplay = new();
        
        private Story _story;

        private bool _clickedBeforeChoices = false;

        private bool _ticketScanned, _ticketRejected = false;

        public bool ticketScanned => _ticketScanned;
        public bool ticketRejected => _ticketRejected;

        public static event EventHandler<DialogueEndedEventArgs> OnDialogueEnded;

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
            PlayerControls.OnSingleClickEvent += StoryHop;
            
            showTicketButton.onClick.AddListener(ToggleTicketDisplay);
        }

        private void OnDisable()
        {
            Passenger.OnPassengerInteracted -= LoadTicketDataAndStart;
            PlayerControls.OnSingleClickEvent -= StoryHop;
            
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

        private void LoadTicketDataAndStart(PassengerData passengerData)
        {
            ticketMinigame.UpdateTicketUI(passengerData);
            npcNameText.text = passengerData.passengerName;
            _ticketScanned = false;
            _ticketRejected = false;
            
            StartStory(passengerData);
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
                Debug.Log("rejected");
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

            if (!_story.canContinue && _story.currentChoices.Count == 0)
            {
                Debug.Log("The end of story");
            }
        }

        private void UpdateNameDisplays(int index)
        {
            switch (index)
            {
                case 0:
                    playerNameBackground.enabled = true;
                    playerNameText.enabled = true;
                    npcNameBackground.enabled = false;
                    npcNameText.enabled = false;
                    break;
                case 1:
                    playerNameBackground.enabled = false;
                    playerNameText.enabled = false;
                    npcNameBackground.enabled = true;
                    npcNameText.enabled = true;
                    break;
                default:
                    playerNameBackground.enabled = false;
                    playerNameText.enabled = false;
                    npcNameBackground.enabled = false;
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
            OnDialogueEnded?.Invoke(
                this,
                new DialogueEndedEventArgs(
                    _ticketScanned,
                    _ticketRejected,
                    _currentPassengerData)
            );
        }
    }

    public class DialogueEndedEventArgs : EventArgs
    {
        public bool ticketScanned;
        public bool ticketRejected;
        public PassengerData passengerData;

        public DialogueEndedEventArgs(bool scanned, bool rejected, PassengerData passenger)
        {
            passengerData = passenger;
            ticketScanned = scanned;
            ticketRejected = rejected;
        }
    }
}
