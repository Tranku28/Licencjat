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
        private InputAction _singleClickAction;

        public static event UnityAction OnInteractEvent;
        public static event UnityAction OnEscapePressedEvent;
        public static event UnityAction OnClickEvent;
        
        public static event UnityAction OnSingleClickEvent;

        private void OnEnable()
        {
            _quitAction = playerInputAsset.FindAction("Quit");
            _interactAction = playerInputAsset.FindAction("Interact");
            _clickAction = playerInputAsset.FindAction("Click");
            _singleClickAction = playerInputAsset.FindAction("SingleClick");

            _quitAction.started += OnEscapePressed;
            _quitAction.performed += OnEscapePressed;
            _quitAction.canceled += OnEscapePressed;

            _interactAction.started += OnInteract;
            _interactAction.performed += OnInteract;
            _interactAction.canceled += OnInteract;

            _clickAction.started += OnClick;
            _clickAction.performed += OnClick;
            _clickAction.canceled += OnClick;
            
            _singleClickAction.started += OnSingleClick;
            _singleClickAction.performed += OnSingleClick;
            _singleClickAction.canceled += OnSingleClick;

            _quitAction.Enable();
            _interactAction.Enable();
            _clickAction.Enable();
            _singleClickAction.Enable();
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
            
            _singleClickAction.started -= OnSingleClick;
            _singleClickAction.performed -= OnSingleClick;
            _singleClickAction.canceled -= OnSingleClick;
            
            _quitAction.Disable();
            _interactAction.Disable();
            _clickAction.Disable();
            _singleClickAction.Disable();
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
        
        private void OnSingleClick(InputAction.CallbackContext context)
        {
            if (context.canceled) OnSingleClickEvent?.Invoke();
        }
    }
}
