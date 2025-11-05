using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Core.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "PlayerControls", menuName = "Scriptable Objects/PlayerControls")]
    public class PlayerControls : ScriptableObject
    {
        [SerializeField] private InputActionAsset playerInputAsset;

        private InputAction _quitAction;
        private InputAction _interactAction;
        private InputAction _clickAction;

        public static event UnityAction OnInteractEvent;
        public static event UnityAction OnEscapePressedEvent;
        public static event UnityAction OnClickEvent;

        private void OnEnable()
        {
            _quitAction = playerInputAsset.FindAction("Quit");
            _interactAction = playerInputAsset.FindAction("Interact");
            _clickAction = playerInputAsset.FindAction("Click");

            _quitAction.started += OnEscapePressed;
            _quitAction.performed += OnEscapePressed;
            _quitAction.canceled += OnEscapePressed;

            _interactAction.started += OnInteract;
            _interactAction.performed += OnInteract;
            _interactAction.canceled += OnInteract;

            _clickAction.started += OnClick;
            _clickAction.performed += OnClick;
            _clickAction.canceled += OnClick;

            _quitAction.Enable();
            _interactAction.Enable();
            _clickAction.Enable();
        }

        private void OnDisable()
        {
            _quitAction.started -= OnEscapePressed;
            _quitAction.performed -= OnEscapePressed;
            _quitAction.canceled -= OnEscapePressed;
            
            _interactAction.started -= OnInteract;
            _interactAction.performed -= OnInteract;
            _interactAction.canceled -= OnInteract;
            
            _clickAction.started -= OnClick;
            _clickAction.performed -= OnClick;
            _clickAction.canceled -= OnClick;
            
            _quitAction.Disable();
            _interactAction.Disable();
            _clickAction.Disable();
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed) OnInteractEvent?.Invoke();
        }

        private void OnEscapePressed(InputAction.CallbackContext context)
        {
            if (context.performed) OnEscapePressedEvent?.Invoke();
        }
        
        private void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed) OnClickEvent?.Invoke();
        }
    }
}
