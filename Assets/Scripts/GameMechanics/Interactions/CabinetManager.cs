using System.Collections.Generic;
using Core.Save_System;
using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;
using GameMechanics.DayHandling;

namespace GameMechanics.Interactions
{
    public class CabinetManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> souvenirPositions = new();
        [SerializeField] private SouvenirAtlas souvenirAtlas;
        
        private void Start() => DayHandler.OnCabinetSpawned += SpawnSouvenir;
        private void OnDestroy() => DayHandler.OnCabinetSpawned -= SpawnSouvenir;

        private void SpawnSouvenir(int ID)
        {
            foreach (var souvenirPosition in souvenirPositions)
            {
                if (souvenirPosition.childCount != 0) continue;

                foreach (SouvenirData souvenirData in souvenirAtlas.souvenirs)
                {
                    if (souvenirData.souvenirID == ID)
                    {
                        Instantiate(souvenirData.souvenirPrefab, souvenirPosition,  false);
                    }
                }
                
                return;
            }
        }
    }
}
