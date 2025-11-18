using System;
using UnityEngine;

namespace Core
{
    public abstract class BaseSystem : MonoBehaviour
    {
        protected virtual void Awake() => InitSystem();

        protected virtual void OnDestroy() => UnregisterSystem();
        
        private void InitSystem()
        {
            DependencyResoler.Instance.Register(this);
        }

        private void UnregisterSystem()
        {
            DependencyResoler.Instance.Unregister(this);
        }
    }
}