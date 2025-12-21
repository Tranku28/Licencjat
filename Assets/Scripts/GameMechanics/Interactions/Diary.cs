using System;
using Core;
using UnityEngine;

namespace GameMechanics.Interactions
{
    public class Diary : MonoBehaviour, IInteractable
    {
        [SerializeField] private TutorialData diaryTutorial;
        public static Action OnDiaryInteracted;
        private bool _tutorialShown;
    
        public void Interact()
        {
            // TODO: Tutorial single popup handling
            if (!_tutorialShown)
            {
                TutorialInfoLoader.Instance.LoadTutorialPanel(diaryTutorial);
                _tutorialShown = true;
            }
            OnDiaryInteracted?.Invoke();
        }
    }
}
