using System.Collections.Generic;
using UnityEngine;

public class Registry : MonoBehaviour
{
    public static Registry Instance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private List<IRegister> _registeredObjects = new();

    public T Get<T>() where T : class
    {
        foreach (var item in _registeredObjects)
        {
            if (item is not T item1) continue;
            return item1;
        }
        
        return null;
    }
    
    public void Register(IRegister item)
    {
        _registeredObjects.Add(item);
    }

    public void Unregister(IRegister item)
    {
        _registeredObjects.Remove(item);
    }
}
