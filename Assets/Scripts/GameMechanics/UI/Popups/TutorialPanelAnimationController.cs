using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TutorialPanelAnimationController : MonoBehaviour
{
    [SerializeField] private float panelVisibilityDuration = 15f;
    private Animator _panelAnimator;
    private float _currentTime;

    private static readonly int Show = Animator.StringToHash("Show");

    void Awake()
    {
        _panelAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        _panelAnimator.SetBool(Show, true);
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime > panelVisibilityDuration)
        {
            _panelAnimator.SetBool(Show, false);
            _currentTime = 0;
        }
    }

    public void OnCloseAnimationEnd() => enabled = false;

    private void OnDisable()
    {
        _currentTime = 0;
    }
}
