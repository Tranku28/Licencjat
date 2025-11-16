using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        [SerializeField] private float timeBetweenChars;

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
            string text = _story.Continue().Trim();
            displayedText.text = text;
        }
        
        private void StoryHop()
        {
            if (_story == null) return;
            
            if (_story.canContinue)
            {
                string text = _story.Continue().Trim();
                Debug.Log(text);
                displayedText.text = text;
                
                HideChoices();
            }
            
            if (_story.currentChoices.Count > 0)
            {
                ShowChoices();
            }

            if (!_story.canContinue && _story.currentChoices.Count == 0)
            {
                Debug.Log("The end of story");
            }
        }
        
        
        private void ShowChoices()
        {
            choiceContainer.SetActive(true);

            int index = 0;

            foreach (var pair in choicesToDisplay)
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
            foreach (var pair in choicesToDisplay)
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

    }
}
