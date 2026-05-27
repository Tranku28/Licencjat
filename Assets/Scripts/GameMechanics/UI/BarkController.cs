using UnityEngine;
using GameMechanics.UI;
using Interactions;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace GameMechanics
{
    public class BarkController : UIElement
    {
        [SerializeField] private TMP_Text textField;
        [SerializeField] private Image portraitImage;
        private Queue<string> _barksQueue;
        private TextPrinter _textPrinter;
        private void Awake()
        {
            _textPrinter = new();
            _barksQueue = new();
            Passenger.OnBark += OnBarkEvent;
        }

        private void OnDestroy()
        {
            Passenger.OnBark -= OnBarkEvent;
        }

        private void OnBarkEvent(string[] barks, Sprite portrait)
        {
            portraitImage.sprite = portrait;
            if (barks.Length == 0) return;

            if (_barksQueue.Count == 0)
            {
                QueueBarks(barks);

                if (_barksQueue.Count == 0) return;
            }

            _textPrinter.Print(textField, _barksQueue.Dequeue());
        }

        private void QueueBarks(string[] barks)
        {
            System.Random rng = new();

            var shuffled = barks.ToArray();

            for (int i = shuffled.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
            }

            foreach (var bark in shuffled)
            {
                _barksQueue.Enqueue(bark);
            }
        }
    }        
}
