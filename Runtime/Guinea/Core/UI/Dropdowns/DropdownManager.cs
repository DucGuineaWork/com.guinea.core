using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Guinea.Core.UI.Dropdowns
{
    public class DropdownManager: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]RectTransform m_dropdownContainer;
        [SerializeField]Image m_image;
        public static bool HasDropdownOpened=> s_dropdownContainer.GetComponentInChildren<IDropdown>() != null;
        private static RectTransform s_dropdownContainer;
        private static Image s_image;

        void Awake()
        {
            s_dropdownContainer = m_dropdownContainer;
            s_image = m_image;
        }

        void OnEnable()
        {
            CloseAll(true);
        }

        public static void Open<T, U>(U args) where T: IDropdown
        {
            IDropdown dropdown = s_dropdownContainer.GetComponentInChildren<T>(true);
            Logger.Assert(dropdown !=null, $"Dropdown {typeof(T)} not exists in container");
            CloseAll();

            // PropertyInfo propertyInfo = dropdown.GetType().GetProperty("DropdownManager");
            // Logger.Assert(propertyInfo != null, "No propertyInfo found");
            // if(propertyInfo.GetValue(dropdown)==null)
            // {
            //     propertyInfo.SetValue(dropdown, this);
            // }
            
            s_image.enabled = true;
            dropdown.transform.SetAsLastSibling();

            dropdown.OnOpen<U>(args);
            dropdown.gameObject.SetActive(true);

            // TODO: Position using clicked event
            EventSystem eventSystem = EventSystem.current;
            RectTransform rect = eventSystem.currentSelectedGameObject.transform as RectTransform;
            
            (dropdown.transform as RectTransform).position = rect.position;
        }

        public static void Close<T>() where T: IDropdown
        {
            IDropdown dropdown = s_dropdownContainer.GetComponentInChildren<T>(true);
            dropdown.OnClose();
            dropdown.gameObject.SetActive(false);
        }

        public static void CloseAll(bool includeInactive=false)
        {
            foreach(IDropdown dropdown in s_dropdownContainer.GetComponentsInChildren<IDropdown>(includeInactive))
            {
                dropdown.OnClose();
                dropdown.gameObject.SetActive(false);
            }
            s_image.enabled = false;
        }

        public void OnPointerClick(PointerEventData ev)
        {
            if(HasDropdownOpened)
            {
                CloseAll();
                return;
            }
        }
    }
}