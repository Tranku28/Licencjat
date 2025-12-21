using System;
using Core;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMechanics.UI.MainMenu
{
    public class StartGameBoardHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private static readonly int ZoomIn = Animator.StringToHash("ZoomIn");
        private static readonly int GameStarted = Animator.StringToHash("GameStarted");
        private static readonly int Hover = Animator.StringToHash("Hover");
        [SerializeField] private Button startJourneyButton, goBackButton;
        [SerializeField] private Animator cameraAnimator;
        [SerializeField] private InteractionHandler cameraAnimatorHandler;
        
        public event Action OnGameplayEntered;
        private Animator _animator;
        private Collider _collider;
        private bool _entered;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            startJourneyButton.onClick.AddListener(OnStartButtonClicked);
            goBackButton.onClick.AddListener(GoBackButtonClicked);
            
            cameraAnimatorHandler.GameStarted += OnStartGame;
        }
        
        private void OnDisable()
        {
            goBackButton.onClick.RemoveListener(GoBackButtonClicked);
            startJourneyButton.onClick.RemoveListener(OnStartButtonClicked);
            
            cameraAnimatorHandler.GameStarted -= OnStartGame;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _animator.SetBool(ZoomIn, true);
            cameraAnimator.SetBool(ZoomIn, true);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.saveBoardOpen, transform.position);
            _entered = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_entered) return;
            _animator.SetBool(Hover, true);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.saveBoardHover, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_entered) return;
            _animator.SetBool(Hover, false);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.saveBoardHover, transform.position);
        }

        public void ZoomInFinished()
        {
            startJourneyButton.interactable = true;
            goBackButton.interactable = true;
            
            _collider.enabled = false;
        }

        private void GoBackButtonClicked()
        {
            _animator.SetBool(ZoomIn, false);
            cameraAnimator.SetBool(ZoomIn, false);
            
            startJourneyButton.interactable = false;
            goBackButton.interactable = false;
            
            _collider.enabled = true;
            _entered = false;
        }

        private void OnStartButtonClicked()
        {
            cameraAnimator.SetBool(GameStarted, true);
            _animator.SetBool(ZoomIn, false);
        }

        private void OnStartGame()
        {
            cameraAnimator.enabled = false;
            _animator.enabled = false;
            
            OnGameplayEntered?.Invoke();
        }
    }
}
