using System.Collections.Generic;
using UnityEngine;

namespace Joeri.Tools.Patterns
{
    /// <summary>
    /// A blackboard in which to store custom data containers, using their class type as a key.
    /// </summary>
    public class ContainerBlackboard
    {
        private Dictionary<System.Type, IBlackboardContainer> m_pool = new();

        public void Add(IBlackboardContainer _instance, System.Type _key = null)
        {
            if (_key == null)
            {
                _key = _instance.GetType();
            }

            if (m_pool.ContainsKey(_key))
            {
                Debug.LogWarning($"Key: {_key} already present in the blackboard.");
                return;
            }
            m_pool.Add(_key, _instance);
        }

        public void Remove(System.Type _key)
        {
            if (!m_pool.ContainsKey(_key))
            {
                Debug.LogWarning($"Key: {_key} is not present in the blackboard.");
                return;
            }
            m_pool.Remove(_key);
        }

        public T Get<T>()
        {
            var key = typeof(T);

            if (m_pool.ContainsKey(key))
            {
                return (T)m_pool[key];
            }
            else
            {
                Debug.LogError($"Key: {key} did not return a valid instance.");
                return default;
            }
        }
    }
}
