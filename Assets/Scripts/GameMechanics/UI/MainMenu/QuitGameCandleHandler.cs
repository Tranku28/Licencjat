using UnityEngine;
using UnityEngine.EventSystems;
using Core;
using DG.Tweening;

namespace GameMechanics.UI.MainMenu
{
    public class QuitGameCandleHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private ParticleSystem candlelightParticles;
        [SerializeField] private Light candlelight;
        [SerializeField] private float tweenDuration;
        [SerializeField] private bool addQuitDelayOnEnd;
        [SerializeField] private Animator animator;
        private Collider _candleCollider;

        private void Awake()
        {
            _candleCollider = GetComponent<Collider>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.candleBlow, transform.position);
            animator.SetBool("Hovered", true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.candleBlow, transform.position);
            animator.SetBool("Hovered", false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            animator.SetBool("Hover", false);
            _candleCollider.enabled = false;

            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.candleBlow, transform.position);

            var mainModule = candlelightParticles.main;

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
                candlelight.DOIntensity(0f, tweenDuration*2)
            );

            if (addQuitDelayOnEnd)
            {
                sequence.AppendInterval(tweenDuration/2);
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
