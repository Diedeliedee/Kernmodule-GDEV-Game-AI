using System;
using System.Collections.Generic;
using UnityEngine;

namespace Joeri.Tools.Patterns
{
    public class Blackboard
    {
        private Dictionary<string, object> m_pool = new();

        public void Add(object _instance, string _key)
        {
            if (m_pool.ContainsKey(_key))
            {
                Debug.LogWarning($"Key: {_key} already present in the blackboard.");
                return;
            }
            m_pool.Add(_key, _instance);
        }

        public void Remove(string _key)
        {
            if (!m_pool.ContainsKey(_key))
            {
                Debug.LogWarning($"Key: {_key} is not present in the blackboard.");
                return;
            }
            m_pool.Remove(_key);
        }

        public T Get<T>(string _key)
        {
            if (m_pool.ContainsKey(_key))
            {
                return (T)m_pool[_key];
            }
            else
            {
                Debug.LogError($"Key: {_key} did not return a valid instance.");
                return default;
            }
        }
    }

    public class Blackboard<E> where E : Enum
    {
        private Dictionary<E, object> m_pool = new();

        public void Add(object _instance, E _key)
        {
            if (m_pool.ContainsKey(_key))
            {
                Debug.LogWarning($"Key: {_key} already present in the blackboard.");
                return;
            }
            m_pool.Add(_key, _instance);
        }

        public void Remove(E _key)
        {
            if (!m_pool.ContainsKey(_key))
            {
                Debug.LogWarning($"Key: {_key} is not present in the blackboard.");
                return;
            }
            m_pool.Remove(_key);
        }

        public T Get<T>(E _key)
        {
            if (m_pool.ContainsKey(_key))
            {
                return (T)m_pool[_key];
            }
            else
            {
                Debug.LogError($"Key: {_key} did not return a valid instance.");
                return default;
            }
        }
    }
}