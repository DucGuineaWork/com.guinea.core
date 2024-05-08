using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Guinea.Core.UI.Effects
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class EffectComponent : MonoBehaviour
    {
        [SerializeField] private bool m_activateOnEnable;
        [SerializeField] private bool m_activateOnClick;
        [SerializeField] private bool m_useCache;
        [SerializeReference]
        public List<IEffect> m_effects = new List<IEffect>();
        private Cache m_cache;

        private static WaitForEndOfFrame s_waitForEndOfFrame = new WaitForEndOfFrame();
        void OnEnable()
        {
            StopCoroutine("Init");
            StartCoroutine("Init");

            if(m_activateOnClick)
            {
                Button btn = GetComponent<Button>();
                if(btn!=null)
                {
                    btn.onClick.AddListener(Activate);
                }
            }
        }

        void OnDisable()
        {
            if(m_activateOnClick)
            {
                Button btn = GetComponent<Button>();
                if(btn!=null)
                {
                    btn.onClick.RemoveListener(Activate);
                }
            }
        }

        private IEnumerator Init()
        {
            yield return s_waitForEndOfFrame;
            CreateCache();
            if(m_activateOnEnable)
            {
                Activate();
            }


            void CreateCache()
            {
                if(!m_useCache || m_cache!=null)
                {
                    return;
                }

                Cache cache = new Cache {
                    rotation = transform.localRotation,
                    scale = transform.localScale
                };

                RectTransform rectTransform = transform as RectTransform;
                if(rectTransform!=null)
                {
                    cache.position = rectTransform.anchoredPosition3D;
                }
                else
                {
                    cache.position =  transform.position;
                }

                m_cache = cache;
            }
        }

        public void AddEffect(Type type)
        {
            // TODO: Logger Assert
            IEffect instance = Activator.CreateInstance(type) as IEffect;
            m_effects.Add(instance);
        }

        private void Activate()
        {
            DOTween.Kill(gameObject);
            
            if(m_useCache)
            {
                ApplyCache();
            }

            Sequence sequence = DOTween.Sequence();
            foreach(IEffect effect in m_effects)
            {
                effect.InsertTween(ref sequence, gameObject);
            }


            void ApplyCache()
            {
                RectTransform rectTransform = transform as RectTransform;
                if(rectTransform)
                {
                    rectTransform.anchoredPosition3D = m_cache.position;
                }
                else
                {
                    transform.position = m_cache.position;
                }

                transform.localRotation = m_cache.rotation;
                transform.localScale = m_cache.scale;
            }
        }

        [Serializable]
        public class Cache
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
        }
    }
}