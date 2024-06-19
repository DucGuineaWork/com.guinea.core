using System;
using System.Collections.Generic;
using UnityEngine;

namespace Guinea.Core.UI.Effects
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [CreateAssetMenu(fileName ="EffectSO",menuName ="SO/EffectSO")]
    public class EffectSO: ScriptableObject
    {
        [SerializeReference]
        private List<IEffect> m_effects = new List<IEffect>();
        public List<IEffect> Effects =>m_effects;

        public void AddEffect(Type type)
        {
            // TODO: Logger Assert
            IEffect instance = Activator.CreateInstance(type) as IEffect;
            m_effects.Add(instance);
        }
    }

    
}