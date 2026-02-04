using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Save_System;
using Player;
using TMPro;
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
        [Header("General")]
        [SerializeField] private Button startJourneyButton, goBackButton;
        [SerializeField] private TMP_Text noSaveSlotsText;
        [SerializeField] private List<SaveDisplayerUI> saveDisplayers = new();

        [Header("External Dependencies")]
        [SerializeField] private InteractionHandler cameraAnimatorHandler;
        [SerializeField] private Animator cameraAnimator;
        
        public event Action<bool> OnGameplayEntered;
        private Animator _animator;
        private Collider _collider;
        private bool _entered;
        private bool _newGame = false;

        private SaveSystem _saveSystem;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            startJourneyButton.onClick.AddListener(OnNewGame);
            goBackButton.onClick.AddListener(GoBackButtonClicked);
            
            cameraAnimatorHandler.GameStarted += OnStartGame;

            _saveSystem = DependencyResolver.Instance.GetType<SaveSystem>();
            _saveSystem.OnSaveDeleted += LoadSaves;
        }
        
        private void OnDisable()
        {
            startJourneyButton.onClick.RemoveListener(OnNewGame);
            goBackButton.onClick.RemoveListener(GoBackButtonClicked);
            
            cameraAnimatorHandler.GameStarted -= OnStartGame;

            _saveSystem.OnSaveDeleted -= LoadSaves;
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
            LoadSaves();

            startJourneyButton.interactable = true;
            goBackButton.interactable = true;
            
            _collider.enabled = false;
        }

        //TODO: move it to SaveDisplayer
        private void LoadSaves()
        {
            foreach(SaveDisplayerUI displayer in saveDisplayers)
            {
                displayer.gameObject.SetActive(false);
            }

            GameSaveData[] saves = DependencyResolver.Instance.GetType<SaveSystem>().GetSaves;
            Debug.Log(saves.Length);
            if (saves.Length == 0) return;

            for (int i=0; i < saves.Length; i++)
            {
                saveDisplayers[i].Index = saves[i].SaveIndex;
                saveDisplayers[i].saveName.text = $"Save {i+1}";
                saveDisplayers[i].dateSaved.text = $"{saves[i].DateSaved}";
                saveDisplayers[i].inGameDay.text = $"Day {saves[i].CurrentDay}";
                saveDisplayers[i].harmonyStatus.text = $"Harmony: {saves[i].HarmonyStatus}%";
                saveDisplayers[i].gameObject.SetActive(true);
            }
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
            // TODO: Fix game start is controlled inside animation invoke method in Interactionhandler
            cameraAnimator.SetBool(GameStarted, true);
            _animator.SetBool(ZoomIn, false);
            _animator.SetBool(Hover, false);
            _collider.enabled = false;
        }

        private void OnStartGame()
        {
            OnGameplayEntered?.Invoke(_newGame);
            cameraAnimator.enabled = false;
            _animator.enabled = false;
            _newGame = false;
        }

        private void OnNewGame()
        {
            _newGame = true;

            SaveSystem saveSystem = DependencyResolver.Instance.GetType<SaveSystem>();
            GameSaveData[] saves = saveSystem.GetSaves;

            if (saves.Length == 5) 
            {
                StartCoroutine(OnSaveSlotsFull());
                return;
            }

            saveSystem.SaveGame(true);
            saveSystem.ReloadSaveOnNewJourney();
            
            //TODO: remake tutorial
            OnStartButtonClicked();
        }

        private IEnumerator OnSaveSlotsFull()
        {
            noSaveSlotsText.enabled = true;
            yield return new WaitForSeconds(5f);
            noSaveSlotsText.enabled = false;
        }

        /// <summary>
        /// SaveDisplayerUI Button onclick event
        /// </summary>
        /// <param name="saveIndex"></param>
        public void OnSaveFieldClicked(int saveIndex)
        {
            SaveSystem saveSystem = DependencyResolver.Instance.GetType<SaveSystem>();
            saveSystem.LoadSave(saveIndex);

            Debug.Log("OnStartButtonClicked SaveField");
            OnStartButtonClicked();
        }
    }
}
