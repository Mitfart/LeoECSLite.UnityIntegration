using System;
using System.Collections.Generic;
using System.Linq;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Nav;
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW {
   public class NewEDW : EditorWindow{
      private const string DRAG_LINE_NAME = "unity-dragline-anchor";

      private readonly Dictionary<int, UIEntity> _entityViews = new();
      
      private VisualElement _header;
      private Filter        _filter;
      
      private SplitView        _content;
      private VisualElement    _dragLine;
      private Tabs<NamedWorld> _worldTabs;
      private EntitiesList     _entitiesList;
      private ScrollView       _entitiesContainer;

      public EcsWorldDebugSystem ActiveDebugSystem { get; private set; }
      
      private bool _isEntitiesListDirty;

      

      private void CreateGUI() {
         CreateElements();
         AddElements();
         InitElements();
      }


      private void CreateElements() {
         _header = new VisualElement();
         _filter = new Filter(this);
         
         _content           = new SplitView();
         _worldTabs         = new Tabs<NamedWorld>();
         _entitiesList      = new EntitiesList();
         _entitiesContainer = new ScrollView();
         
         _dragLine  = _content.Q<VisualElement>(DRAG_LINE_NAME);
      }

      private void AddElements() {
         rootVisualElement
           .AddChild(
               _header
                 .AddChild(_filter))
           .AddChild(_content);
         
         _content
           .left
           .AddChild(_worldTabs)
           .AddChild(_entitiesList);
         
         _content
           .right
           .AddChild(_entitiesContainer);
      }

      private void InitElements() {
         InitContentContainer();
         
         if (!Application.isPlaying) return;
         if (EcsWorldDebugSystem.ActiveSystems.Count <= 0) return;
         
         InitWorldTabs();
         
         _entitiesList.OnSelectEntities += OnSelectEntities;
      }


      private void InitContentContainer() {
         _content.style.SetBorderColor(Utils.Color_DDD);
         _content.style.borderTopWidth   = Utils.METRICS_MIN;
         _dragLine.style.backgroundColor = Utils.Color_DDD;
      }
      
      private void InitWorldTabs() {
         foreach (EcsWorldDebugSystem system in EcsWorldDebugSystem.ActiveSystems.Values)
            _worldTabs.AddTab(system.NamedWorld);

         _worldTabs.OnChangeTab += ChangeWorld;

         ChangeWorld(_worldTabs.ActiveTab);
      }
      
      
      private void OnSelectEntities(IEnumerable<object> entities) {
         List<object> selectedEntities  = entities.ToList();
         var          entitiesToDispose = new List<int>();
         
         foreach (int entity in _entityViews.Keys) {
            if (!selectedEntities.Contains(entity))
               entitiesToDispose.Add(entity);
            selectedEntities.Remove(entity);
         }

         foreach (int entity in entitiesToDispose) {
            DisposeEntity(entity);
         }
         
         foreach (int entity in selectedEntities) {
            if (!ActiveDebugSystem.View.TryGetEntityView(entity, out MonoEntityView monoView)) continue;

            UIEntity uiView = new UIEntity().Init(monoView);
            _entitiesContainer.Add(uiView);
            _entityViews.Add(entity, uiView);
         }
      }
      
      
      private void OnInspectorUpdate() {
         if (!_isEntitiesListDirty) return;

         _entitiesList.Reset();
         _isEntitiesListDirty = false;
      }

      private void UpdateView() {
         foreach (UIEntity view in _entityViews.Values){
            view.MonoView.UpdateComponentsValues();
            view.MarkDirtyRepaint();
         }
      }
      
      
      
      private void ChangeWorld(NamedWorld newNamedWorld) {
         if (EcsWorldDebugSystem.ActiveSystems.TryGetValue(newNamedWorld.Name, out EcsWorldDebugSystem debugSystem))
            SetActiveWorldDebugSystem(debugSystem);
         else throw new Exception($"Can't find System relative to `{newNamedWorld}` world!");
      }
      
      private void SetActiveWorldDebugSystem(EcsWorldDebugSystem newActiveDebugSystem) {
         if (ActiveDebugSystem == newActiveDebugSystem) return;

         if (ActiveDebugSystem != null){
            ActiveDebugSystem.OnUpdate                -= UpdateView;
            ActiveDebugSystem.Sort.OnSortFilterChange -= SetEntitiesListDirty;
            ActiveDebugSystem.Entities.OnCreate       -= SetEntitiesListDirty;
            ActiveDebugSystem.Entities.OnDestroy      -= SetEntitiesListDirty;
            ActiveDebugSystem.Entities.OnDestroy      -= DisposeEntity;
            ActiveDebugSystem.OnDestroy               -= DisposeWorldView;
            
            DisposeActiveEntities();
         }

         ActiveDebugSystem = newActiveDebugSystem;
         if (ActiveDebugSystem == null) return;

         ActiveDebugSystem.OnUpdate                += UpdateView;
         ActiveDebugSystem.Sort.OnSortFilterChange += SetEntitiesListDirty;
         ActiveDebugSystem.Entities.OnCreate       += SetEntitiesListDirty;
         ActiveDebugSystem.Entities.OnDestroy      += SetEntitiesListDirty;
         ActiveDebugSystem.Entities.OnDestroy      += DisposeEntity;
         ActiveDebugSystem.OnDestroy               += DisposeWorldView;

         _entitiesList.Init(ActiveDebugSystem.Sort.SortedAliveEntities);
      }


      
      private void SetEntitiesListDirty() {
         _isEntitiesListDirty = true;
      }
      
      private void SetEntitiesListDirty(int e) {
         SetEntitiesListDirty();
      }
      
      
      
      private void DisposeEntity(int entity) {
         if (!_entityViews.TryGetValue(entity, out UIEntity uiView)) return;

         _entitiesContainer.Remove(uiView);
         _entityViews.Remove(entity);
      }
      
      private void DisposeWorldView(EcsWorldDebugSystem debugSystem) {
         _worldTabs.RemoveTab(debugSystem.NamedWorld);
      }
      
      
      private void Reset() {
         _worldTabs?.Reset();
         _filter?.Reset();

         if (_entitiesList != null) {
            _entitiesList.Reset();
            _entitiesList.OnSelectEntities -= OnSelectEntities;
         }

         DisposeActiveEntities();
      }
      
      
      private void DisposeActiveEntities() {
         if (_entityViews == null) return;
         UIEntity[] entitiesToDispose = _entityViews.Values.ToArray();

         foreach (UIEntity entityView in entitiesToDispose) 
            DisposeEntity(entityView.Entity);
         
         _entityViews.Clear();
      }
      
      
      #region OnPlayModeStateChanged

      private void OnEnable() {
         EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
         EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
      }
      private void OnDisable() {
         EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
      }

      private void OnPlayModeStateChanged(PlayModeStateChange state) {
         Reset();
      }

      #endregion
   }
}
