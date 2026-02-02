using System;
using GameMechanics.UI;
using TMPro;
using UnityEngine;

public class HarmonyIndicator : MonoBehaviour, ISaveElement
{
    //TODO: deserialize
    [SerializeField] private int _harmonyStatus = 100;
    private const int MAX_HARMONY = 100;
    private const int MIN_HARMONY = 0;

    private float _yDefaultScale;

    private void Awake()
    {
        (this as ISaveElement).Register(this);
        _yDefaultScale = transform.localScale.y;
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

        UpdateHarmonyLevelVisual(_harmonyStatus);

        void UpdateHarmonyLevelVisual(int value)
        {
            float yScale = _yDefaultScale * ((float)value / MAX_HARMONY);
            transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);
        }
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
