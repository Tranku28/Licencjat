using System;
using UnityEngine;

namespace Interactions
{
    public class InteractionTester : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("Interacting...");
        }
    }
}
