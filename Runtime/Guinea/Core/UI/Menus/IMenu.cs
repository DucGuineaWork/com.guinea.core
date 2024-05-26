using UnityEngine;


namespace Guinea.Core.UI.Menus
{
    public interface IMenu: ILocale
    {
        bool DestroyWhenClosed { get; }
        bool DisableParent{get;}
        bool CloseSameLayer{get;}
        string Name { get; }
        int Layer{get;}
        IMenu[] ChildrenMenu { get; }
        MenuManager MenuManager { get; }
        GameObject gameObject { get; }
        Transform transform { get; }
        void OnOpenMenu<T>(T args);
        void Refresh();
        void OnCloseMenu();
        void OnBackKeyEvent();
    }
}