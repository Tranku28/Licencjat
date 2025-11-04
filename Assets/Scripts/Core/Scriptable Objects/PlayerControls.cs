using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerControls", menuName = "Scriptable Objects/PlayerControls")]
    public class PlayerControls : ScriptableObject
    {
        [SerializeField] private InputActionAsset playerInputAsset;

        private InputAction _quitAction;
        private InputAction _interactAction;

        public static event UnityAction OnInteractEvent;
        public static event UnityAction OnQuitEvent;

        private void OnEnable()
        {
            _quitAction = playerInputAsset.FindAction("Quit");
            _interactAction = playerInputAsset.FindAction("Interact");

            _quitAction.started += OnQuit;
            _quitAction.performed += OnQuit;
            _quitAction.canceled += OnQuit;
            
            _interactAction.started += OnInteract;
            _interactAction.performed += OnInteract;
            _interactAction.canceled += OnInteract;
            
            _quitAction.Enable();
            _interactAction.Enable();
        }

        private void OnDisable()
        {
            _quitAction.started -= OnQuit;
            _quitAction.performed -= OnQuit;
            _quitAction.canceled -= OnQuit;
            _interactAction.started -= OnInteract;
            _interactAction.performed -= OnInteract;
            _interactAction.canceled -= OnInteract;
            
            _quitAction.Disable();
            _interactAction.Disable();
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed) OnInteractEvent?.Invoke();
        }

        private void OnQuit(InputAction.CallbackContext context)
        {
            if (context.performed) OnQuitEvent?.Invoke();
        }
    }
}
