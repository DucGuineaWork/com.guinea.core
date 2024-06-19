using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Guinea.Core.UI.Effects;


namespace Guinea.Core.UI.Effects
{
    [CustomPropertyDrawer(typeof(TranslationEffect))]
    public class TranslationEffectDrawer: PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            container.Add(new Label($"{typeof(TranslationEffect).Name}"));
            var configField = new PropertyField(property.FindPropertyRelative("m_config"));
            var valueField = new PropertyField(property.FindPropertyRelative("m_value"));
            container.Add(configField);
            container.Add(valueField);
            return container;
        }
    }
}