using System.Collections.Generic;
using Core.Save_System;
using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;
using GameMechanics.UI;
using System;
using UnityEngine.UI;
using System.Linq;
using Core;

namespace GameMechanics.Interactions
{
    public class CabinetManager : MonoBehaviour, ISaveElement
    {
        [SerializeField] private List<Transform> souvenirPositions = new();
        [SerializeField] private SouvenirAtlas souvenirAtlas;
        [SerializeField] private SouvenirViewController souvenirView;
        [SerializeField] private Button useButton;
        private SouvenirEffectResolver _souvenirEffectResolver;
        private List<int> souvenirIdList = new();
        private int _cachedSouvenir;

        private List<Souvenir> _souvenirs = new();

        private void Awake()
        {
            (this as ISaveElement).Register(this);
            _souvenirEffectResolver = new();

            DialogueManager.OnSouvenirReceived += CacheSouvenirId;
            Souvenir.OnSouvenirInteracted += CacheSouvenir;
            GameStateMachine.OnMenuReturned += DisableSouvenirs;

            SpawnSouvenirs();
            SetupButton();
        }

        private void SetupButton()
        {
            useButton.onClick.AddListener(UseSouvenir);
        }

        private void OnDestroy()
        {
            DialogueManager.OnSouvenirReceived -= CacheSouvenirId;
            Souvenir.OnSouvenirInteracted -= CacheSouvenir;
            useButton.onClick.RemoveListener(UseSouvenir);

            foreach(Souvenir souvenir in _souvenirs)
                souvenir.OnSouvenirUsed -= OnUseDeleteSouvenir;
        }

        private void DisableSouvenirs()
        {
            if (_souvenirs.Count == 0) return;

            foreach(Souvenir souvenir in _souvenirs)
            {
                souvenir.gameObject.SetActive(false);
            }
        }

        private void UseSouvenir()
        {
            Souvenir selectedSouvenir = _souvenirs.First(x => x.ID == _cachedSouvenir);
            selectedSouvenir.ApplyEffects();
        }

        private void CacheSouvenir(SouvenirData data)
        {
            _cachedSouvenir = data.souvenirID;
        }

        private void CacheSouvenirId(int id)
        {
            if (!souvenirIdList.Contains(id))
                souvenirIdList.Add(id);
        }

        public void LoadSave(GameSaveData gameSaveData)
        {
            souvenirIdList.Clear();
            souvenirIdList.AddRange(gameSaveData.CollectedSouvenirIdList);

            EnableSouvenirs(souvenirIdList);

            ApplySouvenirPassiveEffects();
        }

        private void ApplySouvenirPassiveEffects()
        {
            foreach (Souvenir souvenir in _souvenirs)
            {
                if (!souvenir.gameObject.activeSelf) continue;
                
                if (souvenir.Effect == null) continue;

                if (souvenir.Effect.passive)
                    souvenir.Effect.Resolve(_souvenirEffectResolver);
            }
        }

        public void SaveData(GameSaveData gameSaveData)
        {
            gameSaveData.CollectedSouvenirIdList = new List<int>(souvenirIdList);
        }
        private void SpawnSouvenirs()
        {
            int position = 0;
            foreach (SouvenirData souvenirData in souvenirAtlas.souvenirs)
            {
                SouvenirEffect effect = souvenirData.souvenirEffect != null ? souvenirData.souvenirEffect : null;
                Souvenir newSouvenir = Instantiate(souvenirData.souvenirPrefab, souvenirPositions[position]).GetComponent<Souvenir>();
                newSouvenir.OnSouvenirUsed += OnUseDeleteSouvenir;
                newSouvenir.Init(_souvenirEffectResolver, effect, souvenirData.souvenirID);
                _souvenirs.Add(newSouvenir);

                newSouvenir.gameObject.SetActive(false);

                position++;
            }
        }

        private void EnableSouvenirs(List<int> IDs)
        {
            foreach (Souvenir souvenir in _souvenirs)
            {
                if (IDs.Contains(souvenir.ID))
                {
                    souvenir.gameObject.SetActive(true);
                    continue;
                }

                souvenir.gameObject.SetActive(false);
            }
        }

        private void OnUseDeleteSouvenir(Souvenir souvenir)
        {
            souvenir.OnSouvenirUsed -= OnUseDeleteSouvenir;
        
            _souvenirs.Remove(souvenir);
            souvenirIdList.Remove(souvenir.ID);
        }
    }
}
