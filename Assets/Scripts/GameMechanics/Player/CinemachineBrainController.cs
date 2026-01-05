using Unity.Cinemachine;
using UnityEngine;

public class CinemachineBrainController : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;

    private void Update()
    {
        if (cinemachineBrain.IsBlending) return;

        if (!cinemachineBrain.IsBlending)
        {
            Debug.Log("Blend Complete");
        }
    }
}
