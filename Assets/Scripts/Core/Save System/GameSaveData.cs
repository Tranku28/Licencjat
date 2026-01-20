public class GameSaveData
    {
        public int SaveIndex;
        public string DateSaved;
        public int CurrentDay;
        public int HarmonyStatus;
        public int TicketsAccepted;
        public int TicketsRejected;
        public int[] CollectedSouvenirIdList;
        public string[] DiaryEntries;
        public string[] TutorialNotes;

        public GameSaveData(int currentDay = 1, int harmonyStatus = 100)
        {
            CurrentDay = currentDay;
            HarmonyStatus = harmonyStatus;
        }
    }