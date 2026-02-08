using System;

namespace GameMechanics.Interactions
{
    public interface IInteractable
    {
        public static Action<IInteractable> OnHover;
        public static Action OnHoverExit;
        public void Interact();
        public string GetName();
    }
}
