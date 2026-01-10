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
            OnDiaryInteracted?.Invoke();
        }
    }
}
