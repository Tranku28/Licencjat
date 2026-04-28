using Core.Scriptable_Objects.Souvenirs;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessController : MonoBehaviour
{
    [SerializeField] private CinemachineVolumeSettings playerCameraVolume;
    [SerializeField] private VolumeProfile globalVolume;
    [SerializeField] private VolumeProfile rewindVolume;

    private void Awake()
    {
        playerCameraVolume.Profile = globalVolume;

        SouvenirEffectResolver.OnRewind += OnRewind;

        InvokeRepeating(nameof(OnRewind), 5f, 1f);
    }

    private void OnDestroy()
    {
        SouvenirEffectResolver.OnRewind -= OnRewind;
    }

    private void OnRewind()
    {
        playerCameraVolume.Profile = rewindVolume;
    }
}