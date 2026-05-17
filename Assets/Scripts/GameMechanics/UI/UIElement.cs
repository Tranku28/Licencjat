using UnityEngine;

namespace GameMechanics.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        [SerializeField] private GameObject UIVisual;

        public virtual void SetVisualVisibility(bool show)
        {
            UIVisual.SetActive(show);
        }
    }
}