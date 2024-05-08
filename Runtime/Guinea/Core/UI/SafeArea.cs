using UnityEngine;

namespace Guinea.Core.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        private RectTransform m_rect;

        void Awake()
        {
            m_rect = GetComponent<RectTransform>();
        }

        void OnRectTransformDimensionsChange()
        {
            Refresh();
        }

        private void Refresh()
        {
            //* Get the safe area of the screen in screen coordinates.
            Rect safeArea = Screen.safeArea;

            //* Convert the safe area to local coordinates of the RectTransform.
            Vector2 minAnchor = new Vector2(safeArea.x / Screen.width, safeArea.y / Screen.height);
            Vector2 maxAnchor = new Vector2((safeArea.x + safeArea.width) / Screen.width, (safeArea.y + safeArea.height) / Screen.height);

            //* Set the anchors and offsets of the RectTransform to match the safe area.
            m_rect.anchorMin = minAnchor;
            m_rect.anchorMax = maxAnchor;
            m_rect.offsetMin = Vector2.zero;
            m_rect.offsetMax = Vector2.zero;
        }
    }
}