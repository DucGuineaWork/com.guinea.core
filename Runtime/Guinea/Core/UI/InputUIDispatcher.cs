using System;
using Guinea.Core.UI.Dropdowns;
using Guinea.Core.UI.Popups;
using UnityEngine;

namespace Guinea.Core.UI
{
    public class InputUIDispatcher : MonoBehaviour
    {
        public static event Action OnMenuBackKeyEvent;

        void Update()
        {
            // * BackKey event
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PopupManager.HavePopupOpened)
                {
                    PopupManager.OnBackKeyEvent();
                }
                else if (DropdownManager.HasDropdownOpened)
                {
                    DropdownManager.CloseAll();
                }
                else
                {
                    OnMenuBackKeyEvent?.Invoke();
                }
            }
        }
    }
}