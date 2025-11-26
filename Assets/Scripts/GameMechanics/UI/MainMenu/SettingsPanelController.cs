using Core;
using UI.MainMenu;
using UnityEngine.UI;

namespace GameMechanics.UI.MainMenu
{
    public class SettingsPanelController : UIElement
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
            gameObject.SetActive(false);
        }
    }
}
