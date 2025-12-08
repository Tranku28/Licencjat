using System;
using UnityEngine;

namespace Core
{
    public abstract class BaseSystem : MonoBehaviour
    {
        protected virtual void Awake() => InitSystem();
        
        private void InitSystem()
        {
            DependencyResolver.Instance.Register(this);
        }
    }
}