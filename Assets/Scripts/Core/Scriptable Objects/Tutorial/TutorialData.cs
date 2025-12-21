using UnityEngine;

[CreateAssetMenu(fileName = "TutorialData", menuName = "Scriptable Objects/TutorialData")]
public class TutorialData : ScriptableObject
{
    public int ID;
    [TextArea(3, 6)]
    public string text;
}
