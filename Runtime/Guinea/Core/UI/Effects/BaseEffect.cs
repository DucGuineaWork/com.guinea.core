using UnityEngine;
using DG.Tweening;

namespace Guinea.Core.UI.Effects
{
    [System.Serializable]
    public class BaseEffect: IEffect
    {
        public EffectConfiguration m_config;

        [System.Serializable]
        public class EffectConfiguration
        {
            public AppendType appendType;
            public float duration;
            public Ease easeType;
            public LoopType loopType;
            public int loop;
        }

        private void InsertTween_(ref Sequence sequence, Tween tween)
        {
            switch (m_config.appendType)
            {
                case AppendType.Join:
                {
                    sequence.Join(tween);
                    break;
                }
                default:
                {
                    sequence.Append(tween);
                    break;
                }
            }
        }
        
        public virtual Tween CreateTween(GameObject go)
        {
            return null;
        }

        public void InsertTween(ref Sequence sequence, GameObject go=null)
        {
            Tween tween = CreateTween(go);
            InsertTween_(ref sequence, tween);
        }
    }
}