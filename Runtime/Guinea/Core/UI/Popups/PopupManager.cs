using System;
using System.Collections.Generic;
using UnityEngine;

namespace Guinea.Core.UI.Popups
{
    public class PopupManager: MonoBehaviour
    {

        [Tooltip("Parent for all popup prefab instantiated")]
        [SerializeField] Transform m_popupContainer;
        private static Transform s_popupContainer;
        private static Dictionary<Type, IPopup> s_registeredPopups = new Dictionary<Type, IPopup>();
        private static Queue<PopupItem> s_popupQueue = new Queue<PopupItem>();
        private static IPopup s_currentOpenedPopup;
        public static bool HavePopupOpened => s_currentOpenedPopup != null;

        #region Unity Callback
        void Awake()
        {
            Logger.Assert(s_popupContainer == null, "s_popupContainer should only be set once");
            s_popupContainer = m_popupContainer;
            if (s_popupContainer != null)
            {
                IPopup[] popups = s_popupContainer.GetComponentsInChildren<IPopup>(true);
                foreach (IPopup popup in popups)
                {
                    Register(popup);
                }
            }
            s_popupContainer.gameObject.SetActive(false);
        }
        #endregion

        public static void Register(IPopup popup)
        {
            Logger.Assert(popup != null, "You are registering a null instance of IPopup");
            // TODO: Handle popup already registered
            if(s_registeredPopups.ContainsKey(popup.GetType()))
            {
                Logger.LogWarning($"Popup {popup.GetType()} already been registered");
                return;
            }

            IPopup popupInContainer = s_popupContainer.GetComponentInChildren(popup.GetType()) as IPopup;

            if(popupInContainer == null)
            {
                s_registeredPopups.Add(popup.GetType(), popup);
                Logger.Log($"Register {popup.GetType()}----{popup} SUCCESS");
            }
            else
            {
                popupInContainer.gameObject.SetActive(false);
                s_registeredPopups.Add(popup.GetType(), popupInContainer);  
                Logger.LogWarning($"Register {popup.GetType()}----{popup}, using already exists popup");
            }

        }

        public static void Unregister(IPopup popup)
        {
            bool success = s_registeredPopups.Remove(popup.GetType());
            Logger.Assert(success, $"You are unregistering a popup that is not registered to {popup.GetType()}");
            Logger.LogIf(success, $"Unregister success {popup.GetType()}----{popup}");
        }

        public static void Open<T, U>(U args) where T : IPopup
        {
            if (HavePopupOpened)
            {
                s_popupQueue.Enqueue(new PopupItem(typeof(T), args));
            }
            else
            {
                Open(typeof(T), args);
            }
        }

        public static void Close(IPopup popup)
        {
            Logger.Assert(s_currentOpenedPopup != null && s_currentOpenedPopup.GetType() == popup.GetType(), $"Popup \"{popup}\" not opened yet");
            Logger.Log($"PopupManager::Close(): Close \"{popup}\"");
            if (s_currentOpenedPopup.DestroyWhenClosed)
            {
                Destroy(s_currentOpenedPopup.gameObject);
            }
            else
            {
                s_currentOpenedPopup.gameObject.SetActive(false);
            }
            s_popupContainer.gameObject.SetActive(false);

            s_currentOpenedPopup = null;

            if (s_popupQueue.TryDequeue(out PopupItem item))
            {
                Open(item.popupType, item.args);
            }
        }

        public static void OnBackKeyEvent()
        {
            Logger.Log("OnBackKeyEvent()");
            s_currentOpenedPopup?.OnBackKeyEvent();
        }

        private static void Open<U>(Type popupType, U args)
        {
            Logger.Assert(s_registeredPopups.ContainsKey(popupType), $"You are trying to show a popup that is not registered {popupType}");
            if (s_registeredPopups.TryGetValue(popupType, out IPopup popup))
            {
                IPopup instance = (IPopup)s_popupContainer.GetComponentInChildren(popupType, true);
                if (instance == null)
                {
                    instance = Instantiate(popup.gameObject, s_popupContainer).GetComponent<IPopup>();
                    RectTransform rectTrans = instance.transform as RectTransform;
                    rectTrans.anchoredPosition = Vector2.zero;
                    rectTrans.localScale = Vector3.one;
                }

                s_popupContainer.gameObject.SetActive(true);
                instance.transform.SetAsLastSibling();
                instance.OnOpen(args);
                instance.gameObject.SetActive(true);
                s_currentOpenedPopup = instance;
            }
        }

        private class PopupItem
        {
            public readonly Type popupType;
            public readonly object args;

            public PopupItem(Type popupType, object args)
            {
                this.popupType = popupType;
                this.args = args;
            }
        }
    }
}