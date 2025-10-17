using System;
using System.Collections;
using Interactions;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Camera))]
    public class InteractionHandler : MonoBehaviour
    {
        [Range(0.05f, 1.0f)]
        [SerializeField] private float tickDuration;
        
        private const float RAYCAST_RANGE = 100f;
        private RaycastHit[] hits = new RaycastHit[10];
        private Camera _playerCamera;

        private void Awake()
        {
            _playerCamera = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            StartCoroutine(CheckIfInteractable());
        }

        private IEnumerator CheckIfInteractable()
        {
            while (true)
            {
                yield return new WaitForSeconds(tickDuration);
                
                Ray ray = _playerCamera.ScreenPointToRay(_playerCamera.transform.position * RAYCAST_RANGE);
                Physics.RaycastNonAlloc(ray, hits, RAYCAST_RANGE);
                Debug.DrawRay(_playerCamera.transform.position, ray.direction * RAYCAST_RANGE, Color.red, 10f);
                
                try
                {
                    foreach (var hit in hits)
                    {
                        if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                        {
                            interactable.Interact();
                            Debug.Log("can Interact");
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
