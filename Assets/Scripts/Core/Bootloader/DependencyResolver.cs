using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class DependencyResolver : MonoBehaviour
    {
        private static DependencyResolver instance;
        public static DependencyResolver Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("DependencyResolver");
                    instance = go.AddComponent<DependencyResolver>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private static List<BaseSystem> _registeredObjects = new();

        public T GetType<T>() where T : class
        {
            foreach (var item in _registeredObjects)
            {
                if (item is not T item1) continue;
                return item1;
            }
        
            return null;
        }
    
        public void Register(BaseSystem item)
        {
            _registeredObjects.Add(item);
            Debug.Log($"Registered {item.GetType().Name} to Dependency Resolver");
        }

        private void OnDestroy()
        {
            _registeredObjects.Clear();
        }
    }
}
