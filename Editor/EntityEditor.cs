using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.Style;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Search;
using Mitfart.LeoECSLite.UnityIntegration.View;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.Style.StyleConsts;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor {
   [CustomEditor(typeof(EntityView))]
   public class EntityEditor : UnityEditor.Editor {
      private const string _COMPONENTS_FIELD       = nameof(EntityView.components);
      private const string _COMPONENTS_COUNT_FIELD = nameof(EntityView.componentsCount);

      private const string _ADD_BTN_TEXT  = "Add";
      private const string _DEL_BTN_TEXT  = "Del";
      private const string _KILL_BTN_TEXT = "Kill";

      private VisualElement   _root;
      private VisualElement   _header;
      private Label           _title;
      private Button          _killBtn;
      private Button          _delBtn;
      private Button          _addBtn;
      private VisualElement   _componentsContainer;
      private PropertyField[] _componentsDrawers = Array.Empty<PropertyField>();

      private EntityView Target          => (EntityView)target;
      private EcsWorld   World           => Target.World;
      private int        Entity          => Target.Entity;
      private string     Label           => Target.BakedIndex;
      private int        ComponentsCount => Target.componentsCount;



      public override VisualElement CreateInspectorGUI() {
         CreateElements();
         StructureElements();
         InitElements();
         return _root;
      }


      private void CreateElements() {
         _root                = new VisualElement();
         _header              = new Box();
         _title               = new Label();
         _addBtn              = new Button(ChooseAndAdd) { text = _ADD_BTN_TEXT };
         _delBtn              = new Button(ChooseAndDel) { text = _DEL_BTN_TEXT };
         _killBtn             = new Button(Kill) { text         = _KILL_BTN_TEXT };
         _componentsContainer = new VisualElement();
      }

      private void StructureElements() {
         _root
           .AddChild(_header.AddChild(_title))
           .AddChild(_componentsContainer);

         if (Target.IsAlive)
            _header
              .AddChild(_addBtn)
              .AddChild(_delBtn)
              .AddChild(_killBtn);
      }

      private void InitElements() {
         _root
           .style
           .marginLeft = -REM;

         _header
           .style
           .Margin(hor: 0, REM_025)
           .Padding(REM_05)
           .FlexRow()
           .BorderRadius(REM_05)
           .FontStyle(FontStyle.Bold)
           .FontSize(REM_125)
           .OverflowHidden();

         _title
           .SetText(Label)
           .style
           .FlexGrow();

         _addBtn.style.width = _delBtn.style.width = _killBtn.style.width = REM * 5;

         RefreshComponents();
         _root.TrackPropertyValue(ComponentsCountProperty(), _ => RefreshComponents());
      }


      private void RefreshComponents() {
         IList<SerializedProperty> properties = ComponentsProperty().GetChildren(ComponentsCount + 1); // "0" - "Size" property => shift by 1

         if (NotEnoughDrawers())
            ResizeForNewDrawers();

         for (var i = 0; i < ComponentsCount; i++) {
            PropertyField      drawer    = GetField(i);
            SerializedProperty component = GetProperty(i);

            if (AlreadyBinded(drawer, component))
               continue;

            drawer.BindProperty(component);
            drawer.RegisterValueChangeCallback(_ => ((ComponentView)component.GetUnderlyingValue()).SetValue());
         }

         for (int i = ComponentsCount; i < _componentsDrawers.Length; i++) {
            DisableDrawer(i);
         }

         return;


         PropertyField GetField(int i) {
            if (_componentsDrawers[i] != null)
               return _componentsDrawers[i];

            var newField = new PropertyField();
            _componentsContainer.AddChild(newField);
            _componentsDrawers[i] = newField;

            return newField;
         }

         bool AlreadyBinded(IBindable drawer, SerializedProperty component) => component.propertyPath == drawer.bindingPath;

         SerializedProperty GetProperty(int   i)  => properties[i + 1]; // "0" - "Size" property => shift by 1
         void               DisableDrawer(int i)  => _componentsDrawers[i].style.display = DisplayStyle.None;
         void               ResizeForNewDrawers() => Array.Resize(ref _componentsDrawers, ComponentsCount);
         bool               NotEnoughDrawers()    => ComponentsCount > _componentsDrawers.Length;
      }



      private void ChooseAndAdd() => ComponentsSearchWindow.OpenFor(World, AddComponent);
      private void ChooseAndDel() => ComponentsSearchWindow.OpenFor(World, Entity, DelComponent);



      private bool AddComponent(Type comp) {
         IEcsPool pool = World.GetPoolByType(comp);

         if (pool.Has(Entity))
            return false;

         pool.AddRaw(Entity, Activator.CreateInstance(comp));
         return true;
      }

      private bool DelComponent(Type comp) {
         IEcsPool pool = World.GetPoolByType(comp);

         if (!pool.Has(Entity))
            return false;

         pool.Del(Entity);
         return true;
      }

      private void Kill() => World.DelEntity(Entity);



      private SerializedProperty ComponentsCountProperty() => serializedObject.FindProperty(_COMPONENTS_COUNT_FIELD);
      private SerializedProperty ComponentsProperty()      => serializedObject.FindProperty(_COMPONENTS_FIELD);
   }
}