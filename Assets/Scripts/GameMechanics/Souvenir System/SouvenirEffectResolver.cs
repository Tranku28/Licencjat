using System;
using Core;
using Core.Save_System;
using UnityEngine;

namespace SouvenirSystem
{
    public class SouvenirEffectResolver
    {
        private SaveSystem _saveSystem;
        public static Action<int> OnHarmonyValueUpdate;

        public SouvenirEffectResolver()
        {
            _saveSystem = DependencyResolver.Instance.GetType<SaveSystem>();
        }

        public void ApplyHarmony(int value)
        {
            Debug.Log("Harmony Applied");
            OnHarmonyValueUpdate?.Invoke(value);
        }

        public void RevertDay()
        {
            _saveSystem.LoadSave(_saveSystem.GetCurrentSave().LastDayData);
        }
    }
}
