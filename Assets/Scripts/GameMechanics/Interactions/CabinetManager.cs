using System.Collections.Generic;
using Core.Save_System;
using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;
using GameMechanics.DayHandling;

namespace GameMechanics.Interactions
{
    public class CabinetManager : MonoBehaviour, ISaveElement
    {
        [SerializeField] private List<Transform> souvenirPositions = new();
        [SerializeField] private SouvenirAtlas souvenirAtlas;

        private List<Souvenir> _souvenirs = new();

        private void Awake()
        {
            (this as ISaveElement).Register(this);
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
                Destroy(souvenir);
            }

            _souvenirs.Clear();
        }

        private void SpawnSouvenirs(int[] IDs)
        {
            for (int i=0; i < IDs.Length; i++)
            {
                if (souvenirAtlas.GetSouvenirDataFromIndex(i, out SouvenirData saveData))
                {
                    Souvenir newSouvenir = Instantiate(saveData.souvenirPrefab, souvenirPositions[i]).GetComponent<Souvenir>();
                    _souvenirs.Add(newSouvenir);
                }
            }
        }
    }
}
