using System;
using System.Collections;
using Core;
using Core.Save_System;
using GameMechanics.Interactions;
using Unity.Cinemachine;
using UnityEngine;

namespace GameMechanics.DayHandling
{
    public class DayHandler : MonoBehaviour, IInteractable
    {
        [SerializeField] private BlinkPanelUI blinkPanelUI;
        [SerializeField] private CinemachineCamera playerCamera, playerCamera2, dayEndCamera;
        
        public static Action<int> OnCabinetSpawned;
        public static Action<string> OnDiaryAddContent;
        public static Action<string> OnNewspaperLoadNews;
        private CinemachineBrain _cinemachineBrain;
        private float _cinemachineBlendDuration;

        void Awake()
        {
            _cinemachineBrain = CinemachineBrainController.Instance.GetComponent<CinemachineBrain>();
            _cinemachineBlendDuration = _cinemachineBrain.DefaultBlend.BlendTime;
        }

        private void Start()
        {
            blinkPanelUI.OnNextDayButtonClicked += OpenPlayerEyes;
        }

        private void OnDestroy()
        {
            blinkPanelUI.OnNextDayButtonClicked -= OpenPlayerEyes;
        }

        private void LoadDay()
        {
            GameSaveData saveData = DependencyResolver.Instance.
                    GetType<SaveSystem>().GetSaveData();


            foreach (int ID in saveData.CollectedSouvenirIdList)
            {
                SpawnSouvenirs(ID);
            }

            foreach (string entry in saveData.DiaryEntries)
            {
                DiaryAddContent(entry);
            }
        }
        
        private void SpawnSouvenirs(int souvenirId)
        {
            OnCabinetSpawned?.Invoke(souvenirId);
        }
        
        private void DiaryAddContent(string entry)
        {
            OnDiaryAddContent?.Invoke(entry);
        }

        public void Interact()
        {
            SaveSystem saveSystem = DependencyResolver.Instance.
                    GetType<SaveSystem>();

            if (saveSystem.ticketsAccepted == 0 && saveSystem.ticketsRejected == 0)
            {
                return;
            }

            StartCoroutine(SitDownAndProceedToNextDay());
        }

        private IEnumerator SitDownAndProceedToNextDay()
        {
            GameStateMachine stateMachine = DependencyResolver.Instance.GetType<GameStateMachine>();

            stateMachine.ChangeGameState(GameState.UIOpened);

            playerCamera2.Prioritize();
            yield return new WaitForSeconds(_cinemachineBlendDuration);

            dayEndCamera.Prioritize();
            yield return new WaitForSeconds(_cinemachineBlendDuration);

            blinkPanelUI.showNewspaper = true;
            blinkPanelUI.ClosePlayerEyes();
            LoadDay();
        }
        
        private void OpenPlayerEyes()
        {
            StartCoroutine(OpenEyes());
        }

        private IEnumerator OpenEyes()
        {
            CinemachineBrainController.Instance.PlayerCamera.Prioritize();
            
            GameStateMachine stateMachine = DependencyResolver.Instance.GetType<GameStateMachine>();
            stateMachine.ChangeGameState(GameState.Gameplay);

            yield return new WaitForSeconds(_cinemachineBlendDuration);

            blinkPanelUI.OpenPlayerEyes();
        }
    }
}
