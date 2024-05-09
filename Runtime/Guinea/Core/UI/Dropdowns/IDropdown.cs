using UnityEngine;

namespace Guinea.Core.UI.Dropdowns
{
    public interface IDropdown: ILocale
    {
        GameObject gameObject { get; }
        Transform transform { get; }
        void OnOpen<T>(T args);
        void OnClose();
    }
}