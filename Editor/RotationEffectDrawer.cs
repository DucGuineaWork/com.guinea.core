using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


namespace Guinea.Core.UI.Effects
{
    [CustomPropertyDrawer(typeof(RotationEffect))]
    public class RotationEffectDrawer: PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            container.Add(new Label($"{typeof(RotationEffect).Name}"));
            var configField = new PropertyField(property.FindPropertyRelative("m_config"));
            var valueField = new PropertyField(property.FindPropertyRelative("m_euler"));
            container.Add(configField);
            container.Add(valueField);
            return container;
        }
    }
}