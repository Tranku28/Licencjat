using System;
using Core;
using GameMechanics.Interactions;
using GameMechanics.UI.MainMenu;
using UnityEngine;

public class ConductorGuidelines : MonoBehaviour, IInteractable
{
    [SerializeField] private StartGameBoardHandler startGameBoardHandler;
    public static event Action OnTutorialNotesInteracted;

    private void OnEnable()
    {
        GameStateMachine.OnGameStarted += OnNewGameTutorialInvoke;
    }

    private void OnDisable()
    {
        GameStateMachine.OnGameStarted -= OnNewGameTutorialInvoke;
    }

    public void Interact()
    {
        OnTutorialNotesInteracted?.Invoke();
    }

    public void OnNewGameTutorialInvoke(bool isNewGame)
    {
        if (isNewGame)
        {
            OnTutorialNotesInteracted?.Invoke();
        }
    }

    public string GetName()
    {
        return "A Conductor's guidelines";
    }
}
