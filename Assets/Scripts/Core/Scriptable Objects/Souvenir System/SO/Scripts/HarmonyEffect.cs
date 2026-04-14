using Core.Scriptable_Objects.Souvenirs;
using UnityEngine;

namespace SouvenirSystem 
{
    [CreateAssetMenu(fileName = "HarmonyEffect", menuName = "SO/SouvenirEffect/HarmonyEffect")]
    public class HarmonyEffect : SouvenirEffect
    {
        public int harmonyValueUpdate;

        public override void Resolve(SouvenirEffectResolver resolver)
        {
            resolver.ApplyHarmony(harmonyValueUpdate);
        }
    }
}

