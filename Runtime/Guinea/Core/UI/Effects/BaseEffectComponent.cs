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
    public class BaseEffectComponent : MonoBehaviour
    {
        [SerializeField] protected bool m_activateOnEnable;
        [SerializeField] protected bool m_activateOnClick;
        [SerializeField] protected bool m_useCache;
        protected Cache m_cache;

        protected static WaitForEndOfFrame s_waitForEndOfFrame = new WaitForEndOfFrame();
        protected void OnEnable()
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

        protected void OnDisable()
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

        protected IEnumerator Init()
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

        protected virtual void AddEffects(ref Sequence sequence)
        {
        }

        protected void Activate()
        {
            DOTween.Kill(gameObject);
            
            if(m_useCache)
            {
                ApplyCache();
            }

            Sequence sequence = DOTween.Sequence();
            AddEffects(ref sequence);


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