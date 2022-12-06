using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public class EcsDebugWindow : EditorWindow{
      private readonly Dictionary<int, EntityView> _activeEntitiesViews = new();
      
      private Toolbar         _toolbar;
      private StringsDropdown _worldsEnum;
      
      private SplitView  _content;
      private ScrollView _entitiesContainer;
      private ListView   _entitiesList;
      private bool       _isEntitiesListDirty;

      private EcsWorldDebugSystem _activeSystem;
      


      private void CreateGUI(){
         CreateElements();
         AddElements();

         if (!Application.isPlaying) return;
         if (EcsWorldDebugSystem.ActiveSystems.Count <= 0) return;

         SetActiveWorldDebugSystem(EcsWorldDebugSystem.ActiveSystems.Values.First());
         InitElements();
      }
      
      private void CreateElements(){
         _toolbar    = new Toolbar();
         _worldsEnum = new StringsDropdown();

         _content           = new SplitView();
         _entitiesList      = new ListView();
         _entitiesContainer = new ScrollView();
      }
      private void AddElements(){
         _content.Left.Add(_entitiesList);
         _content.Right.Add(_entitiesContainer);

         rootVisualElement
           .AddChild(_toolbar
                       .AddChild(_worldsEnum))
           .AddChild(_content);
      }
      private void InitElements(){
         InitEntitiesList();
         InitWorldsEnum();
      }
      


      private void InitEntitiesList(){
         _entitiesList.itemsSource = _activeSystem.AliveEntities;
         
         _entitiesList.bindItem          =  BindItem;
         _entitiesList.makeItem          =  MakeItem;
         _entitiesList.onSelectionChange += OnSelectionChange;
         
         _entitiesList.showFoldoutHeader       = true;
         _entitiesList.showBoundCollectionSize = true;



         void BindItem(VisualElement element, int e){
            if (!_activeSystem.TryGetEntityView(_activeSystem.AliveEntities[e], out var entityView)) return;

            ((Label)element).text = entityView.name;
         }

         VisualElement MakeItem(){
            return new Label{ style ={ unityTextAlign = TextAnchor.MiddleLeft } };
         }

         void OnSelectionChange(IEnumerable<object> objects){
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
               _activeEntitiesViews.First().Value.IsExpanded = true;
         }
      }
      private void SetEntitiesListDirty(int e){
         _isEntitiesListDirty = true;
      }

      
      private void InitWorldsEnum(){
         foreach (var debugWorldName in EcsWorldDebugSystem.ActiveSystems.Keys)
            _worldsEnum.Add(debugWorldName);
         _worldsEnum.index = 0;

         _worldsEnum.OnChangeValue += ChangeWorld;
      }
      private void ChangeWorld(string newValue, string oldValue){
         if (EcsWorldDebugSystem.ActiveSystems.TryGetValue(newValue, out var debugSystem))
            SetActiveWorldDebugSystem(debugSystem);
      }


      // =========================================================
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
      // =========================================================



      private void SetActiveWorldDebugSystem(EcsWorldDebugSystem newActiveDebugSystem){
         if (_activeSystem == newActiveDebugSystem) return;

         if (_activeSystem != null){
            _activeSystem.OnUpdate        -= UpdateView;
            _activeSystem.OnEntityCreate  -= SetEntitiesListDirty;
            _activeSystem.OnEntityDespose -= SetEntitiesListDirty;
            _activeSystem.OnEntityDespose -= DisposeEntityView;
            _activeSystem.OnDestroy       -= DisposeWorldView;
            ResetInspector();
         }

         _activeSystem = newActiveDebugSystem;
         if (_activeSystem == null) return;

         _activeSystem.OnUpdate        += UpdateView;
         _activeSystem.OnEntityCreate  += SetEntitiesListDirty;
         _activeSystem.OnEntityDespose += SetEntitiesListDirty;
         _activeSystem.OnEntityDespose += DisposeEntityView;
         _activeSystem.OnDestroy       += DisposeWorldView;

         InitEntitiesList();
      }
      private void DisposeWorldView(EcsWorldDebugSystem debugSystem){
         var debugName = debugSystem.GetDebugName();
         
         if (_worldsEnum.value == debugName)
            ResetInspector();
         
         _worldsEnum.Remove(debugName);
      }
      

      private EntityView CreateEntityView(int entity){
         return !_activeSystem.TryGetEntityView(entity, out var view) 
                   ? null 
                   : new EntityView().Init(view);
      }
      private void DisposeEntityView(int entity){
         if (!_activeEntitiesViews.TryGetValue(entity, out var view)) return;
         view.Dispose();
      }



      private void Reset(){
         ResetInspector();
         ResetToolbar();
      }
      private void ResetToolbar(){
         if (_worldsEnum == null) return;

         _worldsEnum.Clear();
         _worldsEnum.OnChangeValue -= ChangeWorld;
      }
      private void ResetInspector(){
         foreach (var entityView in _activeEntitiesViews.Values) 
            entityView.Dispose();
         _activeEntitiesViews.Clear();
         
         _entitiesContainer?.Clear();
         _entitiesList?.Clear();
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
            case PlayModeStateChange.EnteredEditMode: break;
            case PlayModeStateChange.ExitingEditMode: break;
            case PlayModeStateChange.EnteredPlayMode: break;
            case PlayModeStateChange.ExitingPlayMode:
               Reset();
               break;
            default:
               throw new ArgumentOutOfRangeException(nameof(state), state, null);
         }
      }

      #endregion


      #region Open

      private static EcsDebugWindow _window;

      [MenuItem(MenuPath.DEBUG_WINDOW)]
      public static void Open(){
         _window = GetWindow<EcsDebugWindow>(nameof(EcsDebugWindow));
         _window.Show();
      }

      #endregion
   }
}
