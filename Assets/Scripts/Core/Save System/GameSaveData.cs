using System.Collections.Generic;

public class GameSaveData
    {
        public int SaveIndex;
        public string DateSaved;
        public int CurrentDay = 1;
        public int HarmonyStatus = 100;
        public int TicketsAccepted;
        public int TicketsRejected;
        public List<int> CollectedSouvenirIdList = new();
        public List<string> DiaryEntries = new();
        public List<string> TutorialNotes = new();
    }