using System.Collections.Generic;
using UnityEngine;

namespace Guinea.Core.UI
{
    public class WorldSpaceUI
    {
        private static Dictionary<GameObject, RectTransform> s_attachedElements = new Dictionary<GameObject, RectTransform>();
        public static RectTransform GetOrAddElement(GameObject key, RectTransform prefab, RectTransform parent)
        {
            RectTransform element;
            if (!s_attachedElements.TryGetValue(key, out element))
            {
                element = Instantiate(prefab, parent);
                s_attachedElements.Add(key,element);
            }
            return element;
        }

        public static void DisableElement(GameObject key)
        {
            if(s_attachedElements.TryGetValue(key, out RectTransform element))
            {
                if(element!=null)
                {
                    element.gameObject.SetActive(false);
                }
            }
        }

        public static void RemoveElement(GameObject key)
        {
            if(s_attachedElements.Remove(key, out RectTransform element))
            {
                if(element!=null)
                {
                    Destroy(element.gameObject);
                }
            }
        }
    }
}