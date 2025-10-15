using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float movementSpeed;
    
    private Camera _playerCamera;
    private InputAction _lookInput;
    private InputAction _moveInput;
    private CharacterController _characterController;
    
    private float _yMoveOffset, _xMoveOffset;

    private void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _characterController = GetComponent<CharacterController>();
        
        _lookInput = InputSystem.actions.FindAction("Look");
        _moveInput = InputSystem.actions.FindAction("Move");
    }
    
    void Update()
    {
        Rotate();
        
        var input = _moveInput.ReadValue<Vector2>();
        var moveDirection = transform.forward * (input.y * movementSpeed);
        
        _characterController.SimpleMove(moveDirection + new Vector3(input.x, 0, input.y));
    }

    private void Rotate()
    {
        _yMoveOffset += _lookInput.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;
        _xMoveOffset = _lookInput.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
        
        var clampedAngle = Mathf.Clamp(-_yMoveOffset, -55f, 55f);
        _playerCamera.transform.localRotation = Quaternion.Euler(clampedAngle, 0f, 0f);
        transform.Rotate(0, _xMoveOffset, 0);
    }
}
