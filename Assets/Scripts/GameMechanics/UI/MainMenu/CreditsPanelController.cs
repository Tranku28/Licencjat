using Core;
using GameMechanics.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class CreditsPanelController : UIElement
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
            MainMenuController menuController = GetComponentInParent<MainMenuController>();

            if (!menuController) return;
            
            menuController.ButtonVisibilitySwitch(true);
            
            gameObject.SetActive(false);
        }
    }
}
