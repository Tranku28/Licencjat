using UnityEngine;

namespace Core
{
    [InitializeSystem("App Global Settings")]
    public class ApplicationGlobalSettings : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }   
}
