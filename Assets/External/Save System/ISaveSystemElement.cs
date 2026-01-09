namespace External.Save_System
{
    public interface ISaveSystemElement
    {
        void LoadData(SaveData saveData);
        void SaveData(SaveData saveData);
    }
}