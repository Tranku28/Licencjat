using TMPro;
using UnityEngine;

public class TutorialInfoLoader : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private TutorialPanelAnimationController tutorialPanelAnimationController;
    public static TutorialInfoLoader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    } 

    public void LoadTutorialPanel(TutorialData tutorialData)
    {
        tutorialPanelAnimationController.enabled = false;
        tutorialText.text = tutorialData.text;
        tutorialPanelAnimationController.enabled = true;
    }
}
