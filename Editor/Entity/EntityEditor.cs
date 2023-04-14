using System;
using System.Collections.Generic;
using GenericUnityObjects;
using LeoECSLite.UnityIntegration.Editor.Component;
using LeoECSLite.UnityIntegration.Editor.Extentions.Style.Border;
using LeoECSLite.UnityIntegration.Editor.Extentions.Style.Overflow;
using LeoECSLite.UnityIntegration.Editor.Extentions.Style.Spacing;
using LeoECSLite.UnityIntegration.Editor.Extentions.Style.Text;
using LeoECSLite.UnityIntegration.Editor.Extentions.UIElement;
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
    private static VisualElement _TitleContainer;
    private static Label         _Title;
    private static VisualElement _ComponentsList;

    private static EntityView _TargetEntityView;

    private static EcsWorld World       => _TargetEntityView.World;
    private static int      Entity      => _TargetEntityView.Entity;
    private static bool     EntityAlive => World.IsEntityAlive(Entity);



    private void OnEnable() {
      _ComponentsCache ??= new Dictionary<Type, ComponentCache>();

      _ComponentsList ??= new VisualElement();
      _Title          ??= new Label();
      _TitleContainer ??= CreateTitleContainer();
      _Root           ??= CreateRoot();


      EditorApplication.update += Refresh;

      if (target is not EntityView entityView)
        return;

      _TargetEntityView = entityView;
      Refresh();
    }
    
    private void OnDisable() {
      EditorApplication.update -= Refresh;
      _TargetEntityView        =  null;
    }

    public override VisualElement CreateInspectorGUI() {
      Refresh();
      return _Root;
    }



    private static void Refresh() {
      if (_TargetEntityView == null)
        return;

      if (!EntityAlive) {
        _Title.SetText($"{_TargetEntityView.name} | DEAD");
        HideAllComponents();
      }
      else {
        _Title.SetText(
          ActiveDebugSystems.TryGet(_TargetEntityView.World, out EcsWorldDebugSystem system)
            ? _TargetEntityView.Entity.ToString(system.NameSettings.Format)
            : _TargetEntityView.Entity.ToString()
        );
        HideUnusedComponents(Entity);
        ShowFreshComponents(Entity, World);
      }
    }



    private static void ShowFreshComponents(int e, EcsWorld world) {
      Type[] types = StaticCache.Types;
      int    count = world.GetComponentTypes(e, ref types);

      for (var i = 0; i < count; i++) {
        Type componentType = types[i];

        ComponentCache cache         = GetComponentCache(componentType);
        ComponentData  componentData = cache.Data;

        componentData.Init(e, world)
                     .RefreshValue();
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



    private static ComponentCache GetComponentCache(Type componentType) {
      return _ComponentsCache.TryGetValue(componentType, out ComponentCache cache)
        ? cache
        : CreateCache(componentType);
    }

    private static ComponentCache CreateCache(Type componentType) {
      ComponentData componentData = CreateData(componentType);
      VisualElement view          = CreateView(componentData);

      var cache = new ComponentCache {
        Data = componentData,
        View = view
      };

      _ComponentsList.Add(view);
      _ComponentsCache.Add(componentType, cache);

      return cache;
    }

    private static ComponentData CreateData(Type componentType) {
      Type type = typeof(ComponentData<>).MakeGenericType(componentType);
      var  data = (ComponentData) GenericScriptableObject.CreateInstance(type);
      return data;
    }

    private static VisualElement CreateView(ComponentData componentData) {
      var viewRoot  = new VisualElement();
      var container = new Box();
      var inspector = new InspectorElement(componentData);

      container
       .style
       .Margin(0, REM_025)
       .Padding(0, REM_05)
       .BorderRadius(REM_05);

      viewRoot.AddChild(container.AddChild(inspector));

      return viewRoot;
    }



    private static VisualElement CreateRoot() {
      var root = new VisualElement();
      root
       .AddChild(_TitleContainer.AddChild(_Title))
       .AddChild(_ComponentsList);
      return root;
    }

    private static VisualElement CreateTitleContainer() {
      var titleContainer = new Box();
      titleContainer
       .style
       .Margin(0, REM_025)
       .Padding(REM_05)
       .BorderRadius(REM_05)
       .FontStyle(FontStyle.Bold)
       .FontSize(REM_125)
       .OverflowHidden();
      return titleContainer;
    }
  }
}