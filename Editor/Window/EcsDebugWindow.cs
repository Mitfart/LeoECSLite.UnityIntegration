using System;
using System.Collections.Generic;
using System.Linq;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Nav;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Sort;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window{
   public class EcsDebugWindow : EditorWindow{
      private readonly Dictionary<int, Elements.Entity.EntityView> _activeEntitiesViews = new();
      
      private SplitView  _content;
      private ScrollView _entitiesContainer;
      private ListView   _entitiesList;
      private bool       _isEntitiesListDirty;

      private SortTagsContainerView _sortTagsContainer;
      private StringsDropdown       _worldsEnum;
      
      public EcsWorldDebugSystem ActiveSystem { get; private set; }



      private void CreateGUI(){
         CreateElements();
         AddElements();

         if (!Application.isPlaying) return;
         if (EcsWorldDebugSystem.ActiveSystems.Count <= 0) return;

         InitElements();
      }
      
      
      private void CreateElements(){
         _sortTagsContainer = new SortTagsContainerView(this);
         _worldsEnum        = new StringsDropdown();

         _content           = new SplitView();
         _entitiesList      = new ListView();
         _entitiesContainer = new ScrollView();
      }
      
      private void AddElements(){
         rootVisualElement
           .AddChild(_sortTagsContainer)
           .AddChild(_content);
         
         _content
           .left
           .AddChild(_worldsEnum)
           .AddChild(_entitiesList);
         _content
           .right
           .AddChild(_entitiesContainer);
      }
      
      private void InitElements(){
         InitWorldsEnum();
         InitEntitiesList();
      }
      
      
      private void InitWorldsEnum(){
         foreach (var systemKey in EcsWorldDebugSystem.ActiveSystems.Keys)
            _worldsEnum.Add(systemKey);

         _worldsEnum.index    =  0;
         _worldsEnum.OnChoose += ChangeWorld;

         ChangeWorld(_worldsEnum.value, null);
      }
      
      private void InitEntitiesList() {
         _entitiesList.itemsSource = ActiveSystem.Sort.SortedAliveEntities;
         
         _entitiesList.bindItem = BindItem;
         _entitiesList.makeItem = MakeItem;
#if UNITY_2022
         _entitiesList.selectionChanged -= SelectionChange;
         _entitiesList.selectionChanged += SelectionChange;
#else
         _entitiesList.onSelectionChange -= SelectionChange;
         _entitiesList.onSelectionChange += SelectionChange;
#endif
         
         _entitiesList.showFoldoutHeader       = true;
         _entitiesList.showBoundCollectionSize = true;
         _entitiesList.ClearSelection();
         _entitiesList.Rebuild();



         void BindItem(VisualElement element, int e){
            if (!ActiveSystem.View.TryGetEntityView(ActiveSystem.Sort.SortedAliveEntities[e], out var entityView)) return;

            ((Label) element).text = entityView.name;
         }

         VisualElement MakeItem() {
            return new Label{ style ={ unityTextAlign = TextAnchor.MiddleLeft } };
         }

         void SelectionChange(IEnumerable<object> objects) {
            var entities = objects as object[] ?? objects.ToArray();

            foreach (var entityView in _activeEntitiesViews.Values)
               entityView.Dispose();
            _activeEntitiesViews.Clear();
            

            foreach (int entity in entities){
               var entityView = CreateEntityView(entity);

               _activeEntitiesViews[entity] = entityView;
               _entitiesContainer.Add(entityView);
            }

            if (entities.Length == 1)
               _activeEntitiesViews[(int) entities[0]].IsExpanded = true;
         }
      }
      
      
      private void ChangeWorld(string newValue, string oldValue){
         if (EcsWorldDebugSystem.ActiveSystems.TryGetValue(newValue, out var debugSystem))
            SetActiveWorldDebugSystem(debugSystem);
         else throw new Exception($"Can't find System relative to `{newValue}` world!");
      }
      

      
      
      private void UpdateView(EcsWorldDebugSystem system){
         foreach (var view in _activeEntitiesViews.Values){
            if (view.IsExpanded){
               view.MonoView.UpdateComponentsValues();
               view.MarkDirtyRepaint();
            }
            view.Label = view.MonoView.name;
         }
      }

      private void OnInspectorUpdate(){
         if (!_isEntitiesListDirty) return;

         _entitiesList.RefreshItems();
         _isEntitiesListDirty = false;
      }
      


      private void SetActiveWorldDebugSystem(EcsWorldDebugSystem newActiveDebugSystem){
         if (ActiveSystem == newActiveDebugSystem) return;

         if (ActiveSystem != null){
            ActiveSystem.OnUpdate                -= UpdateView;
            ActiveSystem.Sort.OnSortFilterChange -= SetEntitiesListDirty;
            ActiveSystem.Entities.OnCreate       -= SetEntitiesListDirty;
            ActiveSystem.Entities.OnDestroy      -= SetEntitiesListDirty;
            ActiveSystem.Entities.OnDestroy      -= DestroyEntityView;
            ActiveSystem.OnDestroy               -= DisposeWorldView;
            
            _sortTagsContainer.OnAddSortTag    -= ActiveSystem.Sort.AddSortSortComponent;
            _sortTagsContainer.OnRemoveSortTag -= ActiveSystem.Sort.RemoveSortSortComponent;
            ClearActiveEntities();
         }

         ActiveSystem = newActiveDebugSystem;
         if (ActiveSystem == null) return;

         ActiveSystem.OnUpdate                += UpdateView;
         ActiveSystem.Sort.OnSortFilterChange += SetEntitiesListDirty;
         ActiveSystem.Entities.OnCreate       += SetEntitiesListDirty;
         ActiveSystem.Entities.OnDestroy      += SetEntitiesListDirty;
         ActiveSystem.Entities.OnDestroy      += DestroyEntityView;
         ActiveSystem.OnDestroy               += DisposeWorldView;
         
         _sortTagsContainer.OnAddSortTag    += ActiveSystem.Sort.AddSortSortComponent;
         _sortTagsContainer.OnRemoveSortTag += ActiveSystem.Sort.RemoveSortSortComponent;

         InitEntitiesList();
      }
      
      private void DisposeWorldView(EcsWorldDebugSystem debugSystem){
         var debugName = debugSystem.GetDebugName();
         
         if (_worldsEnum.value == debugName)
            ClearActiveEntities();
         
         _worldsEnum.Remove(debugName);
      }
      

      
      private Elements.Entity.EntityView CreateEntityView(int entity) {
         return !ActiveSystem.View.TryGetEntityView(entity, out var view) 
                   ? null 
                   : new Elements.Entity.EntityView().Init(view);
      }
      
      private void SetEntitiesListDirty(int e) => SetEntitiesListDirty();
      private void SetEntitiesListDirty()      => _isEntitiesListDirty = true;

      private void DestroyEntityView(int entity) {
         if (_activeEntitiesViews.TryGetValue(entity, out var view)) 
            view.Dispose();
      }



      private void Clear(){
         ClearActiveEntities();
         _worldsEnum?.Clear();
         _entitiesContainer?.Clear();
         ClearEntitiesList();
      }
      
      
      private void ClearActiveEntities(){
         if (_activeEntitiesViews == null) return;
         
         foreach (var entityView in _activeEntitiesViews.Values) 
            entityView.Dispose();
         
         _activeEntitiesViews.Clear();
      }

      private void ClearEntitiesList() {
         if (_entitiesList == null) return;

         _entitiesList.Clear();
         _entitiesList.itemsSource = Enumerable.Empty<Elements.Entity.EntityView>().ToList();
         _entitiesList.RefreshItems();
      }

      

      #region OnPlayModeStateChanged

      private void OnEnable(){
         EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
         EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
      }
      private void OnDisable(){
         EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
      }

      private void OnPlayModeStateChanged(PlayModeStateChange state){
         switch (state){
            case PlayModeStateChange.ExitingEditMode: break;
            case PlayModeStateChange.EnteredPlayMode: break;
            case PlayModeStateChange.EnteredEditMode:
            case PlayModeStateChange.ExitingPlayMode:
               Clear();
               break;
            default:
               throw new ArgumentOutOfRangeException(nameof(state), state, null);
         }
      }

      #endregion
   }
}
