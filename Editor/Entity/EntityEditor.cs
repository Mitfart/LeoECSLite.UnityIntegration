using System;
using System.Collections.Generic;
using GenericUnityObjects;
using LeoECSLite.UnityIntegration.Editor.Component;
using LeoECSLite.UnityIntegration.Editor.Extentions.Style.Border;
using LeoECSLite.UnityIntegration.Editor.Extentions.Style.FlexDirection;
using LeoECSLite.UnityIntegration.Editor.Extentions.Style.FlexGrow;
using LeoECSLite.UnityIntegration.Editor.Extentions.Style.Overflow;
using LeoECSLite.UnityIntegration.Editor.Extentions.Style.Spacing;
using LeoECSLite.UnityIntegration.Editor.Extentions.Style.Text;
using LeoECSLite.UnityIntegration.Editor.Extentions.UIElement;
using LeoECSLite.UnityIntegration.Editor.Search;
using LeoECSLite.UnityIntegration.Entity;
using LeoECSLite.UnityIntegration.Extentions.EcsWorld;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static LeoECSLite.UnityIntegration.Editor.Extentions.Style.StyleConsts;

namespace LeoECSLite.UnityIntegration.Editor.Entity {
  [CustomEditor(typeof(EntityView))]
  public class EntityEditor : UnityEditor.Editor {
    private static Dictionary<Type, ComponentCache> _ComponentsCache;

    private static VisualElement _Root;
    private static VisualElement _Header;
    private static Label         _Title;
    private static VisualElement _ButtonsContainer;
    private static Button        _AddComponentBtn;
    private static Button        _DelComponentBtn;
    private static Button        _KillBtn;
    private static VisualElement _ComponentsList;

    private static EntityView _TargetEntityView;

    private static EcsWorld World       => _TargetEntityView.World;
    private static int      Entity      => _TargetEntityView.Entity;
    private static bool     EntityAlive => World.IsEntityAlive(Entity);



    private void OnEnable() {
      _ComponentsCache ??= new Dictionary<Type, ComponentCache>();

      _Root ??= CreateInspector();


      if (target is not EntityView entityView)
        return;

      _TargetEntityView = entityView;
      DrawInspector();
      EditorApplication.update += DrawInspector;
    }

    private void OnDisable() {
      EditorApplication.update -= DrawInspector;
      _TargetEntityView        =  null;
    }

    public override VisualElement CreateInspectorGUI() {
      DrawInspector();
      return _Root;
    }



    private static void DrawInspector() {
      if (_TargetEntityView == null)
        return;

      if (EntityAlive)
        DrawAliveInspector();
      else
        DrawDeadInspector();
    }

    private static void DrawAliveInspector() {
      _Title.SetText(
        ActiveDebugSystems.TryGet(_TargetEntityView.World, out EcsWorldDebugSystem system)
          ? _TargetEntityView.Entity.ToString(system.NameSettings.Format)
          : _TargetEntityView.Entity.ToString()
      );

      HideUnusedComponents(Entity);
      ShowFreshComponents(Entity, World);
    }

    private static void DrawDeadInspector() {
      _Title.SetText($"{_TargetEntityView.name} | DEAD");

      HideAllComponents();
    }



    private static void ShowFreshComponents(int e, EcsWorld world) {
      Type[] types = StaticCache.Types;
      int    count = world.GetComponentTypes(e, ref types);

      for (var i = 0; i < count; i++) {
        Type component = types[i];

        ComponentCache cache         = GetComponentCache(component);
        ComponentData  componentData = cache.Data;

        componentData
         .Init(e, world)
         .Refresh();
      }
    }

    private static void HideUnusedComponents(int e) {
      foreach (ComponentCache cache in _ComponentsCache.Values)
        cache.View.style.display = IsUsed(cache.Data)
          ? DisplayStyle.Flex
          : DisplayStyle.None;

      bool IsUsed(ComponentData data) {
        return data.Entity == e && data.Pool.Has(data.Entity);
      }
    }

    private static void HideAllComponents() {
      foreach (ComponentCache cache in _ComponentsCache.Values)
        cache.View.style.display = DisplayStyle.None;
    }



    private static ComponentCache GetComponentCache(Type component) {
      return _ComponentsCache.TryGetValue(component, out ComponentCache cache)
        ? cache
        : CreateComponentCache(component);
    }

    private static ComponentCache CreateComponentCache(Type component) {
      ComponentData componentData = CreateComponentData(component);
      VisualElement view          = CreateComponentView(componentData);

      var cache = new ComponentCache {
        Data = componentData,
        View = view
      };

      _ComponentsList.Add(view);
      _ComponentsCache.Add(component, cache);

      return cache;
    }

    private static ComponentData CreateComponentData(Type component) {
      Type type = typeof(ComponentData<>).MakeGenericType(component);
      var  data = (ComponentData) GenericScriptableObject.CreateInstance(type);
      return data;
    }

    private static VisualElement CreateComponentView(ComponentData componentData) {
      var viewRoot  = new VisualElement();
      var container = new Box();
      var inspector = new InspectorElement(componentData);

      container
       .style
       .Margin(0, REM_025)
       .Padding(0, REM_05)
       .BorderRadius(REM_05);

      viewRoot
       .AddChild(
          container
           .AddChild(inspector)
        );

      return viewRoot;
    }



    private static VisualElement CreateInspector() {
      var root = new VisualElement();

      _Header         = CreateHeader();
      _ComponentsList = new VisualElement();


      root
       .AddChild(_Header)
       .AddChild(_ComponentsList);
      return root;
    }

    private static VisualElement CreateHeader() {
      var header = new Box();

      _Title            = new Label();
      _ButtonsContainer = new VisualElement();

      _AddComponentBtn = new Button(ChooseAndAdd) { text = "Add" };
      _DelComponentBtn = new Button(ChooseAndDel) { text = "Del" };
      _KillBtn         = new Button(Kill) { text         = "Kill" };

      header
       .AddChild(_Title)
       .AddChild(
          _ButtonsContainer
           .AddChild(_AddComponentBtn)
           .AddChild(_DelComponentBtn)
           .AddChild(_KillBtn)
        );


      header
       .style
       .Margin(0, REM_025)
       .Padding(REM_05)
       .FlexRow()
       .BorderRadius(REM_05)
       .FontStyle(FontStyle.Bold)
       .FontSize(REM_125)
       .OverflowHidden();

      _Title
       .style
       .FlexGrow();

      _ButtonsContainer
       .style
       .FlexRow();

      _AddComponentBtn.style.width
        = _DelComponentBtn.style.width
          = _KillBtn.style.width
            = 48;

      return header;
    }



    private static void ChooseAndAdd() {
      ComponentsSearchWindow.OpenFor(
        World,
        AddComponent
      );
    }

    private static void ChooseAndDel() {
      ComponentsSearchWindow.OpenFor(
        World,
        Entity,
        DelComponent
      );
    }



    private static bool AddComponent(Type comp) {
      IEcsPool pool = World.GetPoolByType(comp);

      if (pool.Has(Entity))
        return false;

      pool.AddRaw(
        Entity,
        Activator.CreateInstance(comp)
      );
      return true;
    }

    private static bool DelComponent(Type comp) {
      IEcsPool pool = World.GetPoolByType(comp);

      if (!pool.Has(Entity))
        return false;

      pool.Del(Entity);
      return true;
    }

    private static void Kill() {
      World.DelEntity(Entity);
    }
  }
}