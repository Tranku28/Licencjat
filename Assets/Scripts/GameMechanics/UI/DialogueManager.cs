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
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] TMP_Text displayedText;
        [SerializeField] GameObject choiceContainer;

        private int _currentSpeakerIndex;
        
        private Dictionary<Button, TMP_Text> choicesToDisplay = new();
        
        private Story _story;

        private void Awake()
        {
            Button[] buttons = choiceContainer.GetComponentsInChildren<Button>();

            foreach (Button button in buttons)
            {
                TMP_Text buttonTexts = button.GetComponentInChildren<TMP_Text>();
                
                choicesToDisplay.Add(button, buttonTexts);
            }

            foreach (var choice in choicesToDisplay.Keys)
            {
                Debug.Log(choice.name);
            }
        }

        private void OnEnable()
        {
            Passenger.OnPassengerInteracted += StartStory;
            PlayerControls.OnSingleClickEvent += StoryHop;
        }

        private void OnDisable()
        {
            Passenger.OnPassengerInteracted -= StartStory;
            PlayerControls.OnSingleClickEvent -= StoryHop;
        }

        private void StartStory(PassengerData obj)
        {
            _story = new Story(obj.inkJSON.text);
        }
        
        private void StoryHop()
        {
            if (ReferenceEquals(_story, null)) return;
            
            int speakerIndex = (int) _story.variablesState["speakerIndex"];

            if (_currentSpeakerIndex != speakerIndex)
            {
                SwitchSpeaker();
                _currentSpeakerIndex = speakerIndex;
            }
            
            if (_story.currentChoices.Count > 0)
            {
                displayedText.enabled = false;
                
                choiceContainer.SetActive(true);
                
                int index = 0;

                foreach (var choice in choicesToDisplay)
                {
                    if (index < _story.currentChoices.Count)
                    {
                        Button button = choice.Key;
                        
                        button.gameObject.SetActive(true);
                        button.onClick.RemoveAllListeners();
                        var index1 = index;
                        button.onClick.AddListener(() => _story.ChooseChoiceIndex(index1));
                        
                        choice.Value.text = _story.currentChoices[index].text;
                    }
                    
                    if (index >= _story.currentChoices.Count)
                    {
                        choice.Key.gameObject.SetActive(false);
                    }
                    
                    index++;
                }
            }
            
            if (!_story.canContinue) return;
            
            string text = _story.Continue().Trim();
            displayedText.text = text;
        }

        private void SwitchSpeaker()
        {
            Debug.Log(_currentSpeakerIndex);
        }
    }
}
