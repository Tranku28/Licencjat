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
        
        public static Action<GameObject> OnCabinetSpawned;
        public static Action<string> OnDiaryAddContent;
        public static Action<string> OnNewspaperLoadNews;

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
            SaveData[] saveDataArray = DependencyResolver.Instance.
                    GetType<SaveSystem>().
                    GetCachedSaveDataList()
                ;

            Debug.Log(saveDataArray.Length);
            
            foreach (SaveData saveData in saveDataArray)
            {
                Debug.Log(saveData.ticketScanned);
                if (saveData.ticketScanned) 
                    SpawnSouvenirs(saveData);
                
                DiaryAddContent(saveData);
                NewspaperLoadNews(saveData);
            }
        }
        
        private void SpawnSouvenirs(SaveData saveData)
        {
            OnCabinetSpawned?.Invoke(saveData.passengerData.souvenirPrefab);
        }
        
        private void DiaryAddContent(SaveData saveData)
        {
            OnDiaryAddContent?.Invoke(saveData.passengerData.diaryContent);
        }
        
        private void NewspaperLoadNews(SaveData saveData)
        {
            OnNewspaperLoadNews?.Invoke(saveData.passengerData.newspaperText);
        }

        public void Interact()
        {
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
