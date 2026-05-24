using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Save_System;
using Core.Scriptable_Objects;
using Core.Scriptable_Objects.Souvenirs;
using DG.Tweening;
using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using FMOD.Studio;
using FMODUnity;
using GameMechanics.Interactions;
using SouvenirSystem;
using UnityEngine;
using UnityEngine.UI;

public class Souvenir : MonoBehaviour, IInteractable
{
    private static WaitForSeconds _waitForSeconds1 = new(1f);
    [SerializeField] private SouvenirData souvenirData;
    [SerializeField] private bool opensSouvenirUI = true;
    [field: SerializeField] private EventReference souvenirInteractionSound {get; set;}
    private EventInstance _souvenirEventInstance;
    private SouvenirEffect _effect;
    private SouvenirEffectResolver _effectResolver;
    public SouvenirEffect Effect => _effect; 
    private Button _useButton;
    private bool _soundPlays;
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
        if (!opensSouvenirUI)
        {
            if (!souvenirInteractionSound.IsNull && !_soundPlays)
            {
                _soundPlays = true;
                StartCoroutine(WaitTillSoundPlayAgain());
            }

            return;
        }

        OnSouvenirInteracted?.Invoke(souvenirData);
    }

    private IEnumerator WaitTillSoundPlayAgain()
    {
        AudioManager.Instance.PlayOneShot(souvenirInteractionSound, transform.position);
        yield return _waitForSeconds1;

        _soundPlays = false;
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
