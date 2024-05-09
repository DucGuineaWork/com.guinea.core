using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Guinea.Core.UI.Menus
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]Transform m_menuContainer;
        [SerializeField]BaseMenu m_mainMenu;
        // [SerializeField]DropdownManager m_dropdownManager;
        private Dictionary<Type, IMenu> m_registeredMenus = new Dictionary<Type, IMenu>();
        private Stack<IMenu> m_menuStack = new Stack<IMenu>();
        public event Action<IMenu> OnMenuStackChanged = delegate { };

        #region Unity Callbacks
        void OnEnable()
        {
            InputUIDispatcher.OnMenuBackKeyEvent += OnBackKeyEvent;
        }

        void OnDisable()
        {
            InputUIDispatcher.OnMenuBackKeyEvent -= OnBackKeyEvent;
        }

        void Awake()
        {
            if(m_mainMenu!=null)
            {
                Register(m_mainMenu);
            }
        }

        void Start()
        {
            if(m_mainMenu != null)
            {
                Open<int>(m_mainMenu.GetType(), 0);
            }
        }
        #endregion

        public void Register(IMenu menu)
        {
            Logger.Assert(menu != null, "You are registering a null instance of IMenu");
            bool success = m_registeredMenus.TryAdd(menu.GetType(), menu);
            Logger.LogIf(success, $"Register {menu.GetType()}----{menu} success");

            PropertyInfo propertyInfo = menu.GetType().GetProperty("MenuManager");
            Logger.Assert(propertyInfo != null, "No propertyInfo found");
            propertyInfo.SetValue(menu, this);
        }

        public void Unregister(IMenu menu)
        {
            bool success = m_registeredMenus.Remove(menu.GetType());
            Logger.Assert(success, $"You are unregistering a menu that is not registered to {menu.GetType()}");
            Logger.LogIf(success, $"Unregister {menu.GetType()}----{menu} success");
        }

        public void Open<T, U>(U args) where T : IMenu
        {
            Open<U>(typeof(T), args);
        }

        public void Close<T>() where T : IMenu
        {
            Close(typeof(T));
        }

        private void Open<U>(Type menuType, U args)
        {
            if (m_menuStack.Any(menu => menu.GetType() == menuType))
            {
                Logger.LogWarning($"Menu {menuType} already opened");
                if (m_menuStack.Count > 0 && m_menuStack.Peek().GetType() == menuType)
                {
                    m_menuStack.Peek().OnOpenMenu<U>(args);
                    Logger.LogWarning($"Recall IMenu.OnOpenMenu() of current top menu {menuType}");
                }
                return;
            }

            Logger.Assert(m_registeredMenus.ContainsKey(menuType), $"You are opening a menu that is not registered {menuType}");
            if (m_registeredMenus.TryGetValue(menuType, out IMenu menu))
            {
                IMenu instance = (IMenu)m_menuContainer.GetComponentInChildren(menuType, true);
                if (instance == null)
                {
                    GameObject o = Instantiate(menu.gameObject, m_menuContainer);
                    instance = o.GetComponent<IMenu>();
                    PropertyInfo propertyInfo = instance.GetType().GetProperty("MenuManager");
                    Logger.Assert(propertyInfo != null, "No propertyInfo found");
                    propertyInfo.SetValue(instance, this);
                    Logger.Assert(instance != null, $"Could not get IMenu from {instance.GetType()}");
                }
                instance.transform.SetAsLastSibling();
                if (instance.ChildrenMenu != null)
                {
                    foreach (IMenu childMenu in menu.ChildrenMenu)
                    {
                        Register(childMenu);
                    }
                }

                instance.OnOpenMenu(args);
                instance.gameObject.SetActive(true);
                m_menuStack.Push(instance);
                OnMenuStackChanged(instance);
                Logger.Log($"Open \"{menuType}\" \"{instance}\" success");
            }
        }

        public void Close(Type menuType)
        {
            Logger.Assert(m_menuStack.Count > 0 && m_menuStack.Peek().GetType() == menuType, $"IMenu {menuType} not on the top of the stack");
            CloseTopMenu();
        }

        private void CloseTopMenu()
        {
            Logger.Assert(m_menuStack.Count > 0, $"No menu opened in stack to close");
            IMenu menu = m_menuStack.Pop();
            menu.OnCloseMenu();

            if (menu.ChildrenMenu != null)
            {
                foreach (IMenu childMenu in menu.ChildrenMenu)
                {
                    Unregister(childMenu);
                }
            }
            if (menu.DestroyWhenClosed)
            {
                Destroy(menu.gameObject);
            }
            else
            {
                menu.gameObject.SetActive(false);
            }

            if (m_menuStack.Count > 0)
            {
                OnMenuStackChanged(m_menuStack.Peek());
            }
        }
        private void OnBackKeyEvent()
        {
            // if(m_dropdownManager!=null && m_dropdownManager.HasDropdownOpened)
            // {
            //     m_dropdownManager.CloseAll();
            //     return;
            // }

            if (m_menuStack.Count > 0)
            {
                m_menuStack.Peek().OnBackKeyEvent();
            }
        }
    }
}