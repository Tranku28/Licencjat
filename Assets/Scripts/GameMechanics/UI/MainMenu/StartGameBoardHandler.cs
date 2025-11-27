using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMechanics.UI.MainMenu
{
    public class StartGameBoardHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private static readonly int ZoomIn = Animator.StringToHash("ZoomIn");
        [SerializeField] private Button startJourneyButton, goBackButton;
        [SerializeField] private Animator cameraAnimator;
        
        public event Action OnStartClicked;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            startJourneyButton.onClick.AddListener(OnStartButtonClicked);
            goBackButton.onClick.AddListener(GoBackButtonClicked);
        }
        
        private void OnDisable()
        {
            goBackButton.onClick.RemoveListener(GoBackButtonClicked);
            startJourneyButton.onClick.RemoveListener(OnStartButtonClicked);
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
        }

        public void GoBackButtonClicked()
        {
            Debug.Log("GoBack");
            _animator.SetBool(ZoomIn, false);
            cameraAnimator.SetBool(ZoomIn, false);
            
            startJourneyButton.interactable = false;
            goBackButton.interactable = false;
        }

        public void OnStartButtonClicked()
        {
            Debug.Log("Start Game");
            OnStartClicked?.Invoke();
        }
    }
}
