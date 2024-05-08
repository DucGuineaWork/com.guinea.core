using UnityEngine;
using DG.Tweening;

namespace Guinea.Core.UI.Effects
{
    [System.Serializable]
    public class DelayEffect :IEffect
    {
        [SerializeField] float m_delay;
        public void InsertTween(ref Sequence sequence, GameObject go=null)
        {
            sequence.AppendInterval(m_delay);
        }
    }
}