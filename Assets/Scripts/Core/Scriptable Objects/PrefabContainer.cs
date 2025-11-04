using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "PrefabContainer", menuName = "Scriptable Objects/PrefabContainer")]
    public class PrefabContainer : ScriptableObject
    {
        public List<GameObject> prefabs = new();
        
    }
}
