using System.Collections;
using UnityEngine;

public class CameraBuffer : MonoBehaviour
{
    private void Awake()
    {
        BuffEnvironment();
    }

    private void BuffEnvironment()
    {
        Transform cameraTransform = GetComponent<Transform>();
        Quaternion cachedRotation = cameraTransform.rotation;

        for (int i=1; i<5; i++)
        {
            cameraTransform.rotation *= new Quaternion(cachedRotation.eulerAngles.x, i*90, cachedRotation.eulerAngles.z, cachedRotation.w);
        }
    }
}
