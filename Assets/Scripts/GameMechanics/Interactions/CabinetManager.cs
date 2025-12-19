using System.Collections.Generic;
using UnityEngine;
using GameMechanics.DayHandling;

namespace GameMechanics.Interactions
{
    public class CabinetManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> souvenirPositions = new();
        
        private void Start() => DayHandler.OnCabinetSpawned += SpawnSouvenir;
        private void OnDestroy() => DayHandler.OnCabinetSpawned -= SpawnSouvenir;

        private void SpawnSouvenir(GameObject obj)
        {
            foreach (var souvenirPosition in souvenirPositions)
            {
                if (souvenirPosition.childCount != 0) continue;

                Instantiate(obj, souvenirPosition,  false);
                return;
            }
        }
    }
}
