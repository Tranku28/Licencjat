using System;
using System.Collections.Generic;
using Core.Scriptable_Objects;
using GameMechanics.Interactions;
using SouvenirSystem;
using UnityEngine;

public class Souvenir : MonoBehaviour, IInteractable
{
    [SerializeField] private PassengerData souvenirData;
    [SerializeField] private List<SouvenirEffect> effects;
    private SouvenirEffectResolver _effectResolver;

    public static event Action<PassengerData> OnSouvenirInteracted;

    private void Awake()
    {
        _effectResolver = new();
    }

    private void Start()
    {
        BlinkPanelUI.OnSummaryDisplay += ApplyPassiveEffects;
    }

    public void Init(SouvenirEffectResolver resolver)
    {
        _effectResolver = resolver;
    }

    private void OnDestroy()
    {
        BlinkPanelUI.OnSummaryDisplay -= ApplyPassiveEffects;
    }

    public string GetName()
    {
        return souvenirData.souvenirName;
    }

    public void Interact()
    {
        OnSouvenirInteracted?.Invoke(souvenirData);
    }

    private void ApplyPassiveEffects()
    {
        foreach (SouvenirEffect effect in effects)
        {
            if (!effect.singleUse)
            {
                effect.Resolve(_effectResolver);
            }
        }
    }

    private void UseSouvenir()
    {
        //TODO: save file souvenir array needs to be updated by 
    }
}
