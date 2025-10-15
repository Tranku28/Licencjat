using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float cameraSensivity;
    [SerializeField] private float moveSpeed;
    
    private Camera _playerCamera;
    private InputAction _lookInput;
    private InputAction _moveInput;
    private CharacterController _characterController;
    
    private float _yMoveOffset, _xMoveOffset;
    private float _cameraPitch;

    private void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _characterController = GetComponent<CharacterController>();
        
        _lookInput = InputSystem.actions.FindAction("Look");
        _moveInput = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        Rotate();
        
        var input = _moveInput.ReadValue<Vector2>();
        
        if (input == Vector2.zero) return;
        
        var move = new Vector3(input.x * moveSpeed, 0f, input.y * moveSpeed);
        var moveDirection = transform.TransformDirection(move);
        
        _characterController.SimpleMove(moveDirection);
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
