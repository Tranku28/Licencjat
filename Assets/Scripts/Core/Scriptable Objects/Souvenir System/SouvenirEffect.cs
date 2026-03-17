using UnityEngine;

namespace Core.Scriptable_Objects.Souvenirs
{    
    public abstract class SouvenirEffect : ScriptableObject
    {
        public bool singleUse;
        public abstract void Resolve(SouvenirEffectResolver resolver);
    }
}
