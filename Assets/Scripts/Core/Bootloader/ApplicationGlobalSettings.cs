using UnityEngine;

public class ApplicationGlobalSettings : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
