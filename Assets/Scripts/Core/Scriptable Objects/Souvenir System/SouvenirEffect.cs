using FMODUnity;
using UnityEngine;

namespace Core.Scriptable_Objects.Souvenirs
{    
    public abstract class SouvenirEffect : ScriptableObject
    {
        public bool singleUse;
        public EventReference useSound;
        public abstract void Resolve(SouvenirEffectResolver resolver);
    }
}
