using UnityEngine;

namespace Core.Scriptable_Objects.Souvenirs
{
    [CreateAssetMenu(fileName = "SouvenirData", menuName = "Scriptable Objects/SouvenirData")]
    public class SouvenirData : ScriptableObject
    {
        public int souvenirID;
        public GameObject souvenirPrefab;
    }
}
