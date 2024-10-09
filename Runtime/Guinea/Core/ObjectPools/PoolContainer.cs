using System.Collections.Generic;
using UnityEngine;

namespace Guinea.Core.ObjectPools
{
    public class PoolContainer<T>: MonoBehaviour where T: UnityEngine.Component
    {
        protected Dictionary<string, IObjectPool<T>> m_container = new Dictionary<string, IObjectPool<T>>();
        public void AddPool(string key, T prefab, int count=0)
        {
            if(m_container.ContainsKey(key))
            {
                // TODO: Assert already exists
            }
            else
            {
                SimplePool<T> pool = new SimplePool<T>(prefab, count);
                m_container.Add(key, pool);
            }

        }

        public void CleanPool(string key)
        {
            if(!m_container.ContainsKey(key))
            {
                // TODO: Assert not exists
            }
            else
            {
                m_container[key].CleanUp();
                m_container.Remove(key);
            }
        }

        public void ClearPool(string key)
        {
            if(!m_container.ContainsKey(key))
            {
                // TODO: Assert not exists
            }
            else
            {
                m_container[key].Clear();
            }
        }

        public T GetPool(string key, Transform parent = null, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
        {
            if(!m_container.ContainsKey(key))
            {
                // TODO: Assert not exists
            }

            return m_container[key].Get(parent, position, rotation);
        }
    }
}
