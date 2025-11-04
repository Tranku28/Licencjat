using System;
using System.Collections;
using Core;
using GameMechanics.Interactions;
using Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Camera))]
    public class InteractionHandler : MonoBehaviour
    {
        [Range(0.05f, 1.0f)]
        [SerializeField] private float tickDuration;

        [SerializeField] private GameObject interactionUI;
        private IInteractable _currentInteractable;
        
        [Range(0.5f, 3f)]
        [SerializeField] private float raycastMaxDistance = 2f;
        private RaycastHit[] _hitResults = new RaycastHit[5];
        
        private Camera _playerCamera;

        private Awaitable _checkForInteractable;
        
        private GameManager _gameManager;

        private void Awake()
        {
            _playerCamera = GetComponent<Camera>();
            PlayerControls.OnInteractEvent += Interact;
        }

        private void Start()
        {
            _gameManager = Registry.Instance.GetType<GameManager>();
        }

        private void OnEnable()
        {
            _checkForInteractable = CheckIfInteractable();
        }

        private void Interact()
        {
            if (_gameManager.CurrentGameState == GameState.Game)
                _currentInteractable?.Interact();
        }
        
        private async Awaitable CheckIfInteractable()
        {
            while (true)
            {
                await Awaitable.WaitForSecondsAsync(tickDuration);

                Ray ray = _playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                int hitsCount = Physics.RaycastNonAlloc(ray.origin, ray.direction, _hitResults, raycastMaxDistance);

                _currentInteractable = null;
                Debug.Log("awaitable");
                
                for (int i = 0; i < hitsCount; i++)
                {
                    var hit = _hitResults[i];
                    
                    if (!hit.collider || !hit.collider.TryGetComponent(out IInteractable interactable)) continue;
                    
                    _currentInteractable = interactable;
                    break;
                }

                interactionUI.SetActive(_currentInteractable != null);
            }
        }

        private void OnDisable()
        {
            _checkForInteractable?.Cancel();
        }
    }
}
