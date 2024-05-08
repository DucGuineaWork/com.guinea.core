using DG.Tweening;
using UnityEngine;

namespace Guinea.Core.UI.Effects
{
    [System.Serializable]
    public class ScaleEffect : BaseEffect, IEffect
    {
        public Vector3 value;
        public override Tween CreateTween(GameObject go)
        {
            Tween tween = go.transform.DOScale(value, m_config.duration)
                .SetEase(m_config.easeType)
                .SetLoops(m_config.loop, m_config.loopType);
            return tween;
        }
    }
}