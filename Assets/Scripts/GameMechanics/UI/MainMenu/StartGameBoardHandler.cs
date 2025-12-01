using System;
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
        [SerializeField] private Button startJourneyButton, goBackButton;
        [SerializeField] private Animator cameraAnimator;
        [SerializeField] private InteractionHandler cameraAnimatorHandler;
        
        public event Action OnGameplayEntered;
        private Animator _animator;
        private Collider _collider;

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
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Board Enter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Board Exit");
        }

        public void ZoomInFinished()
        {
            startJourneyButton.interactable = true;
            goBackButton.interactable = true;
            
            _collider.enabled = false;
        }

        public void GoBackButtonClicked()
        {
            Debug.Log("GoBack");
            _animator.SetBool(ZoomIn, false);
            cameraAnimator.SetBool(ZoomIn, false);
            
            startJourneyButton.interactable = false;
            goBackButton.interactable = false;
            
            _collider.enabled = true;
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
