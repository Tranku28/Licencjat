using Unity.Cinemachine;
using UnityEngine;

public class CinemachineBrainController : MonoBehaviour
{
    private CinemachineBrain _cinemachineBrain;
    [SerializeField] private CinemachineCamera playerCamera;

    public CinemachineCamera PlayerCamera { get => playerCamera;}
    public CinemachineBrain Brain => _cinemachineBrain;

    public static CinemachineBrainController Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        _cinemachineBrain = GetComponent<CinemachineBrain>();

        playerCamera.Prioritize();
    }
}
