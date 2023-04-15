using System;
using System.Collections.Generic;
using System.Linq;
using LeoECSLite.UnityIntegration.Editor.Extentions.UIElement;
using LeoECSLite.UnityIntegration.Editor.Window.Entity;
using LeoECSLite.UnityIntegration.Editor.Window.Filter.View;
using LeoECSLite.UnityIntegration.Editor.Window.Layout;
using LeoECSLite.UnityIntegration.Editor.Window.World;
using LeoECSLite.UnityIntegration.Entity;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Window {
  public class EcsDebugWindow : BaseEcsDebugWindow {
    private const string STYLES_PATH   = "LeoECSLite.UnityIntegration/uss/index";
    private const string SPLIT_VIEW_CL = "split-view";

    private readonly Dictionary<int, VisualElement> _activeEntities = new();
    private          HorizontalTwoPanelLayout       _content;
    private          ScrollView                     _entitiesContainer;
    private          EntitiesList                   _entitiesList;

    private Filter.Filter _filter;
    private FilterView    _filterView;

    private VisualElement          _header;
    private TabsMenu<WorldTabData> _worldTabsMenu;


    private void OnInspectorUpdate() {
      if (!Application.isPlaying)
        return;

      _entitiesList.Refresh();
    }



    [MenuItem("LeoECS Lite/Debug Window NEW")]
    public static void OpenEcsDebugWindow() {
      GetWindow<EcsDebugWindow>(nameof(EcsDebugWindow))
       .Show();
    }


    protected override void CreateElements() {
      _filter = new Filter.Filter();

      _header     = new VisualElement();
      _filterView = new FilterView(_filter);

      _content           = new HorizontalTwoPanelLayout();
      _worldTabsMenu     = new TabsMenu<WorldTabData>(ChangeWorld);
      _entitiesList      = new EntitiesList(_filter);
      _entitiesContainer = new ScrollView();
    }

    protected override void AddElements() {
      rootVisualElement
       .AddChild(
          _header
           .AddChild(_worldTabsMenu)
           .AddChild(_filterView)
        )
       .AddChild(_content);

      _content
       .Left
       .AddChild(_entitiesList);

      _content
       .Right
       .AddChild(_entitiesContainer);
    }

    protected override void InitElements() {
      rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>(STYLES_PATH));

      _content.AddToClassList(SPLIT_VIEW_CL);

      if (!Application.isPlaying)
        return;

      ActiveDebugSystems.Foreach(
        sys => {
          _worldTabsMenu.AddTab(
            new WorldTabData(
              sys.World,
              sys.WorldName
            )
          );
        }
      );
    }


    public override void OnEntityCreated(int e) {
      _entitiesList.Add(e);
    }

    public override void OnEntityDestroyed(int e) {
      _entitiesList.Remove(e);

      if (_activeEntities.ContainsKey(e))
        RemoveActiveEntity(e);
    }

    public override void OnWorldDestroyed(EcsWorld world) {
      _worldTabsMenu.RemoveTab(
        _worldTabsMenu
         .GetWhere(data => data.World == world)
         .First()
      );
    }



    protected override void InitInspector() {
      _filter.Init(ActiveDebugSystem);
      _entitiesList.Setup(ActiveDebugSystem);

      _entitiesList.OnSelectEntity   += AddActiveEntity;
      _entitiesList.OnUnselectEntity += RemoveActiveEntity;
    }

    protected override void ResetInspector() {
      _entitiesList.Reset();
      ClearActiveEntities();

      _filter.Reset();
      _filterView.Reset();

      _entitiesList.OnSelectEntity   -= AddActiveEntity;
      _entitiesList.OnUnselectEntity -= RemoveActiveEntity;
    }

    private void ClearActiveEntities() {
      if (_activeEntities == null)
        return;

      foreach (VisualElement view in _activeEntities.Values)
        _entitiesContainer.Remove(view);

      _activeEntities.Clear();
    }


    private VisualElement CreateEntityInspector(int e) {
      EntityView entityView = ActiveDebugSystem.View.GetEntityView(e);
      return new InspectorElement(entityView);
    }

    private void AddActiveEntity(int e) {
      VisualElement inspector = CreateEntityInspector(e);

      _entitiesContainer.Add(inspector);
      _activeEntities.Add(e, inspector);
    }

    private void RemoveActiveEntity(int e) {
      try {
        _entitiesContainer.Remove(_activeEntities[e]);
        _activeEntities.Remove(e);
      }
      catch (Exception exception) {
        Debug.Log($"Error on: {e}   ->   {exception}");
      }
    }
  }
}