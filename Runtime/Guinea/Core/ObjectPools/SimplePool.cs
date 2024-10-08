using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Guinea.Core.ObjectPools
{
    public class SimplePool<T> : IObjectPool<T> where T : UnityEngine.Component
    {
        private Action<GameObject> OnCreated;
        private List<T> m_pool; // TODO: Using IObjectPool<T> instead of List<T> 
        private T prefab;

        public SimplePool(T prefab, int count = 0, Transform parent = null, Action<GameObject> OnCreated = null)
        {
            this.prefab = prefab;
            this.m_pool = new List<T>();
            this.OnCreated = OnCreated;
            if (count > 0)
            {
                T instance;
                for (int i = 0; i < count; i++)
                {
                    instance = GameObject.Instantiate(prefab, parent);
                    OnCreated?.Invoke(instance.gameObject);
                    m_pool.Add(instance);
                }
            }
            Clear();
        }

        public void Clear(Transform parent = null)
        {
            this.prefab.gameObject.SetActive(false);
            foreach (T t in m_pool)
            {
                if (parent != null)
                {
                    t.transform.SetParent(parent);
                }
                t.gameObject.SetActive(false);
            }
        }

        public T Get(Transform parent = null, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
        {
            T instance = m_pool.FirstOrDefault(t => !t.gameObject.activeSelf);

            if (instance == null)
            {
                instance = GameObject.Instantiate(prefab, parent);
                OnCreated?.Invoke(instance.gameObject);
                m_pool.Add(instance);
            }
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void CleanUp()
        {
            foreach (T t in m_pool)
            {
                if(t!=null)
                {
                    GameObject.Destroy(t.gameObject);
                }
            }
            m_pool.Clear();
        }
    }
}
