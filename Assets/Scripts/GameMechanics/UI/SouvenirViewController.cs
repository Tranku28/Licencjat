using System;
using Core.Scriptable_Objects;
using GameMechanics.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SouvenirViewController : UIElement
{
    [SerializeField] private Button nextButton, previousButton;
    [SerializeField] private TMP_Text souvenirName, souvenirDescription, receivedFrom;
    [SerializeField] private Transform souvenirContainer;
    [SerializeField] private float souvenirContainerRotationSpeed;

    private Souvenir _lastSouvenir;

    private Quaternion souvenirPrefabContainerInitialRotation;

    private int _rotationDir = 0;

    private void Awake()
    {
        nextButton.onClick.AddListener(ShowNextSouvenirData);
        previousButton.onClick.AddListener(ShowPreviousSouvenirData);

        souvenirPrefabContainerInitialRotation = souvenirContainer.transform.rotation;
    }

    private void Start()
    {
        Souvenir.OnSouvenirInteracted += LoadSouvenirWindow;
    }

    void Update()
    {
        if (_rotationDir == 0) return;

        souvenirContainer.rotation *= Quaternion.Euler(0, _rotationDir * souvenirContainerRotationSpeed, 0);
    }

    private void OnDestroy()
    {
        nextButton.onClick.RemoveListener(ShowNextSouvenirData);
        previousButton.onClick.RemoveListener(ShowPreviousSouvenirData);
        
        Souvenir.OnSouvenirInteracted -= LoadSouvenirWindow;
    }

    private void ShowNextSouvenirData()
    {
        if (_rotationDir == -1) 
        {
            _rotationDir = 0;
            return;
        }

        _rotationDir = 1;
    }

    private void ShowPreviousSouvenirData()
    {
        if (_rotationDir == 1) 
        {
            _rotationDir = 0;
            return;
        }

        _rotationDir = -1;
    }
    
    //TODO: disable items instead of Destroy
    private void LoadSouvenirWindow(PassengerData obj)
    {
        _rotationDir = 0;

        if (_lastSouvenir != null) Destroy(_lastSouvenir.gameObject);

        souvenirName.text = obj.souvenirName;
        souvenirDescription.text = obj.souvenirNote;
        receivedFrom.text = $"Received from: {obj.passengerName} {obj.passengerSurname}";
        _lastSouvenir = Instantiate(obj.souvenirData.souvenirPrefab, souvenirContainer).GetComponent<Souvenir>();
    }
}
