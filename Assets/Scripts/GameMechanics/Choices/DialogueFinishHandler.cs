using GameMechanics.UI;
using UnityEngine;

public class DialogueFinishHandler : MonoBehaviour
{
    void Start()
    {
        DialogueManager.OnDialogueEnded += HandleDialogueEnded;
    }

    private void OnDestroy()
    {
        DialogueManager.OnDialogueEnded -= HandleDialogueEnded;
    }

    private void HandleDialogueEnded(object sender, DialogueEndedEventArgs e)
    {
        
    }
}
