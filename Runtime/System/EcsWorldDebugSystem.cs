using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;

namespace Mitfart.LeoECSLite.UnityIntegration.System{
   public class EcsWorldDebugSystem : IEcsPreInitSystem, IEcsRunSystem, IEcsWorldEventListener {
      public static readonly Dictionary<string, EcsWorldDebugSystem> ActiveSystems = new();

      private static int[]      _entitiesCache   = new int[32];
      private static object[]   _componentsCache = new object[32];
      private static IEcsPool[] _poolsCache      = new IEcsPool[32];
      
      public event Action<int> OnWorldResize;

      public event Action<EcsWorldDebugSystem> OnInit;
      public event Action<EcsWorldDebugSystem> OnUpdate;
      public event Action<EcsWorldDebugSystem> OnDestroy;


      public EWDSView     View     { get; }
      public EWDSEntities Entities { get; }
      public EWDSSort     Sort     { get; }

      public string             WorldName    { get; }
      public EntityNameSettings NameSettings { get; }
      public EcsWorld           World        { get; private set; }

      
      
      public EcsWorldDebugSystem(string worldName = null, EntityNameSettings nameSettings = default){
         WorldName    = worldName;
         NameSettings = nameSettings;

         View     = this.CreateView();
         Entities = new EWDSEntities(this);
         Sort     = new EWDSSort(this);
      }

      

      public void PreInit(IEcsSystems systems){
         InitWorld();
         InitEntities();

         ActiveSystems.Add(this.GetDebugName(), this);
         OnInit?.Invoke(this);
         

         void InitWorld(){
            World = systems.GetWorld(WorldName) ?? throw new Exception($"Cant find required world! ({WorldName})");
            World.AddEventListener(this);
         }

         void InitEntities() => ForeachEntity(OnEntityCreated);
      }
      
      public void Run(IEcsSystems systems){
         Entities.UpdateDirtyEntities();
         Sort.UpdateSortedEntities();
         OnUpdate?.Invoke(this);
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
      
      
      
      public void OnEntityCreated(int   entity) => Entities.OnEntityCreated(entity);
      public void OnEntityChanged(int   entity) => Entities.OnEntityChanged(entity);
      public void OnEntityDestroyed(int entity) => Entities.OnEntityDestroyed(entity);
      
      public void OnFilterCreated(EcsFilter filter) { }

      public void OnWorldResized(int newSize) {
         View.SetWorldSize(newSize);
         OnWorldResize?.Invoke(newSize);
      }
      
      public void OnWorldDestroyed(EcsWorld world) {
         OnDestroy?.Invoke(this);

         View.Destroy();
         World.RemoveEventListener(this);
         ActiveSystems.Remove(this.GetDebugName());
      }
   }
}
