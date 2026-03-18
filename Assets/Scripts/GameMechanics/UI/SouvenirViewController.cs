using System;
using Core.Scriptable_Objects;
using Core.Scriptable_Objects.Souvenirs;
using GameMechanics.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SouvenirViewController : UIElement
{
    [SerializeField] private Button nextButton, previousButton, useButton;
    [SerializeField] private TMP_Text souvenirName, souvenirDescription, receivedFrom;
    [SerializeField] private Transform souvenirContainer;
    [SerializeField] private float souvenirContainerRotationSpeed;

    private Souvenir _lastSouvenir;

    private Quaternion souvenirPrefabContainerInitialRotation;

    private int _rotationDir = 0;

    public Button UseButton => useButton;

    private void Awake()
    {
        nextButton.onClick.AddListener(ShowNextSouvenirData);
        previousButton.onClick.AddListener(ShowPreviousSouvenirData);

        souvenirPrefabContainerInitialRotation = souvenirContainer.transform.rotation;
        Debug.Log(UseButton);
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
    private void LoadSouvenirWindow(SouvenirData obj)
    {
        _rotationDir = 0;

        if (_lastSouvenir != null) Destroy(_lastSouvenir.gameObject);

        souvenirName.text = obj.name;
        souvenirDescription.text = obj.souvenirDescription;
        receivedFrom.text = $"Received from: {obj.receivedFrom}";
        _lastSouvenir = Instantiate(obj.souvenirPrefab, souvenirContainer).GetComponent<Souvenir>();
    }
}
