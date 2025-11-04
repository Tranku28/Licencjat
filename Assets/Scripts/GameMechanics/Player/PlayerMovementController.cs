using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour, IRegister
    {
        [SerializeField] private float cameraSensivity;
        [SerializeField] private float moveSpeed;
        [SerializeField] private bool canSprint;
        [SerializeField] private float sprintMultiplier;
    
        private Camera _playerCamera;
        private InputAction _lookInput;
        private InputAction _moveInput;
        private InputAction _sprintInput;
        private CharacterController _characterController;
    
        private float _yMoveOffset, _xMoveOffset;
        private float _cameraPitch;

        private void Awake()
        {
            _playerCamera = GetComponentInChildren<Camera>();
            _characterController = GetComponent<CharacterController>();
        
            _lookInput = InputSystem.actions.FindAction("Look");
            _moveInput = InputSystem.actions.FindAction("Move");
            _sprintInput = InputSystem.actions.FindAction("Sprint");
        }

        private void Start()
        {
            Register();
        }

        private void OnDestroy()
        {
            Unregister();
        }

        private void Update()
        {
            Rotate();
            Move();
        }

        private void Move()
        {
            var input = _moveInput.ReadValue<Vector2>();
        
            if (input == Vector2.zero) return;

            if (_sprintInput.IsPressed() && canSprint)
            {
                float sprint = sprintMultiplier = canSprint ? sprintMultiplier : 1;
            
                var move = new Vector3(input.x * moveSpeed * sprint, 0f, input.y * moveSpeed * sprint);
                var moveDirection = transform.TransformDirection(move);
        
                _characterController.SimpleMove(moveDirection);
            }
            else
            {
                var move = new Vector3(input.x * moveSpeed, 0f, input.y * moveSpeed);
                var moveDirection = transform.TransformDirection(move);
        
                _characterController.SimpleMove(moveDirection);
            }
        
        }

        private void Rotate()
        {
            Vector2 lookInput = _lookInput.ReadValue<Vector2>();
            if (lookInput == Vector2.zero) return;
        
            float mouseX = lookInput.x * cameraSensivity * Time.fixedDeltaTime;
            float mouseY = lookInput.y * cameraSensivity * Time.fixedDeltaTime;
        
            _cameraPitch -= mouseY;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -55f, 55f);
        
            _playerCamera.transform.localEulerAngles = new Vector3(_cameraPitch, 0f, 0f);
        
            transform.Rotate(0f, mouseX, 0f);
        }

        public void Register()
        {
            Registry.Instance.Register(this);
        }

        public void Unregister()
        {
            Registry.Instance.Unregister(this);
        }
    }
}
