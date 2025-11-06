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
        
        private Dictionary<Button, TMP_Text> choicesToDisplay = new();
        
        private Story story;

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
            story = new Story(obj.inkJSON.text);
        }
        
        private void StoryHop()
        {
            if (!story) return;

            Debug.Log("Story Hop");
            
            if (story.currentChoices.Count > 0)
            {
                Debug.Log(story.currentChoices.Count);
            }
            
            string text = story.Continue().Trim();
            Debug.Log(text);
            displayedText.text = text;
        }
    }
}
