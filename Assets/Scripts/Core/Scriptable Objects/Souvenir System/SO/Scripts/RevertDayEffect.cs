using Core;
using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;

namespace SouvenirSystem 
{
    [CreateAssetMenu(fileName = "RevertDayEffect", menuName = "SO/SouvenirEffect/RevertDayEffect")]
    public class RevertDayEffect : SouvenirEffect
    {
        public override void Resolve(SouvenirEffectResolver resolver)
        {
            resolver.RevertDay();   
        }
    }
}

