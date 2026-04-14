using UnityEngine;
using UnityEngine.EventSystems;
using Core;
using DG.Tweening;

namespace GameMechanics.UI.MainMenu
{
    public class QuitGameCandleHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private ParticleSystem candlelightParticles;
        [SerializeField] private ParticleSystem candlelightSmoke;
        [SerializeField] private Light candlelight;
        [SerializeField] private float tweenDuration;
        [SerializeField] private bool addQuitDelayOnEnd;
        [SerializeField] private Animator animator;
        private Collider _candleCollider;
        private float _initSmokeParticleSize;

        private void Awake()
        {
            _candleCollider = GetComponent<Collider>();
            _initSmokeParticleSize = candlelightSmoke.main.startSize.constant;
            var m = candlelightSmoke.main;
            m.startSize = 0;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.candleHover, transform.position);
            animator.SetBool("Hovered", true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.candlePutDown, transform.position);
            animator.SetBool("Hovered", false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            animator.SetBool("Hover", false);
            _candleCollider.enabled = false;

            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.candleBlow, transform.position);

            var mainModule = candlelightParticles.main;
            var smokeMainModule = candlelightSmoke.main;

            Sequence sequence = DOTween.Sequence();

            sequence.Join(
                DOTween.To(
                    () => mainModule.startSize.constant,
                    x =>
                    {
                        var m = candlelightParticles.main;

                        m.startSize = x;
                    },
                    0f,
                    tweenDuration
                )
            );

            sequence.Join(
                DOTween.To(
                    () => candlelightSmoke.main.startSize.constant,
                    x =>
                    {
                        var m = candlelightSmoke.main;
                        m.startSize = x;
                    },
                    _initSmokeParticleSize,
                    tweenDuration
                )
            );

            sequence.Join(
                candlelight.DOIntensity(0f, tweenDuration*2)
            );

            if (addQuitDelayOnEnd)
            {
                sequence.AppendInterval(tweenDuration);
            }

            sequence.OnComplete(() =>
                {
                    Debug.Log("App Quit");
                    Application.Quit();
                }
            );
        }
    }
}
