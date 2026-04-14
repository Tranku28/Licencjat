using GameMechanics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuncherMover : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private RectTransform uiTransform;
    [SerializeField] private TicketMinigame ticketMinigame;
    
    private PuncherOrientation _orientation;

    private void Start()
    {
        camera = Camera.main;

        ticketMinigame.OnTicketMoved += RepositionPuncher;
    }

    private void OnDestroy() => ticketMinigame.OnTicketMoved -= RepositionPuncher;

    private void RepositionPuncher(object sender, TicketMinigame.OnTicketPointerMovedEventArgs e)
    {
        if (e.Orientation == _orientation)
        {
            return;
        }

        float targetY = e.Orientation switch
        {
            PuncherOrientation.Left => 180f,
            PuncherOrientation.Top => 90f,
            PuncherOrientation.Right => 0f,
            PuncherOrientation.Bottom => -90f,
            _ => 0f
        };
        
        Quaternion puncherWorldRotation = e.Rotation * Quaternion.Euler(-90f, 0f, 0f);

        transform.rotation = puncherWorldRotation * Quaternion.Euler(0f, targetY, 0f);

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
