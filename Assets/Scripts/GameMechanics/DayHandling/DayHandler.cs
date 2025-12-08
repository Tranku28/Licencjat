using System;
using Core;
using UnityEngine;

namespace GameMechanics.DayHandling
{
    public class DayHandler : MonoBehaviour
    {
        [SerializeField] private BlinkPanelUI blinkPanelUI;
        
        public static Action<GameObject> OnCabinetSpawned;
        public static Action<string> OnDiaryAddContent;
        public static Action<string> OnNewspaperLoadNews;
        
        private void LoadDay()
        {
            SaveData[] saveDataArray = DependencyResolver.Instance.
                    GetType<SaveSystem>().
                    GetCachedSaveDataList()
                ;

            foreach (SaveData saveData in saveDataArray)
            {
                if (saveData.TicketScanned) 
                    SpawnSouvenirs(saveData);
                
                DiaryAddContent(saveData);
                NewspaperLoadNews(saveData);
            }
        }
        
        private void SpawnSouvenirs(SaveData saveData)
        {
            OnCabinetSpawned?.Invoke(saveData.PassengerData.souvenirPrefab);
        }
        
        private void DiaryAddContent(SaveData saveData)
        {
            OnDiaryAddContent?.Invoke(saveData.PassengerData.diaryContent);
        }
        
        private void NewspaperLoadNews(SaveData saveData)
        {
            OnNewspaperLoadNews?.Invoke(saveData.PassengerData.newspaperText);
        }
    }
}
