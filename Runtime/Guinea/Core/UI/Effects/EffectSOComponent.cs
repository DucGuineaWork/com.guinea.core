using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Guinea.Core.UI.Effects
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class EffectSOComponent : BaseEffectComponent
    {
        [SerializeField]EffectSO m_effectSO;

        protected override void AddEffects(ref Sequence sequence)
        {
            foreach(IEffect effect in m_effectSO.Effects)
            {
                effect.InsertTween(ref sequence, gameObject);
            }
        }
    }
}