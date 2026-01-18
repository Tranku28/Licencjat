using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveDisplayerUI : MonoBehaviour
{
    [field: SerializeField] public Button saveSelectedButton {get; private set;}
    [field: SerializeField] public TMP_Text saveName {get; private set;}
    [field: SerializeField] public TMP_Text dateSaved {get; private set;}
    [field: SerializeField] public TMP_Text inGameDay {get; private set;}
    [field: SerializeField] public TMP_Text harmonyStatus {get; private set;}
    [field: SerializeField] public Button saveLoadButton {get; private set;}


    void Awake()
    {
        gameObject.SetActive(false);
    }
}
