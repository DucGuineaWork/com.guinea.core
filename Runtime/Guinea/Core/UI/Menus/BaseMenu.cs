using UnityEngine;

namespace Guinea.Core.UI.Menus
{
    public class BaseMenu: MonoBehaviour, IMenu
    {
        [SerializeField]bool m_destroyWhenClosed;
        public MenuManager MenuManager{get; protected set;}
        public bool DestroyWhenClosed => m_destroyWhenClosed;
        public virtual IMenu[] ChildrenMenu => null;
        public virtual string Name => s_name;
        private static readonly string s_name = "Base Menu";

        public virtual void OnOpenMenu<T>(T args)
        {
        }

        public virtual void OnCloseMenu()
        {
        }

        public virtual void OnBackKeyEvent()
        {
            MenuManager.Close(GetType());
        }

        public virtual void OnLocaleLanguageChanged(int language)
        {
        }
    }
}