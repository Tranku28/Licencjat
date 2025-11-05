using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [InitializeSystem("Registry")]
    public class DependencyResoler : MonoBehaviour
    {
        public static DependencyResoler Instance;
    
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
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
        }

        public void Unregister(BaseSystem item)
        {
            _registeredObjects.Remove(item);
        }
    }
}
