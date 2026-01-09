using System;

namespace External.Save_System
{
    [Serializable]
    public class SaveData
    {
        public int day;
        public int harmony;

        public SaveData(int day, int harmony)
        {
            this.day = day;
            this.harmony = harmony;
        }
    }
}