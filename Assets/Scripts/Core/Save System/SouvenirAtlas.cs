using System.Collections.Generic;
using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;

namespace Core.Save_System
{
    [CreateAssetMenu(fileName = "SouvenirAtlas", menuName = "Scriptable Objects/SouvenirAtlas")]
    public class SouvenirAtlas : ScriptableObject
    {
        public List<SouvenirData> souvenirs = new();

        public bool GetSouvenirDataFromIndex(int index, out SouvenirData data)
        {
            foreach (SouvenirData souvenirData in souvenirs)
            {
                if (souvenirData.souvenirID == index) 
                {
                    data = souvenirData;
                    return true;
                }
            }

            data = default;
            return false;
        }
    }
}
