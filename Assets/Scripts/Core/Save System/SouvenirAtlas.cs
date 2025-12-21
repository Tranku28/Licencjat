using System.Collections.Generic;
using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;

namespace Core.Save_System
{
    [CreateAssetMenu(fileName = "SouvenirAtlas", menuName = "Scriptable Objects/SouvenirAtlas")]
    public class SouvenirAtlas : ScriptableObject
    {
        public List<SouvenirData> souvenirs = new();
    }
}
