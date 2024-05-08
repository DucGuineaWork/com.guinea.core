using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Guinea.Core.UI.Effects
{
    [System.Serializable]
    public class CallbackEffect : IEffect
    {
        [SerializeField] UnityEvent m_callback;
        public TweenCallback Callback =>m_callback!=null ? m_callback.Invoke : EmptyCallback;

        public static void EmptyCallback(){}
        public void InsertTween(ref Sequence sequence, GameObject go=null)
        {
            TweenCallback callback= m_callback!=null? m_callback.Invoke : EmptyCallback;
            sequence.AppendCallback(callback);
        }
    }
}