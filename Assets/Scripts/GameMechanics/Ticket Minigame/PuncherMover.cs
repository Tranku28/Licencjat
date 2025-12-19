using System;
using GameMechanics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuncherMover : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private RectTransform uiTransform;
    [SerializeField] private TicketMinigame ticketMinigame;
    
    private PuncherOrientation _orientation;
    private Quaternion _initialRotation;

    private void Awake()
    {
        _initialRotation = transform.rotation;
    }

    private void Start()
    {
        camera = Camera.main;

        ticketMinigame.OnTicketMoved += RepositionPuncher;
    }

    private void OnDestroy() => ticketMinigame.OnTicketMoved -= RepositionPuncher;

    private void RepositionPuncher(object sender, TicketMinigame.OnTicketClickedEventArgs e)
    {
        if (e.Orientation == _orientation)
        {
            return;
        }
        
        float targetY = e.Orientation switch
        {
            PuncherOrientation.Left => -90f,
            PuncherOrientation.Right => 90f,
            PuncherOrientation.Top => 180f,
            PuncherOrientation.Bottom => 0f,
            _ => 0f
        };
        
        transform.rotation = _initialRotation * Quaternion.Euler(0f, targetY, 0f);

        _orientation = e.Orientation;
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
