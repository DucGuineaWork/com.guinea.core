using UnityEngine;


namespace Guinea.Core.UI.Menus
{
    public interface IMenu: ILocale
    {
        bool DestroyWhenClosed { get; }
        string Name { get; }
        IMenu[] ChildrenMenu { get; }
        MenuManager MenuManager { get; }
        GameObject gameObject { get; }
        Transform transform { get; }
        void OnOpenMenu<T>(T args);
        void OnCloseMenu();
        void OnBackKeyEvent();
    }
}