using UnityEngine;

namespace Core
{
    [InitializeSystem("App Global Settings")]
    public class ApplicationGlobalSettings : MonoBehaviour
    {
        private void Awake()
        {
            QualitySettings.vSyncCount = 1;
            Cursor.visible = false;

#if UNITY_EDITOR
            Cursor.visible = true;
#endif 
        }
    } 
}
