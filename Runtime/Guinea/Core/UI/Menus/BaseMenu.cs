using UnityEngine;

namespace Guinea.Core.UI.Menus
{
    public class BaseMenu: MonoBehaviour, IMenu
    {
        [SerializeField]protected bool m_destroyWhenClosed;
        [SerializeField]protected bool m_disableParent;
        [SerializeField]protected bool m_closeSameLayer;
        [SerializeField]protected int m_layer;
        public MenuManager MenuManager{get; protected set;}
        public virtual IMenu[] ChildrenMenu => null;
        public bool DestroyWhenClosed => m_destroyWhenClosed;
        public bool DisableParent => m_disableParent;
        public bool CloseSameLayer => m_closeSameLayer;
        public virtual string Name => s_name;
        public int Layer => m_layer;

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

        public virtual void Refresh()
        {
        }
    }
}