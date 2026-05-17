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
    private SouvenirEffect _effect;
    private SouvenirEffectResolver _effectResolver;
    public SouvenirEffect Effect => _effect; 
    private Button _useButton;
    public int ID { get; private set; }

    public event Action<Souvenir> OnSouvenirUsed;

    public static event Action<SouvenirData> OnSouvenirInteracted;
    public static event Action OnSouvenirUiQuit;

    private void Awake()
    {
        _effectResolver = new();
    }

    public void Init(SouvenirEffectResolver resolver, SouvenirEffect effect, int id)
    {
        _effectResolver = resolver;
        _effect = effect;
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

    public void ApplyEffects()
    {
        bool destroy = false;

        _effect.Resolve(_effectResolver);

            if (_effect.singleUse)
            {
                Debug.Log("Resolving: " + _effect);
                destroy = true;
                AudioManager.Instance.PlayOneShot(_effect.useSound, transform.position);
            }

        if (destroy)
        {
            DependencyResolver.Instance.GetType<SaveSystem>().ForceDeleteSaveSouvenirData(souvenirData.souvenirID);

            OnSouvenirUiQuit?.Invoke();
            OnSouvenirUsed?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
