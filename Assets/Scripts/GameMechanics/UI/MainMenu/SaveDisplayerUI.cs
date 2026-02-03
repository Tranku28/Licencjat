using System;
using Core;
using Core.Save_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveDisplayerUI : MonoBehaviour
{
    [field: SerializeField] public Button saveSelectedButton {get; private set;}
    [field: SerializeField] public TMP_Text saveName {get; private set;}
    [field: SerializeField] public TMP_Text dateSaved {get; private set;}
    [field: SerializeField] public TMP_Text inGameDay {get; private set;}
    [field: SerializeField] public TMP_Text harmonyStatus {get; private set;}
    [field: SerializeField] public Button deleteSaveButton {get; private set;}

    public static Action<SaveDisplayerUI> OnDeleteSaveClicked;

    public int Index {get; set;}

    private void Awake()
    {
        gameObject.SetActive(false);

        deleteSaveButton.onClick.AddListener(DeleteSave);
    }

    private void OnDestroy()
    {
        deleteSaveButton.onClick.RemoveListener(DeleteSave);
    }

    private void DeleteSave()
    {
        OnDeleteSaveClicked?.Invoke(this);
    }
}
