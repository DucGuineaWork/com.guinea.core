using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Guinea.Core.UI.Effects
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class EffectComponent : BaseEffectComponent
    {
        [SerializeReference]
        public List<IEffect> m_effects = new List<IEffect>();

        public void AddEffect(Type type)
        {
            // TODO: Logger Assert
            IEffect instance = Activator.CreateInstance(type) as IEffect;
            m_effects.Add(instance);
        }

        protected override void AddEffects(ref Sequence sequence)
        {
            foreach(IEffect effect in m_effects)
            {
                effect.InsertTween(ref sequence, gameObject);
            }
        }
    }
}