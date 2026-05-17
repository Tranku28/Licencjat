using System;
using Core.Scriptable_Objects.Souvenirs;
using GameMechanics.Interactions;
using GameMechanics.UI;
using UnityEngine;

public class HarmonyIndicator : MonoBehaviour, ISaveElement, IInteractable
{
    private int _harmonyStatus = 100;
    [SerializeField] private Transform topEndpoint, bottomEndpoint;
    [SerializeField] private Transform pointerTransform;
    private const int TOTAL_STEPS = 100;
    private float _bottomYPosition;
    private float _distanceStep;
    private int _cachedHarmony;
    private const int MAX_HARMONY = 100;
    private const int MIN_HARMONY = 0;

    public static Action<int> OnHarmonyValueSet;

    private void Awake()
    {
        (this as ISaveElement).Register(this);

        _distanceStep = Vector3.Distance(topEndpoint.position, bottomEndpoint.position) / TOTAL_STEPS;
        _bottomYPosition = bottomEndpoint.position.y;
    }

    private void Start()
    {
        DialogueManager.OnHarmonyDecreased += CacheHarmonyStatus;
        SouvenirEffectResolver.OnHarmonyValueUpdate += CacheHarmonyStatus;
    }

    void OnDestroy()
    {
        DialogueManager.OnHarmonyDecreased -= CacheHarmonyStatus;
        SouvenirEffectResolver.OnHarmonyValueUpdate -= CacheHarmonyStatus;
    }

    private void CacheHarmonyStatus(int value) => _cachedHarmony += value;

    private void UpdateHarmonyPointerPosition(int value)
    {
        float yPos = _bottomYPosition + _distanceStep * value;
        pointerTransform.transform.position = new Vector3(pointerTransform.position.x, yPos, pointerTransform.position.z);
    }

    public void LoadSave(GameSaveData gameSaveData)
    {
        _harmonyStatus = gameSaveData.HarmonyStatus;
        _cachedHarmony = 0;

        UpdateHarmonyPointerPosition(_harmonyStatus);
        OnHarmonyValueSet?.Invoke(_harmonyStatus);
    }

    public void SaveData(GameSaveData gameSaveData)
    {
        gameSaveData.HarmonyStatus = Mathf.Clamp(_harmonyStatus + _cachedHarmony, MIN_HARMONY, MAX_HARMONY);
        Debug.Log($"Save Harmony: {gameSaveData.HarmonyStatus}");
    }

    public void Interact()
    {
        
    }

    public string GetName()
    {
        return $"Harmony: {_harmonyStatus}%";
    }
}
