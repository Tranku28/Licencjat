using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;

namespace HarmonyHandling
{    
    public class HarmonyEnvironmentalController : MonoBehaviour
    {
        private enum DisorderStatus
        {
            Order,
            Low,
            Medium,
            Chaos,
            End
        }

        [SerializeField] private float generalWaitTime;
        [SerializeField] private List<HarmonyLightController> harmonyLights = new();
        private Coroutine _mainCoroutine;
        private Coroutine _flickerCoroutine;
        private DisorderStatus _disorderStatus;

        private void Start()
        {
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
                Debug.Log("Main Coroutine Invoked");
                _flickerCoroutine ??= StartCoroutine(HandleFlickering());
                
                yield return new WaitForSeconds(generalWaitTime);
            }
        }

        private IEnumerator HandleFlickering()
        {
            Debug.Log("Flickering");

            if (_disorderStatus == DisorderStatus.Order)
                goto End;

            if (_disorderStatus == DisorderStatus.Low)
            {
                Debug.Log("Random....");
                int flickeringIndex = Random.Range(0, harmonyLights.Count-1);
                harmonyLights[flickeringIndex].PlaySingleFlicker();
                goto End;
            }

            if (_disorderStatus == DisorderStatus.Medium)
            {
                Debug.Log("All sequentially");
                foreach (var lightController in harmonyLights)
                {
                    Sequence sequence = lightController.PlaySingleFlicker();
                    yield return sequence.WaitForPosition(sequence.Duration(false) * 0.5f);
                }

                goto End;
            }

            if (_disorderStatus == DisorderStatus.Chaos)
            {
                Debug.Log("All at once");
                foreach (var lightController in harmonyLights)
                {
                    lightController.PlaySingleFlicker();
                }
            }

            End:
            _flickerCoroutine = null;
        }

        private void UpdateDisorder(int value)
        {
            _disorderStatus = SetDisorderStatus(value);
            Debug.Log(_disorderStatus);   
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
