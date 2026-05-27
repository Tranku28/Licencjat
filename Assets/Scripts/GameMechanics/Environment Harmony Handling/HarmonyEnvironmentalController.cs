using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;

namespace HarmonyHandling
{    
    public class HarmonyEnvironmentalController : MonoBehaviour
    {
        public enum DisorderStatus
        {
            Order,
            Low,
            Medium,
            Chaos,
            End
        }

        [SerializeField] private float generalWaitTime;
        [SerializeField] private List<HarmonyLightController> harmonyLights = new();
        private HarmonySoundController _harmonySoundController;
        private HashSet<int> _flickeringSelected = new();
        private int _levelOneFlickersCount = 2;
        private Coroutine _mainCoroutine;
        private Coroutine _flickerCoroutine;
        private Coroutine _soundCoroutine;
        private DisorderStatus _disorderStatus;

        private void Start()
        {
            _harmonySoundController = new(this);

            HarmonyIndicator.OnHarmonyValueSet += UpdateDisorder;
            GameStateMachine.OnMenuReturned += StopAllCoroutines;
            GameStateMachine.OnGameStarted += SetupMainCoroutine;
        }

        private void OnDestroy()
        {
            HarmonyIndicator.OnHarmonyValueSet -= UpdateDisorder;
            GameStateMachine.OnMenuReturned -= StopAllCoroutines;
            GameStateMachine.OnGameStarted -= SetupMainCoroutine;
        }

        private void SetupMainCoroutine(bool obj)
        {
            _mainCoroutine = StartCoroutine(MainRoutine());
        }

        private IEnumerator MainRoutine()
        {
            while (true)
            {
                _flickerCoroutine ??= StartCoroutine(HandleFlickering());
                HandleSounds();
                
                yield return new WaitForSeconds(generalWaitTime);
            }
        }

        private IEnumerator HandleFlickering()
        {
            if (_disorderStatus == DisorderStatus.Order)
                goto End;

            if (_disorderStatus == DisorderStatus.Low)
            {
                _flickeringSelected.Clear();

                for (int i=0; i < _levelOneFlickersCount; i++)
                {
                    int randomIndex = Random.Range(0, harmonyLights.Count-1);

                    if (_flickeringSelected.Contains(randomIndex)) continue;

                    harmonyLights[randomIndex].PlaySingleFlicker();
                    _flickeringSelected.Add(randomIndex);
                }
                goto End;
            }

            if (_disorderStatus == DisorderStatus.Medium)
            {
                foreach (var lightController in harmonyLights)
                {
                    Sequence sequence = lightController.PlaySingleFlicker();
                    yield return sequence.WaitForPosition(sequence.Duration(false) * 0.5f);
                }

                goto End;
            }

            if (_disorderStatus == DisorderStatus.Chaos)
            {
                foreach (var lightController in harmonyLights)
                {
                    lightController.PlaySingleFlicker();
                }
            }

            End:
            _flickerCoroutine = null;
        }

        private void HandleSounds()
        {
            switch (_disorderStatus)
            {
                case DisorderStatus.Low:
                    _harmonySoundController.TryPlaySound(1);
                    break;
                case DisorderStatus.Medium:
                    _harmonySoundController.TryPlaySound(3);
                    break;
                case DisorderStatus.Chaos:
                    _harmonySoundController.TryPlaySound(5);
                    break;
                default:
                    _harmonySoundController.TryPlaySound(0);
                    break;
            }
        }

        private void UpdateDisorder(int value)
        {
            _disorderStatus = SetDisorderStatus(value);  
        }

        private DisorderStatus SetDisorderStatus(int value)
        {
            return value switch
            {
                100 => DisorderStatus.Order,
                >= 75 => DisorderStatus.Low,
                >= 50 => DisorderStatus.Medium,
                >= 25 => DisorderStatus.Chaos,
                _ => DisorderStatus.End
            };
        }
    }
}
