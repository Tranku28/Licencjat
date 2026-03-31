using System;
using System.Collections;
using Core;
using Core.Save_System;
using GameMechanics.Interactions;
using GameMechanics.UI;
using Interactions;
using Unity.Cinemachine;
using UnityEngine;

namespace GameMechanics.DayHandling
{
    public class DayHandler : MonoBehaviour, IInteractable
    {
        [SerializeField] private BlinkPanelUI blinkPanelUI;
        [SerializeField] private CinemachineCamera playerCamera2, dayEndCamera;
        private CinemachineBrain _cinemachineBrain;
        private float _cinemachineBlendDuration;
        private bool _canNextDay;

        private void Start()
        {
            _cinemachineBrain = CinemachineBrainController.Instance.GetComponent<CinemachineBrain>();
            _cinemachineBlendDuration = _cinemachineBrain.DefaultBlend.BlendTime;
            blinkPanelUI.OnNextDayButtonClicked += OpenPlayerEyes;
            Passenger.OnPassengerInteracted += AllowNextDay;
        }

        private void AllowNextDay(object sender, PassengerInteractedEventArgs e)
        {
            _canNextDay = true;
        }

        private void OnDestroy()
        {
            blinkPanelUI.OnNextDayButtonClicked -= OpenPlayerEyes;
            Passenger.OnPassengerInteracted -= AllowNextDay;
        }

        //TODO: Prevent player from ending day without talking to passengers
        public void Interact()
        {
            if (!_canNextDay)
            {
                return;
            }

            _canNextDay = false;
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

            blinkPanelUI.showDaySummary = true;
            blinkPanelUI.ClosePlayerEyes();
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

        public string GetName()
        {
            return "Proceed to the next day";
        }
    }
}
