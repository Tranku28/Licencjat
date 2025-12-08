using UnityEngine;
using GameMechanics.DayHandling;

namespace GameMechanics.Interactions
{
    public class CabinetManager : MonoBehaviour
    {
        private void Start() => DayHandler.OnCabinetSpawned += SpawnSouvenir;
        private void OnDestroy() => DayHandler.OnCabinetSpawned -= SpawnSouvenir;

        // TODO: setup spawning positions
        
        private void SpawnSouvenir(GameObject obj)
        {
            Instantiate(obj, transform,  false);
        }
    }
}
