using System;
using Core;
using UnityEngine;

namespace HarmonyHandling
{

    public class HarmonySoundController
    {
        private HarmonyEnvironmentalController _controller;
        private int _pointsRemainingToPlayAgain = 20;
        private int _collectedPoints;

        public HarmonySoundController(HarmonyEnvironmentalController controller)
        {
            _controller = controller;
        }

        internal void TryPlaySound(int value)
        {
            _collectedPoints += value;
            if (_collectedPoints >= _pointsRemainingToPlayAgain)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.harmonyDisorder, _controller.transform.position);
                _collectedPoints = 0;
            }
        }
    }
}
