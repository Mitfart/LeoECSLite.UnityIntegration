using LeoECSLite.UnityIntegration.Editor.Extensions;
using LeoECSLite.UnityIntegration.Editor.Extensions.Spacing;
using LeoECSLite.UnityIntegration.Editor.Extensions.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static LeoECSLite.UnityIntegration.Editor.Extensions.StyleConsts;

namespace LeoECSLite.UnityIntegration.Editor.Component {
  [CustomPropertyDrawer(typeof(ComponentView), true)]
  public class ComponentEditor : PropertyDrawer {
    private const string COMPONENT_FIELD = "component";

    private SerializedProperty _property;

    private VisualElement _root;
    private VisualElement _header;
    private Label         _label;
    private VisualElement _main;
    private VisualElement _fields;



    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
      _property = property;
      CreateElements();
      StructureElements();
      InitElements();
      return _root;
    }



    private void CreateElements() {
      _root   = new VisualElement();
      _header = new VisualElement();
      _label  = new Label();
      _main   = new VisualElement();
      _fields = new VisualElement();
    }

    private void StructureElements() {
      _root
       .AddChild(_header.AddChild(_label))
       .AddChild(_main.AddChild(_fields.AddChildProperties(ComponentProperty())));
    }

    private void InitElements() {
      _label
       .SetText(ComponentName())
       .style
       .Margin(top: 0, bottom: 0, left: -REM_05, right: 0)
       .FontStyle(FontStyle.Bold);
    }



    private string             ComponentName()     => Target().ComponentType.Name;
    private ComponentView      Target()            => (ComponentView) _property.managedReferenceValue;
    private SerializedProperty ComponentProperty() => _property.FindPropertyRelative(COMPONENT_FIELD);
  }
}