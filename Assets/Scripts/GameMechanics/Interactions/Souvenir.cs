using System;
using System.Collections.Generic;
using Core;
using Core.Save_System;
using Core.Scriptable_Objects;
using Core.Scriptable_Objects.Souvenirs;
using GameMechanics.Interactions;
using SouvenirSystem;
using UnityEngine;
using UnityEngine.UI;

public class Souvenir : MonoBehaviour, IInteractable
{
    [SerializeField] private SouvenirData souvenirData;
    private List<SouvenirEffect> _effects;
    private SouvenirEffectResolver _effectResolver;
    private Button _useButton;
    public int ID { get; private set; }

    public event Action<Souvenir> OnSouvenirused;

    public static event Action<SouvenirData> OnSouvenirInteracted;
    public static event Action OnSouvenirUiQuit;

    private void Awake()
    {
        _effectResolver = new();
    }

    public void Init(SouvenirEffectResolver resolver, Button useButton, List<SouvenirEffect> effects, int id)
    {
        _useButton = useButton;
        _useButton.onClick.AddListener(ApplyEffects);
        _effectResolver = resolver;
        _effects = effects;
        ID = id;
    }

    public string GetName()
    {
        return souvenirData.name;
    }

    public void Interact()
    {
        OnSouvenirInteracted?.Invoke(souvenirData);
    }

    private void ApplyEffects()
    {
        bool destroy = false;

        foreach (SouvenirEffect effect in _effects)
        {
            effect.Resolve(_effectResolver);

            if (effect.singleUse)
            {
                destroy = true;
                AudioManager.Instance.PlayOneShot(effect.useSound, transform.position);
            }
        }

        if (destroy)
        {
            DependencyResolver.Instance.GetType<SaveSystem>().ForceDeleteSaveSouvenirData(souvenirData.souvenirID);

            OnSouvenirUiQuit?.Invoke();
            OnSouvenirused?.Invoke(this);
            Destroy(gameObject);
        }

        _useButton.onClick.RemoveListener(ApplyEffects);
    }
}
