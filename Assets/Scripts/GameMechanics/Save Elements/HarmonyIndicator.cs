using System;
using GameMechanics.UI;
using TMPro;
using UnityEngine;

public class HarmonyIndicator : MonoBehaviour, ISaveElement
{
    private const int MAX_HARMONY = 100;
    private const int MIN_HARMONY = 0;
    //TODO: deserialize
    [SerializeField] private int _harmonyStatus = 100;

    private void Awake()
    {
        (this as ISaveElement).Register(this);
    }

    private void Start()
    {
        DialogueManager.OnHarmonyDecreased += UpdateHarmonyStatus;
    }

    void OnDestroy()
    {
        DialogueManager.OnHarmonyDecreased -= UpdateHarmonyStatus;
    }

    private void UpdateHarmonyStatus(int value)
    {
        _harmonyStatus += value;
        _harmonyStatus = Mathf.Clamp(_harmonyStatus, MIN_HARMONY, MAX_HARMONY);
    }

    public void LoadSave(GameSaveData gameSaveData)
    {
        _harmonyStatus = gameSaveData.HarmonyStatus;
    }

    public void SaveData(GameSaveData gameSaveData)
    {
        gameSaveData.HarmonyStatus = _harmonyStatus;
    }
}
