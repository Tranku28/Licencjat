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

    private Quaternion souvenirPrefabContainerInitialRotation;

    private int rotationDir = 0;

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
        if (rotationDir == 0) return;

        souvenirContainer.rotation *= Quaternion.Euler(0, rotationDir * souvenirContainerRotationSpeed, 0);
    }

    private void OnDestroy()
    {
        nextButton.onClick.RemoveListener(ShowNextSouvenirData);
        previousButton.onClick.RemoveListener(ShowPreviousSouvenirData);
        
        Souvenir.OnSouvenirInteracted -= LoadSouvenirWindow;
    }

    private void ShowNextSouvenirData()
    {
        if (rotationDir == -1) 
        {
            rotationDir = 0;
            return;
        }

        rotationDir = 1;
    }

    private void ShowPreviousSouvenirData()
    {
        if (rotationDir == 1) 
        {
            rotationDir = 0;
            return;
        }

        rotationDir = -1;
    }
    
    private void LoadSouvenirWindow(PassengerData obj)
    {
        souvenirName.text = obj.souvenirName;
        souvenirDescription.text = obj.souvenirNote;
        receivedFrom.text = $"Received from: {obj.passengerName} {obj.passengerSurname}";
        Instantiate(obj.souvenirData.souvenirPrefab, souvenirContainer);
    }
}
