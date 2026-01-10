using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSinglePopup : MonoBehaviour
{
    //TODO: add functionality to put those notes in interactable object in Guard's Van
    [TextArea(3,5)]
    [SerializeField] private string tutorialNote;
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private Button closeButton;

    private void OnEnable()
    {
        tmpText.text = tutorialNote;
        closeButton.onClick.AddListener(DestroyPopup);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(DestroyPopup);
    }

    private void DestroyPopup()
    {
        Destroy(gameObject);
    }
}
