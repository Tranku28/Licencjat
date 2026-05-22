using System;
using System.Collections.Generic;
using Core.Scriptable_Objects;
using DG.Tweening;
using GameMechanics.Interactions;
using Ink.Runtime;
using Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameMechanics.UI
{
    //TODO: Break down class into smaller manageable pieces
    public class DialogueEndEventArgs : EventArgs
    {
        public string PassengerName;
        public string PassengerAction;
        public string Decision;
        public int HarmonyValue;
        public string RuleBroken;
        public int EntryId;
        public PassengerEntry[] Entries;

        public DialogueEndEventArgs(string passengerName, string passengerAction, string decision, int harmonyValue, int entryId, PassengerEntry[] entries, string ruleBroken = null)
        {
            PassengerName = passengerName;
            PassengerAction = passengerAction;
            Decision = decision;
            HarmonyValue = harmonyValue;
            EntryId = entryId;
            Entries = entries;
            RuleBroken = ruleBroken;
        }
    }

    public class DialogueManager : UIElement, IPointerClickHandler, ISaveElement
    {
        [SerializeField] private TMP_Text displayedText;
        [SerializeField] private GameObject choiceContainer;
        [SerializeField] private float timeBetweenChars;
        [SerializeField] private BlinkPanelUI blinkPanelUI;
        
        [SerializeField] private TMP_Text playerNameText, npcNameText;

        [SerializeField] private Button showTicketButton;
        [SerializeField] private Image clickableArea;
        [Header("Ticket Pulsation Settings")]
        [SerializeField] private Transform ticketImageTransform;
        [SerializeField] private float pulsationIntervalDuration;

        [Header("External")]
        [SerializeField] private TicketMinigame ticketMinigame;
        [SerializeField] private PassengerPersonalDataLoader personalId;
        private List<PassengerEntry> _cachedEntries = new();

        private List<int> _souvenirsReceived = new();

        public static Action<int> OnHarmonyDecreased;
        public static Action<int> OnSouvenirReceived;
        public static EventHandler<DialogueEndEventArgs> OnDialogueQuitEvent;

        private Passenger _currentPassenger;

        private PassengerData _currentPassengerData;
        
        private readonly Dictionary<Button, TMP_Text> _choicesToDisplay = new();
        
        private Story _story;

        private bool _ticketScanned;
        private bool _passengerDissapear;
        public bool CanCloseDialogueWindow => _ticketScanned || _canQuitDialogue;

        private int _harmonyChange;
        private string _brokenRule;

        private Awaitable _dialogueAwaitable;
        private TextPrinter _textPrinter;
        private string _passengerAction;
        private bool _canQuitDialogue;
        private string _decision;
        private bool _gift;

        public static event Action CloseButtonEnableEvent;

        private void Awake()
        {
            Button[] buttons = choiceContainer.GetComponentsInChildren<Button>();

            foreach (Button button in buttons)
            {
                TMP_Text buttonTexts = button.GetComponentInChildren<TMP_Text>();
                
                _choicesToDisplay.Add(button, buttonTexts);
            }

            (this as ISaveElement).Register(this);

            _textPrinter = new TextPrinter();

            clickableArea.enabled = false;
        }

        private void OnEnable()
        {
            Passenger.OnPassengerInteracted += LoadPassengerDataAndStart;
            
            showTicketButton.onClick.AddListener(ToggleTicketDisplay);
        }

        private void OnDisable()
        {
            Passenger.OnPassengerInteracted -= LoadPassengerDataAndStart;
            
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

        private void LoadPassengerDataAndStart(object sender, PassengerInteractedEventArgs passengerArgs)
        {
            clickableArea.enabled = true;

            personalId.UpdatePassenderID(passengerArgs.PassengerData);

            _currentPassenger = sender as Passenger;
            _currentPassenger.GetComponent<Collider>().enabled = false;

            _currentPassengerData = passengerArgs.PassengerData;
            ticketMinigame.SetupTicketUI(passengerArgs.PassengerData);
            npcNameText.text = passengerArgs.PassengerData.passengerName;

            ResetDialogueVariables();

            StartStory(passengerArgs.PassengerData);
        }

        private void PulsateTicketImage()
        {
            ticketImageTransform
                .DOScale(1.05f, pulsationIntervalDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(5, LoopType.Yoyo);
        }

        private void ResetDialogueVariables()
        {
            _cachedEntries.Clear();
            _ticketScanned = false;
            _canQuitDialogue = false;
            _passengerDissapear = false;
            _harmonyChange = 0;
            _passengerAction = "";
            _brokenRule = "";
            _decision = "";
        }

        private void StartStory(PassengerData passengerData)
        {
            int randomDialogueIndex = Random.Range(0, passengerData.dialogueVariants.Count);
            _story = new Story(passengerData.dialogueVariants[randomDialogueIndex].ToString());
            
            _story.ObserveVariable("speakerIndex", (string varName, object newValue) => {
                UpdateNameDisplays((int)newValue);
            });
            
            _story.ObserveVariable("canScan", (string varName, object newValue) =>
            {
                SetScannable((bool)newValue);
                PulsateTicketImage();
            });

            _story.ObserveVariable("allowQuitDialogue", (string varName, object canPlayerQuitDialogue) =>
            {
                _canQuitDialogue = (bool)canPlayerQuitDialogue;
            });

            _story.ObserveVariable("dissapear", (string varName, object newValue) =>
            {
                _passengerDissapear = (bool)newValue;
            });

            _story.ObserveVariable("harmony", (string varName, object newValue) =>
            {
                _harmonyChange = (int)newValue;
                OnHarmonyDecreased?.Invoke(_harmonyChange);
            });

            _story.ObserveVariable("passengerAction", (string varName, object newValue) =>
            {
                _passengerAction = (string)newValue;
            });

            _story.ObserveVariable("decision", (string varName, object newValue) =>
            {
                _decision = (string)newValue;
            });

            _story.ObserveVariable("brokenRule", (string varName, object newValue) =>
            {
                _brokenRule = (string)newValue;
            });

            _story.ObserveVariable("staysNextDay", (string varName, object newValue) =>
            {
                _currentPassenger.StaysNextDay = (bool)newValue;
            });

            _story.ObserveVariable("addEntry", (string varName, object newValue) =>
            {
                _cachedEntries.Add(new PassengerEntry(passengerData.GetFullName(), newValue.ToString()));
                Debug.Log($"GE: {newValue}");
            });

            
            _story.ObserveVariable("gift", (string varName, object newValue) =>
            {
                _gift = (bool)newValue;

                Debug.Log("Gift is " + _gift);

                if (_gift)
                {
                    OnSouvenirReceived?.Invoke(passengerData.souvenirData.souvenirID);
                }
            });

            TrySyncSpeakerDisplayFromStoryState();
            StoryHop();
        }
        
        private void TrySyncSpeakerDisplayFromStoryState()
        {
            if (_story == null) return;

            try
            {
                object speakerValue = _story.variablesState["speakerIndex"];
                if (speakerValue is int speakerIndex)
                {
                    UpdateNameDisplays(speakerIndex);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Cannot sync speakerIndex on start: {ex.Message}");
            }
        }

        private void StoryHop()
        {
            if (_story == null) return;
            
            if (_textPrinter.IsPrinting)
            {
                _textPrinter.ForcePrintEnd(displayedText);
                return;
            }

            if (_story.canContinue)
            {
                if (_textPrinter.IsPrinting)
                {
                    _textPrinter.ForcePrintEnd(displayedText);
                    return;
                }

                _textPrinter.Print(displayedText, _story.Continue().Trim());
                
                HideChoices();
                return;
            }
            
            if (_story.currentChoices.Count > 0)
            {
                ShowChoices();
                return;
            }

            if (!_story.canContinue)
            {
                clickableArea.enabled = false;
                CloseButtonEnableEvent?.Invoke();
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

        
        private void ToggleTicketDisplay()
        {
            ticketMinigame.ShowUI();
            personalId.gameObject.SetActive(!personalId.gameObject.activeSelf);
            clickableArea.enabled = !personalId.gameObject.activeSelf;
        }

        public void HideTicketDisplay()
        {
            ticketMinigame.HideUI();
            personalId.gameObject.SetActive(false);
        }

        public void OnDialogueQuit()
        {   
            PlayerMessenger.instance.MessagePlayer("New diary entry appeared", this);

            _currentPassenger.GetComponent<Collider>().enabled = true;

            if (_brokenRule == string.Empty)
                _brokenRule = "No rules broken";

            OnDialogueQuitEvent?.Invoke(
                this,
                new DialogueEndEventArgs(
                    _currentPassenger.PassengerData.GetFullName(),
                    _passengerAction,
                    "approved",
                    _harmonyChange,
                    _currentPassenger.PassengerData.souvenirData.souvenirID,
                    _cachedEntries.ToArray(),
                    _brokenRule
                    ));

            clickableArea.enabled = false;

            _dialogueAwaitable = AwaitableDialogueQuit();
        }
        
        private async Awaitable AwaitableDialogueQuit()
        {
            if (!_passengerDissapear) return;

            await blinkPanelUI.ClosePlayerEyes();
            _currentPassenger.gameObject.SetActive(false);
            await Awaitable.WaitForSecondsAsync(1);
            await blinkPanelUI.OpenPlayerEyes();

            _dialogueAwaitable = null;
        }


        public void SaveData(GameSaveData gameSaveData)
        {
            gameSaveData.CollectedSouvenirIdList.AddRange(_souvenirsReceived);
        }

        public void LoadSave(GameSaveData gameSaveData)
        {
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GameObject pointerObject = eventData.pointerCurrentRaycast.gameObject;

            if (pointerObject == displayedText.gameObject)
            {
                StoryHop();
                return;
            }

            if (!clickableArea.enabled) 
            {
                Debug.Log("Clickable area disabled");
                return;
            }

            if (pointerObject == showTicketButton.gameObject) 
            {
                Debug.Log("ticket button Clicked");
                return;
            }

            if (pointerObject == clickableArea.gameObject)
            {
                Debug.Log("StoryHop");
                StoryHop();
                return;
            }
        }
    }
}
