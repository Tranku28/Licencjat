using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float movementSensivity;
    
    private Camera _playerCamera;
    private InputAction _lookInput;
    private InputAction _moveInput;
    private Rigidbody _playerRb;
    
    private float _yMoveOffset, _xMoveOffset;

    private void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _playerRb = GetComponent<Rigidbody>();
        
        _lookInput = InputSystem.actions.FindAction("Look");
        _moveInput = InputSystem.actions.FindAction("Move");
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        Rotate();
    }

    private void FixedUpdate()
    {
        var input = _moveInput.ReadValue<Vector2>();
        var move = new Vector3(input.x * movementSensivity, 0f, input.y * movementSensivity);
        var moveDirection = transform.TransformDirection(move);
        
        _playerRb.linearVelocity = moveDirection;
    }

    private void Rotate()
    {
        _yMoveOffset += _lookInput.ReadValue<Vector2>().y * movementSensivity * Time.deltaTime;
        _xMoveOffset = _lookInput.ReadValue<Vector2>().x * movementSensivity * Time.deltaTime;
        
        var clampedAngle = Mathf.Clamp(-_yMoveOffset, -55f, 55f);
        _playerCamera.transform.localRotation = Quaternion.Euler(clampedAngle, 0f, 0f);
        transform.Rotate(0, _xMoveOffset, 0);
    }
}
