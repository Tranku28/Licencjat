using Core;
using Core.Save_System;
using UnityEngine;

namespace SouvenirSystem
{
    public class SouvenirEffectResolver
    {
        private SaveSystem _saveSystem;

        public SouvenirEffectResolver()
        {
            _saveSystem = DependencyResolver.Instance.GetType<SaveSystem>();
        }

        public void ApplyHarmony(int value)
        {
            GameSaveData gameSaveData = _saveSystem.GetCurrentSave();

            gameSaveData.HarmonyStatus += value;
        }

        public void RevertDay()
        {
            _saveSystem.LoadSave(_saveSystem.GetCurrentSave().LastDayData);
        }
    }
}
