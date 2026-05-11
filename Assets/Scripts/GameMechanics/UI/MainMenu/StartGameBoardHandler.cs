using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Save_System;
using Player;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMechanics.UI.MainMenu
{
    [RequireComponent(typeof(Collider))]
    public class StartGameBoardHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private static readonly int ZoomIn = Animator.StringToHash("ZoomIn");
        private static readonly int GameStarted = Animator.StringToHash("GameStarted");
        private static readonly int Hover = Animator.StringToHash("Hover");
        [Header("General")]
        [SerializeField] private Button startJourneyButton;
        [SerializeField] private TMP_Text noSaveSlotsText;
        [SerializeField] private List<SaveDisplayerUI> saveDisplayers = new();
        [SerializeField] private CinemachineBlendDefinition zoomInBlendSettings;
        [SerializeField] private Animator boardAnimator;

        [Header("External Dependencies")]
        [SerializeField] private CinemachineCamera saveSelectorCamera;
        [SerializeField] private CinemachineCamera playerHeadCamera;
        [SerializeField] private CinemachineCamera mainMenuZoomOutCamera;

        private CinemachineBrainController _cinemachineBrainController;
        
        public event Action<bool> OnGameplayEntered;
        public event Action InvokeCloseButtonDissapear;
        public event Action<bool> OnStartBoardZoomed;
        private Collider _collider;
        private bool _entered;
        private bool _newGame = false;

        private SaveSystem _saveSystem;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            _cinemachineBrainController = CinemachineBrainController.Instance;
            SetCinemachineBrainCutBlend();
        }

        private void OnEnable()
        {
            startJourneyButton.onClick.AddListener(OnNewGame);

            _saveSystem = DependencyResolver.Instance.GetType<SaveSystem>();
            _saveSystem.OnSaveDeleted += LoadSaves;

            GameStateMachine.OnMenuReturned += HideSaves;
        }

        private void OnDisable()
        {
            startJourneyButton.onClick.RemoveListener(OnNewGame);

            _saveSystem.OnSaveDeleted -= LoadSaves;

            GameStateMachine.OnMenuReturned -= HideSaves;
        }

        private void HideSaves()
        {
            foreach (SaveDisplayerUI saveDisplayer in saveDisplayers)
            {
                saveDisplayer.gameObject.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            boardAnimator.SetBool(ZoomIn, true);
            _cinemachineBrainController.BlendCustom(zoomInBlendSettings);
            saveSelectorCamera.Prioritize();
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.saveBoardOpen, transform.position);
            _entered = true;

            StartCoroutine(ZoomInFinished(_cinemachineBrainController.Brain.DefaultBlend.BlendTime));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_entered) return;
            boardAnimator.SetBool(Hover, true);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.saveBoardHover, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_entered) return;
            boardAnimator.SetBool(Hover, false);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.saveBoardHover, transform.position);
        }

        public IEnumerator ZoomInFinished(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            LoadSaves();

            startJourneyButton.interactable = true;
            OnStartBoardZoomed?.Invoke(true);
            
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
                saveDisplayers[i].Index = i;
                saveDisplayers[i].saveName.text = $"Save {i+1}";
                saveDisplayers[i].dateSaved.text = $"{saves[i].DateSaved}";
                saveDisplayers[i].inGameDay.text = $"Day {saves[i].CurrentDay}";
                saveDisplayers[i].harmonyStatus.text = $"Harmony: {saves[i].HarmonyStatus}%";
                saveDisplayers[i].gameObject.SetActive(true);
            }
        }

        public void GoBackButtonClicked()
        {
            boardAnimator.SetBool(ZoomIn, false);
            
            startJourneyButton.interactable = false;
            OnStartBoardZoomed?.Invoke(false);
            
            _collider.enabled = true;
            _entered = false;

            mainMenuZoomOutCamera.Prioritize();
        }

        private void OnStartButtonClicked()
        {
            boardAnimator.SetBool(ZoomIn, false);
            boardAnimator.SetBool(Hover, false);
            _collider.enabled = false;
            playerHeadCamera.Prioritize();

            InvokeCloseButtonDissapear?.Invoke();

            StartCoroutine(OnStartGame(_cinemachineBrainController.Brain.DefaultBlend.Time));
        }

        private IEnumerator OnStartGame(float time)
        {
            yield return new WaitForSeconds(time);     
            OnGameplayEntered?.Invoke(_newGame);
            _newGame = false;
        }

        public void MenuReturned()
        {
            SetCinemachineBrainCutBlend();
            boardAnimator.SetBool(ZoomIn, false);
            
            startJourneyButton.interactable = false;
            
            _collider.enabled = true;
            _entered = false;
        }

        private void SetCinemachineBrainCutBlend()
        {
            _cinemachineBrainController.BlendSetCut();
            mainMenuZoomOutCamera.Prioritize();
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
            saveSystem.LoadSave(saveSystem.GetSaves[saveIndex]);

            OnStartButtonClicked();
        }
    }
}
