using Core;
using DG.Tweening;
using GameMechanics.UI;
using TMPro;
using UnityEngine;

namespace GameMechanics.Interactions
{
    public class PlayerMessenger : MonoBehaviour
    {
        [SerializeField] private RectTransform _messageBoxTransform;
        [SerializeField] private float _tweenTime;
        [SerializeField] private float _waitTillHideTime;
        private float _messageBoxOffset;
        private TMP_Text _message;
        private Vector2 _messageBoxInitialPosition;
        private bool _isTweening;

        public static PlayerMessenger instance;
        

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            _messageBoxInitialPosition = new(_messageBoxTransform.position.x, _messageBoxTransform.position.y);
            
            _messageBoxOffset = _messageBoxTransform.rect.width + _messageBoxTransform.rect.position.x;
            _messageBoxTransform.position = new Vector2(-_messageBoxOffset, _messageBoxTransform.position.y);

            _message = _messageBoxTransform.GetComponentInChildren<TMP_Text>();
        }

        public void MessagePlayer(string message, object sender)
        {
            _message.text = message;

            if (_isTweening) return;

            _isTweening = true;

            Sequence sequence = DOTween.Sequence();

            sequence
                .Append(_messageBoxTransform.DOMoveX(_messageBoxInitialPosition.x, _tweenTime, true))
                .AppendInterval(_waitTillHideTime)
                .Append(_messageBoxTransform.DOMoveX(-_messageBoxOffset, _tweenTime, true))
                .OnComplete(() => _isTweening = false);

            if (sender is DialogueManager)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.diaryEntry, transform.position);
            }

            if (sender is IInteractable)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.notification, transform.position);
            }
        }
    }    
}

