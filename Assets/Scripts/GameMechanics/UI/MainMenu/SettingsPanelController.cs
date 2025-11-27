using Core;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace GameMechanics.UI.MainMenu
{
    public class SettingsPanelController : UIElement
    {
        [SerializeField] private Button goBackButton;

        private void OnEnable()
        {
            goBackButton.onClick.AddListener(BackToMainMenu);
        }

        private void OnDisable()
        {
            goBackButton.onClick.RemoveListener(BackToMainMenu);
        }

        private void BackToMainMenu()
        {
            gameObject.SetActive(false);
        }
    }
}
