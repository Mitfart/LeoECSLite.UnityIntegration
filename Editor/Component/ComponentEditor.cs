using LeoECSLite.UnityIntegration.Editor.Extentions.Style.Spacing;
using LeoECSLite.UnityIntegration.Editor.Extentions.Style.Text;
using LeoECSLite.UnityIntegration.Editor.Extentions.UIElement;
using LeoECSLite.UnityIntegration.Extentions.Type;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static LeoECSLite.UnityIntegration.Editor.Extentions.Style.StyleConsts;

namespace LeoECSLite.UnityIntegration.Editor.Component {
  [CustomEditor(typeof(ComponentData<>), true)]
  public class ComponentEditor : UnityEditor.Editor {
    private const string COMPONENT_PROPERTY_NAME = nameof(ComponentData<Vector2>.component);

    private ComponentData _target;
    
    private VisualElement _root;
    private Label _typeLabel;
    private VisualElement _fields;
    


    public override VisualElement CreateInspectorGUI() {
      CreateElements();
      AddElements();
      InitElements();
      return _root;
    }



    private void CreateElements() {
      _root      = new VisualElement();
      _typeLabel = new Label();
      _fields    = new VisualElement();
    }

    private void AddElements() {
      _root
       .AddChild(_typeLabel)
       .AddChild(_fields)
        ;
    }

    private void InitElements() {
      _target = ComponentView();

      InitLabel();
      InitFields();
    }



    private void InitLabel() {
      _typeLabel
       .SetText(_target.Type.GetCleanName())
       .style
       .Margin(0, 0, -REM_05, 0)
       .FontStyle(FontStyle.Bold);
    }

    private void InitFields() {
      SerializedProperty data = Component();

      if (data != null)
        _fields.AddChildProperties(data);
    }



    private ComponentData ComponentView() {
      return (ComponentData) serializedObject.targetObject;
    }

    private SerializedProperty Component() {
      return serializedObject.FindProperty(COMPONENT_PROPERTY_NAME);
    }
  }
}