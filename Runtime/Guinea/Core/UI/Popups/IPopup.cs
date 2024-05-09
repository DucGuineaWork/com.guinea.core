using UnityEngine;
using Guinea.Core.UI;

namespace Guinea.Core.UI.Popups
{
    public interface IPopup: ILocale
    {
        bool DestroyWhenClosed{get;}
        void OnOpen<T>(T args);
        void OnBackKeyEvent();
        Transform transform{get;}
        GameObject gameObject{get;}
    }
}