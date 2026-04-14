using System;
using Core.Save_System;

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
            _saveSystem.LoadPreviousDay(_saveSystem.GetCurrentSave().LastDayData);
        }
    }
}
