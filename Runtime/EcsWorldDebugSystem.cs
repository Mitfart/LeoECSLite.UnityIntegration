using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public class EcsWorldDebugSystem : IEcsPreInitSystem, IEcsRunSystem, IEcsWorldEventListener{
      public static readonly Dictionary<string, EcsWorldDebugSystem> ActiveSystems = new();

      private static int[]      _entitiesCache   = new int[32];
      private static object[]   _componentsCache = new object[32];
      private static IEcsPool[] _poolsCache      = new IEcsPool[32];
      
      private GameObject       _rootGo;
      private HashSet<int>     _dirtyEntities;
      private MonoEntityView[] _monoEntityViews;

      public string                              WorldName    { get; }
      public MonoEntityView.NameBuilder.Settings NameSettings { get; }
      public EcsWorld                            World        { get; private set; }
      public List<int>                           AliveEntities{ get; private set; }

      
      
      public EcsWorldDebugSystem(string worldName = null, MonoEntityView.NameBuilder.Settings nameSettings = default){
         WorldName    = worldName;
         NameSettings = nameSettings;
      }

      

      public void PreInit(IEcsSystems systems){
         InitWorld();
         CreateDebugObject();
         InitEntities();

         ActiveSystems.Add(GetDebugName(), this);
         OnInit?.Invoke(this);

         void InitWorld(){
            World = systems.GetWorld(WorldName) ?? throw new Exception($"Cant find required world! ({WorldName})");
            World.AddEventListener(this);
         }

         void CreateDebugObject(){
            _rootGo = new GameObject(GetDebugName()){ hideFlags = HideFlags.NotEditable };
            Object.DontDestroyOnLoad(_rootGo);
         }

         void InitEntities(){
            var size = World.GetWorldSize();
            _monoEntityViews = new MonoEntityView[size];
            _dirtyEntities   = new HashSet<int>(size);
            AliveEntities    = new List<int>(size);
            ForeachEntity(OnEntityCreated);
         }
      }
      
      public void Run(IEcsSystems systems){
         foreach (var entity in _dirtyEntities){
            if (!TryGetEntityView(entity, out var view)) continue;
            
            view.UpdateComponents();
            view.UpdateName();

            OnEntityChange?.Invoke(entity);
         }

         _dirtyEntities.Clear();

         OnUpdate?.Invoke(this);
      }



      public bool TryGetEntityView(int entity, out MonoEntityView view){
         if (entity < 0 || entity >= _monoEntityViews.Length){
            view = null;
            return false;
         }
         view = _monoEntityViews[entity];
         return view != null;
      }
      private MonoEntityView CreateEntityView(int entity){
         var viewObject = new GameObject();
         viewObject.transform.SetParent(_rootGo.transform, false);

         var view = viewObject.AddComponent<MonoEntityView>();
         view.Init(this, entity);

         return _monoEntityViews[entity] = view;
      }

      

      public int ForeachEntity(Action<int> action){
         var count = World.GetAllEntities(ref _entitiesCache);
         for (var i = 0; i < count; i++) action.Invoke(_entitiesCache[i]);
         return count;
      }
      public int ForeachComponent(int entity, Action<object> action){
         var count = World.GetComponents(entity, ref _componentsCache);
         for (var i = 0; i < count; i++) action.Invoke(_componentsCache[i]);
         return count;
      }
      public int ForeachPool(Action<IEcsPool> action){
         var count = World.GetAllPools(ref _poolsCache);
         for (var i = 0; i < count; i++) action.Invoke(_poolsCache[i]);
         return count;
      }

      

      public string GetDebugName(){
         return !string.IsNullOrWhiteSpace(WorldName) ? $"[ECS-WORLD {WorldName}]" : "[ECS-WORLD]";
      }

      

      #region Events

      public event Action<int> OnEntityCreate;
      public event Action<int> OnEntityChange;
      public event Action<int> OnEntityDespose;

      public event Action<int> OnWorldResize;

      public event Action<EcsWorldDebugSystem> OnInit;
      public event Action<EcsWorldDebugSystem> OnUpdate;
      public event Action<EcsWorldDebugSystem> OnDestroy;

      #endregion



      #region EcsWorldEvents

      public void OnEntityCreated(int entity){
         if (!TryGetEntityView(entity, out var view)) 
            view = CreateEntityView(entity);
         view.Activate();

         _dirtyEntities.Add(entity);
         AliveEntities.Add(entity);
         OnEntityCreate?.Invoke(entity);
      }
      public void OnEntityChanged(int entity){
         _dirtyEntities.Add(entity);
      }
      public void OnEntityDestroyed(int entity){
         if (TryGetEntityView(entity, out var view)) 
            view.Deactivate();

         _dirtyEntities.Remove(entity);
         AliveEntities.Remove(entity);
         OnEntityDespose?.Invoke(entity);
      }


      public void OnFilterCreated(EcsFilter filter){}
      public void OnWorldResized(int newSize){
         Array.Resize(ref _monoEntityViews, newSize);
         OnWorldResize?.Invoke(newSize);
      }


      public void OnWorldDestroyed(EcsWorld world){
         OnDestroy?.Invoke(this);

         ActiveSystems.Remove(GetDebugName());
         World.RemoveEventListener(this);

         Object.Destroy(_rootGo);
      }

      #endregion
   }
}