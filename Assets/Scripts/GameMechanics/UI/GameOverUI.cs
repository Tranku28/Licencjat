using System;
using Core;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text gameOverReasonText;

    private void Awake()
    {
        gameOverText.enabled = false;
        gameOverReasonText.enabled = false;

        GameStateMachine.OnGameOver += OnGameOver;
        GameStateMachine.OnMenuReturned += OnMenuReturned;
    }

    private void OnDestroy()
    {
        GameStateMachine.OnGameOver -= OnGameOver;
        GameStateMachine.OnMenuReturned -= OnMenuReturned;
    }

    private void OnGameOver()
    {
        gameOverText.enabled = true;
        gameOverReasonText.enabled = true;
    }

    private void OnMenuReturned()
    {
        gameOverText.enabled = false;
        gameOverReasonText.enabled = false;
    }
}
