using Unity.Cinemachine;
using UnityEngine;

public class CinemachineBrainController : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineCamera playerCamera;

    public CinemachineCamera PlayerCamera { get => playerCamera;}

    public static CinemachineBrainController Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        playerCamera.Prioritize();
    }
}
