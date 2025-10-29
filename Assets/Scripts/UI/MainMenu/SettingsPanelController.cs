using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class SettingsPanelController : MonoBehaviour
    {
        private Button _goBackButton;

        private void Awake()
        {
            _goBackButton = GetComponentInChildren<Button>();
        }

        private void OnEnable()
        {
            _goBackButton.onClick.AddListener(BackToMainMenu);
        }

        private void OnDisable()
        {
            _goBackButton.onClick.RemoveListener(BackToMainMenu);
        }

        private void BackToMainMenu()
        {
            MainMenuManager menuManager = Registry.Instance.Get<MainMenuManager>();

            if (!menuManager) return;
            
            menuManager.ButtonVisibilitySwitch(true);
            
            gameObject.SetActive(false);
        }
    }
}
