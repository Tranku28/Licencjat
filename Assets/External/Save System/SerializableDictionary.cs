using System;
using System.Collections.Generic;
using UnityEngine;

namespace External.Save_System
{
    [Serializable]
    public class SerializableDictionary <TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> _keys = new List<TKey>();
        [SerializeField] private List<TValue> _values = new List<TValue>();
        
        public void OnBeforeSerialize()
        {
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                _keys.Add(pair.Key);
                _values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();
            
            if (_keys.Count != _values.Count)
            {
                Debug.LogError("Serialized Dictionary keys and values do not match.");
                return;
            }

            for (int i = 0; i < _keys.Count; i++)
            {
                this.Add(_keys[i], _values[i]);
            }
        }
    }
}