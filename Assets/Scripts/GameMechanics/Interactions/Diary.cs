using System;
using Core;
using UnityEngine;

namespace GameMechanics.Interactions
{
    public class Diary : MonoBehaviour, IInteractable
    {
        public static Action OnDiaryInteracted;
    
        public void Interact()
        {
            OnDiaryInteracted?.Invoke();
        }

        public string GetName()
        {
            return "Diary";
        }
    }
}
