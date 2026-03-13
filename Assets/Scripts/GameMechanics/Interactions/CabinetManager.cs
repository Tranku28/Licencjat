using System.Collections.Generic;
using Core.Save_System;
using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;
using GameMechanics.DayHandling;
using SouvenirSystem;
using UnityEngine.UI;

namespace GameMechanics.Interactions
{
    public class CabinetManager : MonoBehaviour, ISaveElement
    {
        [SerializeField] private List<Transform> souvenirPositions = new();
        [SerializeField] private SouvenirAtlas souvenirAtlas;
        [SerializeField] private SouvenirViewController souvenirView;
        private SouvenirEffectResolver _souvenirEffectResolver;

        private List<Souvenir> _souvenirs = new();

        private void Awake()
        {
            (this as ISaveElement).Register(this);
            _souvenirEffectResolver = new();
        }

        public void LoadSave(GameSaveData gameSaveData)
        {
            SpawnSouvenirs(gameSaveData.CollectedSouvenirIdList.ToArray());
        }

        //TODO: Refactor to clear in another method on reload
        public void SaveData(GameSaveData gameSaveData)
        {
            foreach(Souvenir souvenir in _souvenirs)
            {
                Destroy(souvenir.gameObject);
            }

            _souvenirs.Clear();
        }

        private void SpawnSouvenirs(int[] IDs)
        {
            for (int i=0; i < IDs.Length; i++)
            {
                if (souvenirAtlas.GetSouvenirDataFromIndex(i, out SouvenirData saveData))
                {
                    Debug.Log(_souvenirEffectResolver);
                    Debug.Log(souvenirView.UseButton);
                    Souvenir newSouvenir = Instantiate(saveData.souvenirPrefab, souvenirPositions[i]).GetComponent<Souvenir>();
                    newSouvenir.Init(_souvenirEffectResolver, souvenirView.UseButton);
                    _souvenirs.Add(newSouvenir);
                }
            }
        }
    }
}
