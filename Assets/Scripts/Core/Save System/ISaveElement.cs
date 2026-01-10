using Core.Save_System;
using UnityEditor.Overlays;

public interface ISaveElement
{
    public void SaveData(GameSaveData gameSaveData);
    public void LoadSave(GameSaveData gameSaveData);
    public void Register();
}
