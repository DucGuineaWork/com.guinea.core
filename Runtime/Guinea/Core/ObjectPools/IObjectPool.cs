using UnityEngine;

namespace Guinea.Core.ObjectPools
{
    public interface IObjectPool<T> where T : UnityEngine.Component
    {
        T Get(Transform parent = null, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion));
        void Clear(Transform parent = null);
        void CleanUp();
    }
}