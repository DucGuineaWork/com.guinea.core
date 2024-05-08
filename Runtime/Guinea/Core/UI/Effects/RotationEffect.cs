using UnityEngine;
using DG.Tweening;

namespace Guinea.Core.UI.Effects
{
    [System.Serializable]
    public class RotationEffect : BaseEffect, IEffect
    {
        public Vector3 m_euler;
        public override Tween CreateTween(GameObject go)
        {
            Quaternion rotation = Quaternion.Euler(m_euler);
            Tween tween = go.transform.DOLocalRotateQuaternion(rotation * go.transform.localRotation, m_config.duration)
                .SetEase(m_config.easeType)
                .SetLoops(m_config.loop, m_config.loopType);

            return tween;
        }
    }
}