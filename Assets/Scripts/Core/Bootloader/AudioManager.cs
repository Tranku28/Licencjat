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
        private EventInstance musicEvent;

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
            GameStateMachine.OnGameStarted += ReplayMusic;
        }

        private void OnDestroy()
        {
            GameStateMachine.OnMenuReturned -= ReplayAmbient;
            GameStateMachine.OnGameStarted -= ReplayMusic;
        }

        private void ReplayMusic(bool obj)
        {
            musicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicEvent.release();

            musicEvent = RuntimeManager.CreateInstance(FMODEvents.Instance.music);
            musicEvent.start();
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