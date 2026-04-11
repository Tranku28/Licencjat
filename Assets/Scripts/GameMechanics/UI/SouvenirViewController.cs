using System;
using Core.Scriptable_Objects;
using Core.Scriptable_Objects.Souvenirs;
using GameMechanics.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SouvenirViewController : UIElement, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button nextButton, previousButton, useButton;
    [SerializeField] private TMP_Text souvenirName, souvenirDescription, effectDescription, receivedFrom;
    [SerializeField] private Transform souvenirContainer;
    [SerializeField] private float souvenirContainerRotationSpeed;

    private Souvenir _lastSouvenir;

    private Quaternion souvenirPrefabContainerInitialRotation;

    private int _rotationDir = 0;
    private bool _isRotatingObject;

    public Button UseButton => useButton;

    private void Awake()
    {
        nextButton.GetComponent<HoldButton>().OnHoldStateChanged += RotateRight;
        previousButton.GetComponent<HoldButton>().OnHoldStateChanged += RotateLeft;

        souvenirPrefabContainerInitialRotation = souvenirContainer.rotation;
    }

    private void Start()
    {
        Souvenir.OnSouvenirInteracted += LoadSouvenirWindow;
    }

    void Update()
    {
        if (_isRotatingObject)
        {
            souvenirContainer.rotation *= Quaternion.Euler(0, _rotationDir * souvenirContainerRotationSpeed, 0);
        }
    }

    private void OnDestroy()
    {
        nextButton.GetComponent<HoldButton>().OnHoldStateChanged -= RotateRight;
        previousButton.GetComponent<HoldButton>().OnHoldStateChanged -= RotateLeft;

        Souvenir.OnSouvenirInteracted -= LoadSouvenirWindow;
    }

    private void RotateLeft(bool direction)
    {
        _rotationDir = direction ? -1 : 0;
        _isRotatingObject = direction;
    }

    private void RotateRight(bool direction)
    {
        _rotationDir = direction ? -1 : 0;
        _isRotatingObject = direction;
    }
    
    //TODO: disable items instead of Destroy
    private void LoadSouvenirWindow(SouvenirData obj)
    {
        _rotationDir = 0;

        if (_lastSouvenir != null) Destroy(_lastSouvenir.gameObject);

        souvenirName.text = obj.name;
        souvenirDescription.text = obj.souvenirDescription;
        effectDescription.text = obj.souvenirEffectDescription;
        receivedFrom.text = $"Received from: {obj.receivedFrom}";
        _lastSouvenir = Instantiate(obj.souvenirPrefab, souvenirContainer).GetComponent<Souvenir>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject raycastedObject = eventData.pointerPressRaycast.gameObject;
        Debug.Log(raycastedObject);

        if (raycastedObject == previousButton || raycastedObject == nextButton)
        {
            _isRotatingObject = true;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isRotatingObject = false;
    }
}
