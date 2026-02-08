using Core;
using Core.Save_System;
using UnityEngine;
using UnityEngine.UI;

public class SaveDeleteUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject visuals;
    [SerializeField] private Button deleteButton, cancelButton; 
    private int _selectedSaveIndex;

    private void Awake()
    {
        deleteButton.onClick.AddListener(DeleteSave);
        cancelButton.onClick.AddListener(Cancel);
    }

    private void Start()
    {
        SaveDisplayerUI.OnDeleteSaveClicked += OnDeleteSaveClicked;
    }

    private void OnDestroy()
    {
        SaveDisplayerUI.OnDeleteSaveClicked -= OnDeleteSaveClicked;

        deleteButton.onClick.RemoveListener(DeleteSave);
        cancelButton.onClick.RemoveListener(Cancel);
    }

    private void OnDeleteSaveClicked(SaveDisplayerUI saveDisplayerUI)
    {
        visuals.SetActive(true);
        _selectedSaveIndex = saveDisplayerUI.Index;
        Debug.Log(_selectedSaveIndex);
    }

    private void DeleteSave()
    {
        SaveSystem saveSystem = DependencyResolver.Instance.GetType<SaveSystem>();
        saveSystem.DeleteSave(_selectedSaveIndex);
        visuals.SetActive(false);
    }

    private void Cancel()
    {
        visuals.SetActive(false);
    }
}
