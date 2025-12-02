using System;
using GameMechanics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuncherMover : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private RectTransform uiTransform;
    [SerializeField] private TicketMinigame ticketMinigame;
    
    private void Start()
    {
        camera = Camera.main;

        ticketMinigame.OnTicketClicked += PunchHole;
    }

    private void OnDestroy() => ticketMinigame.OnTicketClicked -= PunchHole;

    private void PunchHole(object sender, TicketMinigame.OnTicketClickedEventArgs e)
    {
        switch (e.Orientation)
        {
            case PuncherOrientation.Left:
                transform.localEulerAngles = new Vector3(90f, transform.rotation.y, 0f);
                break;
            case PuncherOrientation.Right:
                transform.localEulerAngles = new Vector3(-90f, transform.rotation.y, 0f);
                break;
            case PuncherOrientation.Top:
                transform.localEulerAngles = new Vector3(-180f, transform.rotation.y, 0f);
                break;
            case PuncherOrientation.Bottom:
                transform.localEulerAngles = new Vector3(0f, transform.rotation.y, 0f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Update()
    {
        Vector2 pointerPosition = Mouse.current.position.ReadValue();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                uiTransform,
                pointerPosition,
                camera,
                out Vector2 localPoint))
        {
            transform.localPosition = new Vector3(localPoint.x, localPoint.y, 0f);
        }
    }
}
