using System;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameMechanics.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float cameraSensivity;
        [SerializeField] private float moveSpeed;
        [SerializeField] private bool canSprint;
        [SerializeField] private float sprintMultiplier;
        [SerializeField] private float stepSoundCooldown = 0.5f;
        
        private float _currentTime = 0;
        private Camera _playerCamera;
        private InputAction _lookInput;
        private InputAction _moveInput;
        private InputAction _sprintInput;
        private CharacterController _characterController;
    
        private float _yMoveOffset, _xMoveOffset;
        private float _cameraPitch;
        
        private bool _canMove = false;
        
        private GameStateMachine _gameStateMachine;

        private void Awake()
        {
            _playerCamera = GetComponentInChildren<Camera>();
            _characterController = GetComponent<CharacterController>();
        
            _lookInput = InputSystem.actions.FindAction("Look");
            _moveInput = InputSystem.actions.FindAction("Move");
            _sprintInput = InputSystem.actions.FindAction("Sprint");
        }

        private void OnEnable()
        {
            GameStateMachine.OnGameStateChanged += MovementEnabler;
        }

        private void OnDisable()
        {
            GameStateMachine.OnGameStateChanged -= MovementEnabler;
        }

        private void Update()
        {
            Debug.Log(_canMove);
            if (!_canMove) return;
            
            Rotate();
            Move();
        }

        private void MovementEnabler(GameState obj)
        {
            if (obj == GameState.Paused
                || obj == GameState.UIOpened
                || obj == GameState.MainMenu
                )
            {
                _canMove = false;
                return;
            }
            
            _canMove = true;
        }
        
        private void Move()
        {
            var input = _moveInput.ReadValue<Vector2>();
        
            if (input == Vector2.zero) return;
            
            _currentTime += Time.deltaTime;

            if (_currentTime > stepSoundCooldown)
            {
                _currentTime = 0;
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.stepSound, transform.position);
            }

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
    }
}
