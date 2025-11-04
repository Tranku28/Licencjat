using GameMechanics.UI;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameMechanics
{
    public class RuntimeUIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private UIElement pauseMenu;
        [SerializeField] private UIElement ticketTab;
        
        private UIElement _currentElement;
        private InputAction _currentAction;

        private void OnEnable()
        {
            PlayerControls.OnQuitEvent += CloseUI;
            PlayerControls.OnInteractEvent += DisplayInteractableUI;
        }

        private void OnDisable()
        {
            PlayerControls.OnQuitEvent -= CloseUI;
            PlayerControls.OnInteractEvent -= DisplayInteractableUI;
        }

        private void DisplayInteractableUI()
        {
            if (_currentElement != null) return;
                _currentElement = ticketTab;
        }

        private void CloseUI()
        {
            if (_currentElement == null)
            {
                pauseMenu.SetVisualVisibility(true);
                return;
            }

            if (_currentElement == pauseMenu)
            {
                pauseMenu.SetVisualVisibility(false);
                _currentElement = null;
                return;
            }
            
            _currentElement.SetVisualVisibility(false);
            _currentElement = null;
        }
    }
}
