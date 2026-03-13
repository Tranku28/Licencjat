using UnityEngine;

namespace SouvenirSystem
{    
    public abstract class SouvenirEffect : ScriptableObject
    {
        public bool singleUse;
        public abstract void Resolve(SouvenirEffectResolver resolver);
    }
}
