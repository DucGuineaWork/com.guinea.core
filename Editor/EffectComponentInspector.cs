using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Guinea.Core.UI.Effects
{
    [CustomEditor(typeof(EffectComponent))]
    public class EffectComponentInspector : UnityEditor.Editor
    {
        public VisualTreeAsset m_visualTreeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = new VisualElement();
            m_visualTreeAsset = m_visualTreeAsset?? AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.guinea.core/Editor/EffectComponentInspector.uxml");
            m_visualTreeAsset.CloneTree(inspector);

            VisualElement defaultInspector = inspector.Q("DefaultInspector");
            InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);

            Button btnAddItem = inspector.Q<Button>("AddItem");
            AddContextMenu(btnAddItem);
            return inspector;
        }

        private void AddContextMenu(VisualElement element)
        {
            ContextualMenuManipulator contextMenu = new ContextualMenuManipulator(AddMenuItems);

            contextMenu.activators.Clear();
            contextMenu.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse
            });

            element.AddManipulator(contextMenu);
        }

        private void AddMenuItems(ContextualMenuPopulateEvent evt)
        {
            EffectComponent effectComponent = serializedObject.targetObject as EffectComponent;
            List<Type> effectImplements = new List<Type>
            {
                typeof(TranslationEffect),
                typeof(ScaleEffect),
                typeof(RotationEffect),
                typeof(CallbackEffect),
                typeof(DelayEffect),
            };

            foreach (var effectImplement in effectImplements)
            {
                evt.menu.AppendAction($"Add {effectImplement.Name}", (x) => effectComponent.AddEffect(effectImplement), DropdownMenuAction.AlwaysEnabled);
            }
        }
    }
}