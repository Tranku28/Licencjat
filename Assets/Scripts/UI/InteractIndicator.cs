using System;
using Interactions;
using UnityEngine;

namespace UI
{
    public class InteractIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject indicatorVisual;

        private void OnEnable()
        {
            IInteractable.OnHover += ShowUI;
            IInteractable.OnHoverExit += HideUI;
        }

        private void ShowUI(IInteractable interactable)
        {
            indicatorVisual.SetActive(true);
        }

        private void HideUI()
        {
            indicatorVisual.SetActive(false);
        }

        private void OnDisable()
        {
            IInteractable.OnHover -= ShowUI;
            IInteractable.OnHoverExit -= HideUI;
        }
    }
}
