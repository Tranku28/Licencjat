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
        public static Action<string> OnDiaryAddContent;
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
        
        private void DiaryAddContent(string entry)
        {
            OnDiaryAddContent?.Invoke(entry);
        }

        //TODO: Prevent player from ending day without talking to passengers
        public void Interact()
        {
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

            SaveSystem saveSystem = DependencyResolver.Instance.GetType<SaveSystem>();
            saveSystem.SaveGame();
            int saveIndex = saveSystem.RuntimeSaveIndex;
            saveSystem.LoadSave(saveIndex);
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
