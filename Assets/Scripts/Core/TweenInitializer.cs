using Core;
using DG.Tweening;
using UnityEngine;

[InitializeSystem("DO Tween Global Initializer")]
public class TweenInitializer : BaseSystem
{
    protected override void Awake()
    {
        base.Awake();
        DOTween.Init();
    }
}
