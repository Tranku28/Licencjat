using UnityEngine;

namespace Core
{
    [InitializeSystem("App Global Settings")]
    public class ApplicationGlobalSettings : BaseSystem
    {
        protected override void Awake()
        {
            base.Awake();
            QualitySettings.vSyncCount = 1;

#if UNITY_EDITOR
            Cursor.visible = true;
#endif 
        }

        public void CursorActive (bool value) => Cursor.visible = value;
    } 
}
