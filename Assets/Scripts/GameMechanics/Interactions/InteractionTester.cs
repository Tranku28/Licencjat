using GameMechanics.Interactions;
using UnityEngine;

namespace Interactions
{
    public class InteractionTester : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log($"Interacted with {name}");
        }

        public void DisplayUI()
        {
            
        }
    }
}
