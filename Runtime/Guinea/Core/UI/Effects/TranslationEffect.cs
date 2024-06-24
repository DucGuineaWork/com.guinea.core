using UnityEngine;
using DG.Tweening;

namespace Guinea.Core.UI.Effects
{
    [System.Serializable]
    public class TranslationEffect : BaseEffect, IEffect
    {
        public Vector3 m_value;
        public override Tween CreateTween(GameObject go)
        {
            RectTransform rectTransform = go.transform as RectTransform;
            Tween tween;
            if(rectTransform)
            {
                if(m_config.isFrom)
                {
                    tween = rectTransform.DOAnchorPos3D(rectTransform.anchoredPosition3D + m_value, m_config.duration).From();
                }
                else
                {
                    tween = rectTransform.DOAnchorPos3D(rectTransform.anchoredPosition3D + m_value, m_config.duration);
                }
            }
            else
            {
                if(m_config.isFrom)
                {
                    tween = go.transform.DOMove(go.transform.position + m_value, m_config.duration).From();
                }
                else
                {
                    tween = go.transform.DOMove(go.transform.position + m_value, m_config.duration);
                }
            }
            tween.SetEase(m_config.easeType)
            .SetLoops(m_config.loop, m_config.loopType);
            return tween;
        } 
    }
}
