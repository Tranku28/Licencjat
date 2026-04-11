using System;
using Core;
using Core.Save_System;
using UnityEngine;

namespace Core.Scriptable_Objects.Souvenirs
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
            OnHarmonyValueUpdate?.Invoke(value);
        }

        public void RevertDay()
        {
            Debug.Log("Revert day");
            _saveSystem.LoadPreviousDay(_saveSystem.GetCurrentSave().LastDayData);
        }
    }
}
