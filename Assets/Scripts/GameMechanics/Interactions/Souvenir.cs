using System;
using System.Collections.Generic;
using Core;
using Core.Scriptable_Objects;
using Core.Scriptable_Objects.Souvenirs;
using GameMechanics.Interactions;
using SouvenirSystem;
using UnityEngine;
using UnityEngine.UI;

public class Souvenir : MonoBehaviour, IInteractable
{
    [SerializeField] private PassengerData souvenirData;
    [SerializeField] private List<SouvenirEffect> effects;
    private SouvenirEffectResolver _effectResolver;
    private Button _useButton;

    public static event Action<PassengerData> OnSouvenirInteracted;

    private void Awake()
    {
        _effectResolver = new();
    }

    public void Init(SouvenirEffectResolver resolver, Button useButton)
    {
        _useButton = useButton;
        _useButton.onClick.AddListener(ApplyEffects);
        _effectResolver = resolver;
    }

    public string GetName()
    {
        return souvenirData.souvenirData.name;
    }

    public void Interact()
    {
        OnSouvenirInteracted?.Invoke(souvenirData);
    }

    private void ApplyEffects()
    {
        bool destroy = false;

        foreach (SouvenirEffect effect in effects)
        {
            effect.Resolve(_effectResolver);

            if (effect.singleUse) 
                destroy = true;
        }

        if (destroy)
        {
            PlayerControls playerControls = DependencyResolver.Instance.GetType<PlayerControls>();
            playerControls.ForceOnEscapePressed();

            Destroy(gameObject);
        }

        _useButton.onClick.RemoveListener(ApplyEffects);
    }
}
