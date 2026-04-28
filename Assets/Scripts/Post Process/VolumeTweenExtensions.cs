using UnityEngine.Rendering;
using DG.Tweening;

public static class VolumeTweenExtensions
{
    public static Tweener DOWeight(this Volume volume, float endValue, float duration)
    {
        return DOTween.To(
            () => volume.weight,
            x => volume.weight = x,
            endValue,
            duration
        );
    }
}