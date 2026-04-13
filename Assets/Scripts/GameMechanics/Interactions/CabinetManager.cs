using System.Collections.Generic;
using Core.Save_System;
using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;
using GameMechanics.UI;

namespace GameMechanics.Interactions
{
    public class CabinetManager : MonoBehaviour, ISaveElement
    {
        [SerializeField] private List<Transform> souvenirPositions = new();
        [SerializeField] private SouvenirAtlas souvenirAtlas;
        [SerializeField] private SouvenirViewController souvenirView;
        private SouvenirEffectResolver _souvenirEffectResolver;
        private List<int> souvenirIdList = new();
        private int _cachedSouvenir;

        private List<Souvenir> _souvenirs = new();

        private void Awake()
        {
            (this as ISaveElement).Register(this);
            _souvenirEffectResolver = new();

            DialogueManager.OnSouvenirReceived += CacheSouvenirId;

            SpawnSouvenirs();
        }

        private void OnDestroy()
        {
            DialogueManager.OnSouvenirReceived -= CacheSouvenirId;
        }

        private void CacheSouvenirId(int id)
        {
            Debug.Log("Caching souvenir Id...");
            if (!souvenirIdList.Contains(id))
            {
                Debug.Log("Souvenir Added");
                souvenirIdList.Add(id);
            }
        }

        public void LoadSave(GameSaveData gameSaveData)
        {
            souvenirIdList.Clear();
            souvenirIdList.AddRange(gameSaveData.CollectedSouvenirIdList);

            EnableSouvenirs(souvenirIdList);
        }

        public void SaveData(GameSaveData gameSaveData)
        {
            gameSaveData.CollectedSouvenirIdList = souvenirIdList;

            EnableSouvenirs(souvenirIdList);
        }
        private void SpawnSouvenirs()
        {
            int position = 0;
            foreach (SouvenirData souvenirData in souvenirAtlas.souvenirs)
            {
                Souvenir newSouvenir = Instantiate(souvenirData.souvenirPrefab, souvenirPositions[position]).GetComponent<Souvenir>();
                newSouvenir.OnSouvenirused += OnUseDeleteSouvenir;
                newSouvenir.Init(_souvenirEffectResolver, souvenirView.UseButton, souvenirData.souvenirEffects, souvenirData.souvenirID);
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
            souvenir.OnSouvenirused -= OnUseDeleteSouvenir;
            _souvenirs.Remove(souvenir);
        }
    }
}
