using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Entity;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Filter.View;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Layout;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.World;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window {
  public class EcsDebugWindow : BaseEcsDebugWindow {
    private const string STYLES_PATH   = "LeoECSLite.UnityIntegration/uss/index";
    private const string SPLIT_VIEW_CL = "split-view";

    private readonly Dictionary<int, VisualElement> _selectedEntities = new();
    private          HorizontalTwoPanelLayout       _content;
    private          ScrollView                     _entitiesInspector;
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



    [MenuItem("LeoECS Lite/Debug Window")]
    public static void OpenEcsDebugWindow() {
      GetWindow<EcsDebugWindow>(nameof(EcsDebugWindow)).Show();
    }



    protected override void CreateElements() {
      _filter = new Filter.Filter();

      _header     = new VisualElement();
      _filterView = new FilterView(_filter);

      _content           = new HorizontalTwoPanelLayout();
      _worldTabsMenu     = new TabsMenu<WorldTabData>(ChangeWorld);
      _entitiesList      = new EntitiesList(_filter);
      _entitiesInspector = new ScrollView();
    }

    protected override void StructureElements() {
      rootVisualElement
       .AddChild(
          _header
           .AddChild(_worldTabsMenu)
           .AddChild(_filterView)
        )
       .AddChild(_content);

      _content.Left.AddChild(_entitiesList);
      _content.Right.AddChild(_entitiesInspector);
    }

    protected override void InitElements() {
      rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>(STYLES_PATH));

      _content.AddToClassList(SPLIT_VIEW_CL);

      if (!Application.isPlaying)
        return;

      AddWorldTabs();
    }



    public override void OnEntityCreated(int e) {
      _entitiesList.Add(e);
    }

    public override void OnEntityDestroyed(int e) {
      _entitiesList.Remove(e);

      if (_selectedEntities.ContainsKey(e))
        UnselectEntity(e);
    }

    public override void OnWorldDestroyed(EcsWorld world) {
      _worldTabsMenu.RemoveTab(
        _worldTabsMenu
         .Where(data => data.World == world)
         .First()
      );
    }



    protected override void InitInspector() {
      _filter.Init(ActiveSystem);
      _entitiesList.Setup(ActiveSystem);

      _entitiesList.OnSelect   += SelectEntity;
      _entitiesList.OnUnselect += UnselectEntity;
    }

    protected override void ResetInspector() {
      ClearSelectedEntities();
      ClearEntities();
      ClearFilter();
    }

    private void ClearFilter() {
      _filter?.Reset();
      _filterView?.Reset();
    }

    private void ClearEntities() {
      if (_entitiesList == null)
        return;

      _entitiesList.Reset();
      _entitiesList.OnSelect   -= SelectEntity;
      _entitiesList.OnUnselect -= UnselectEntity;
    }

    private void ClearSelectedEntities() {
      if (_selectedEntities == null)
        return;

      foreach (VisualElement view in _selectedEntities.Values)
        _entitiesInspector.Remove(view);

      _selectedEntities.Clear();
    }


    private void SelectEntity(int e) {
      VisualElement inspector = EntityInspector(e);

      _entitiesInspector.Add(inspector);
      _selectedEntities.Add(e, inspector);
    }

    private void UnselectEntity(int e) {
      try {
        _entitiesInspector.Remove(_selectedEntities[e]);
        _selectedEntities.Remove(e);
      }
      catch (Exception exception) {
        Debug.Log($"Error: {e} -> {exception}");
      }
    }



    private void AddWorldTabs() {
      ActiveDebugSystems.Foreach(sys => _worldTabsMenu.AddTab(new WorldTabData(sys.WorldName, sys.World)));
    }

    private VisualElement EntityInspector(int e) => new InspectorElement(ActiveSystem.View.GetEntityView(e));
  }
}