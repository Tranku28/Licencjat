using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        private EventInstance ambientEvent;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            ReplayAmbient();

            GameStateMachine.OnMenuReturned += ReplayAmbient;
        }

        private void OnDestroy()
        {
            GameStateMachine.OnMenuReturned -= ReplayAmbient;
        }

        private void ReplayAmbient()
        {
            ambientEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            ambientEvent.release();
            ambientEvent = RuntimeManager.CreateInstance(FMODEvents.Instance.ambient);
            ambientEvent.start();
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound, worldPos);
        }
    }
}