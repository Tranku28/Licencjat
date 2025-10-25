using System;
using UnityEngine;

namespace Interactions
{
    public interface IInteractable
    {
        public static Action<IInteractable> OnHover;
        public static Action OnHoverExit;
        public void Interact(){}
    }
}
