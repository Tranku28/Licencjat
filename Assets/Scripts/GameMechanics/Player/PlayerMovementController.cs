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
        [SerializeField] private float stepDistance;
        
        private float _currentTime = 0;
        private Camera _playerCamera;
        private InputAction _lookInput;
        private InputAction _moveInput;
        private InputAction _sprintInput;
        private CharacterController _characterController;
        private GameStateMachine _gameStateMachine;
    
        private float _cameraPitch;
        
        private bool _canMove = false;
        private Vector2 _moveInputVector;
        private Vector3 _previousDistance;
        private bool _stepDone;

        private void Awake()
        {
            _playerCamera = GetComponentInChildren<Camera>();
            _characterController = GetComponent<CharacterController>();

            _gameStateMachine = DependencyResolver.Instance.GetType<GameStateMachine>();
        
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
            _previousDistance = transform.position;
        }
        
        private void Move()
        {
            _moveInputVector = _moveInput.ReadValue<Vector2>();
        
            if (_moveInputVector == Vector2.zero) return;
            
            _currentTime += Time.deltaTime;
            _stepDone = Vector3.Distance(transform.position, _previousDistance) > stepDistance;

            if (_stepDone && _currentTime > stepSoundCooldown)
            {
                _previousDistance = transform.position;
                _currentTime = 0;
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.stepSound, transform.position);
            }

            if (_sprintInput.IsPressed() && canSprint)
            {
                float sprint = sprintMultiplier = canSprint ? sprintMultiplier : 1;
            
                var move = new Vector3(_moveInputVector.x * moveSpeed * sprint, 0f, _moveInputVector.y * moveSpeed * sprint);
                var moveDirection = transform.TransformDirection(move);
        
                _characterController.SimpleMove(moveDirection);
            }
            else
            {
                var move = new Vector3(_moveInputVector.x * moveSpeed, 0f, _moveInputVector.y * moveSpeed);
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
