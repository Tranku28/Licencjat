using Core;
using Core.Save_System;

public interface ISaveElement
{
    public void SaveData(GameSaveData gameSaveData);
    public void LoadSave(GameSaveData gameSaveData);
    public void Register(ISaveElement saveElement)
    {
        DependencyResolver.Instance.GetType<SaveSystem>().RegisterToSaveSystem(saveElement);
    }
}
