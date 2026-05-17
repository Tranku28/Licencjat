using System;
using Core;
using GameMechanics.Interactions;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MainMenuQuickReturn : MonoBehaviour, IInteractable
{
    private Collider _interactionCollider;

    private void Awake()
    {
        _interactionCollider = GetComponent<Collider>();
        _interactionCollider.enabled = false;

        GameStateMachine.OnMenuReturned += EnableCollider;
        GameStateMachine.OnGameStarted += EnableCollider;
    }

    private void OnDestroy()
    {
        GameStateMachine.OnMenuReturned -= EnableCollider;
        GameStateMachine.OnGameStarted -= EnableCollider;
    }

    private void EnableCollider()
    {
        _interactionCollider.enabled = false;
    }

    private void EnableCollider(bool newGame)
    {
        _interactionCollider.enabled = true;
    }


    public string GetName()
    {
        return "Back To Main Menu";
    }

    public void Interact()
    {
        _interactionCollider.enabled = false;
        DependencyResolver.Instance.GetType<GameStateMachine>().OnMainMenuReturn();
    }
}
