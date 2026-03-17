using System.Collections.Generic;
using UnityEngine;

namespace Core.Scriptable_Objects.Souvenirs
{
    [CreateAssetMenu(fileName = "SouvenirData", menuName = "Scriptable Objects/SouvenirData")]
    public class SouvenirData : ScriptableObject
    {
        public int souvenirID;
        public new string name;
        public GameObject souvenirPrefab;
        public List<SouvenirEffect> souvenirEffects;
        [TextArea(3, 5)]
        public string souvenirDescription;
        [TextArea(2,3)]
        public string souvenirEffectDescription;
    }
}
