using Core;
using GameMechanics.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class CreditsPanelController : UIElement
    {
        [SerializeField] private Button _goBackButton;

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
