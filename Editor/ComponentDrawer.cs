using System;
using System.Collections;
using System.Reflection;
using LeoECSLite.UnityIntegration.Attributes;
using LeoECSLite.UnityIntegration.View;
using Leopotam.EcsLite;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Git.Extensions.Editor;
using Git.Extensions.Editor.Style.Border;
using Git.Extensions.Editor.Style.Spacing;
using Git.Extensions.Editor.Style.Text;
using LeoECSLite.UnityIntegration.Editor.Extensions;
using static Git.Extensions.Editor.Style.StyleConsts;
using Label = UnityEngine.UIElements.Label;

namespace LeoECSLite.UnityIntegration.Editor {
  [CustomPropertyDrawer(typeof(ComponentView), true)]
  public class ComponentDrawer : PropertyDrawer {
    private const string COMPONENT_NAME_FIELD = nameof(ComponentView.componentName);
    private const string COMPONENT_FIELD      = nameof(ComponentView.component);

    private Box           _root;
    private VisualElement _header;
    private Label         _label;
    private VisualElement _main;
    private VisualElement _fields;

    private SerializedProperty _property;
    private ComponentView      _target;

    private object Component     => _target.component;
    private Type   ComponentType => _target.ComponentType;



    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
      _property = property;
      _target   = (ComponentView) property.GetUnderlyingValue();

      CreateElements();
      StructureElements();
      InitElements();
      return _root;
    }



    private void CreateElements() {
      _root   = new Box();
      _header = new VisualElement();
      _label  = new Label();
      _main   = new VisualElement();
      _fields = new VisualElement();
    }

    private void StructureElements() {
      _root
       .AddChild(_header.AddChild(_label))
       .AddChild(_main.AddChild(_fields.AddChildPropertiesOf(ComponentProperty())));
      AddPackedEntities();
    }

    private void InitElements() {
      _root
       .style
       .Margin(0, REM_025)
       .Padding(REM_05)
       .BorderRadius(REM_05);

      _label
       .SetText(ComponentName())
       .style
       .FontStyle(FontStyle.Bold);

      _main.style.paddingLeft = REM;
    }



    private void AddPackedEntities() {
      if (!ComponentsWithPackedEntities.Contains(ComponentType))
        return;

      foreach (FieldInfo field in ComponentType.GetFields()) {
        if (!field.HasAttribute<PackedEntityAttribute>())
          continue;

        string title      = field.HumanName();
        object fieldValue = field.GetValueOptimized(Component);

        _main.Add(
          fieldValue is IEnumerable entities
            ? PackedEntitiesArrayView(entities, field, title)
            : PackedEntityView(entity: fieldValue, field, title)
        );
      }
    }

    private VisualElement PackedEntitiesArrayView(IEnumerable entities, FieldInfo field, string title) {
      return new Foldout { text = title }
       .AddAllOf(
          entities,
          entity => PackedEntityView(entity, field)
        );
    }

    private EntityField PackedEntityView(object entity, FieldInfo field, string title = null) {
      return entity switch {
        int raw => new EntityField(
          raw,
          _target.World,
          newView => field.SetValueOptimized(Component, newView.Entity),
          title
        ),
        EcsPackedEntity packed => new EntityField(
          packed,
          _target.World,
          newView => {
            int             e;
            EcsPackedEntity pev;
            
            pev = (EcsPackedEntity) field.GetValueOptimized(Component);
            Debug.Log(!pev.EqualsTo(default) ? pev.Unpack(_target.World, out e) ? e : "DEAD" : "ERROR");
            field.SetValueOptimized(Component, newView.GetPackedEntity());
            pev = (EcsPackedEntity) field.GetValueOptimized(Component);
            Debug.Log(!pev.EqualsTo(default) ? pev.Unpack(_target.World, out e) ? e : "DEAD" : "ERROR");
          },
          title
        ),
        EcsPackedEntityWithWorld packedWithWorld => new EntityField(
          packedWithWorld,
          newView => field.SetValueOptimized(Component, newView.GetPackedEntityWithWorld()),
          title
        ),
        _ => null
      };
    }



    private string             ComponentName()         => ComponentNameProperty().stringValue;
    private SerializedProperty ComponentNameProperty() => _property.FindPropertyRelative(COMPONENT_NAME_FIELD);
    private SerializedProperty ComponentProperty()     => _property.FindPropertyRelative(COMPONENT_FIELD);
  }
}