using System;
using Core;
using Core.Save_System;
using GameMechanics.Interactions;
using UnityEngine;

namespace GameMechanics.DayHandling
{
    public class DayHandler : MonoBehaviour, IInteractable
    {
        [SerializeField] private BlinkPanelUI blinkPanelUI;
        [SerializeField] private TutorialData cannotProceedData;
        
        public static Action<int> OnCabinetSpawned;
        public static Action<string> OnDiaryAddContent;
        public static Action<string> OnNewspaperLoadNews;
        private bool _tutorialShown;

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
                // TODO: Tutorial single popup handling
                if (!_tutorialShown)
                {
                    TutorialInfoLoader.Instance.LoadTutorialPanel(cannotProceedData);
                    _tutorialShown = true;
                }
                return;
            }

            blinkPanelUI.showNewspaper = true;
            blinkPanelUI.ClosePlayerEyes();
            LoadDay();
        }
        
        private void OpenPlayerEyes()
        {
            blinkPanelUI.OpenPlayerEyes();
        }
    }
}
