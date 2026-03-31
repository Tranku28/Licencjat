using Unity.Cinemachine;
using UnityEngine;

public class CinemachineBrainController : MonoBehaviour
{
    private CinemachineBrain _cinemachineBrain;
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineBlendDefinition defaultBlendSettings;

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

    public void BlendSetCut()
    {
        _cinemachineBrain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
    }

    public void BlendCustom(CinemachineBlendDefinition blendDefinition)
    {
        _cinemachineBrain.DefaultBlend = blendDefinition;
    }
    
    public void BlendRestoreDefaultSettings()
    {
        _cinemachineBrain.DefaultBlend = defaultBlendSettings;
    }

    public void PrioritizeCamera(CinemachineCamera cam)
    {
        cam.Prioritize();
    }
}
