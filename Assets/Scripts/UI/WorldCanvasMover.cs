using System;
using Interactions;
using UnityEngine;

namespace UI
{
    public class WorldCanvasMover : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        private float yOffset = 0.5f;

        private void OnEnable()
        {
            IInteractable.OnHover += MoveCanvas;
            IInteractable.OnHoverExit += HideCanvas;
        }

        private void HideCanvas()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            transform.LookAt(playerCamera.transform);
        }

        private void MoveCanvas(IInteractable interactable)
        {
            var monoBehaviour = interactable as MonoBehaviour;
            
            if (!monoBehaviour) return;
        
            var objectTop = monoBehaviour.GetComponent<Renderer>().bounds.extents.y;
        
            transform.position = new  Vector3(
                monoBehaviour.transform.position.x,
                objectTop + yOffset, 
                monoBehaviour.transform.position.z
                );
        }

        private void OnDisable()
        {
            IInteractable.OnHover -= MoveCanvas;
            IInteractable.OnHoverExit -= HideCanvas;
        }
    }
}
