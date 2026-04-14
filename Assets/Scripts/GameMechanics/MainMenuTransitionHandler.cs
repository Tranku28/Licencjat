using Core;
using GameMechanics;
using GameMechanics.Player;
using Unity.Cinemachine;
using UnityEngine;

public class MainMenuTransitionHandler : MonoBehaviour
{
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private CinemachineCamera playerHeadCamera;
    private GameStateMachine _gameStateMachine;
    private Vector3 _initialPlayerPosition;
    private Vector3 _initialCameraPosition;
    private Quaternion _initialPlayerRotation;
    private Quaternion _initialCameraRotation;

    private void Start()
    {
        GameStateMachine.OnMenuReturned += GoToMainMenu;

        _gameStateMachine = DependencyResolver.Instance.GetType<GameStateMachine>();

        _initialPlayerPosition = playerMovementController.transform.position;
        _initialPlayerRotation = playerMovementController.transform.rotation;
        _initialCameraPosition = playerHeadCamera.transform.position;
        _initialCameraRotation = playerHeadCamera.transform.rotation;
    }

    private void OnDestroy()
    {
        GameStateMachine.OnMenuReturned -= GoToMainMenu;
    }

    private void GoToMainMenu()
    {
        _gameStateMachine.ChangeGameState(GameState.MainMenu);

        playerMovementController.transform.position = _initialPlayerPosition;
        playerMovementController.transform.rotation = _initialPlayerRotation;
        playerHeadCamera.transform.position = _initialCameraPosition;
        playerHeadCamera.transform.rotation = _initialCameraRotation;
    }
}
