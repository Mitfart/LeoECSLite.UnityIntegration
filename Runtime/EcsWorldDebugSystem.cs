#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;

namespace Mitfart.LeoECSLite.UnityIntegration{
    public class EcsWorldDebugSystem : IEcsPreInitSystem, IEcsRunSystem, IEcsWorldEventListener {
        public static readonly Dictionary<string, EcsWorldDebugSystem> ActiveSystems = new();

        private static int[]      ENTITIES_CACHE   = new int[32];
        private static object[]   COMPONENTS_CACHE = new object[32];
        private static IEcsPool[] POOLS_CACHE      = new IEcsPool[32];
      
        public event Action<int> OnWorldResize;

        public event Action<EcsWorldDebugSystem> OnInit;
        public event Action<EcsWorldDebugSystem> OnUpdate;
        public event Action<EcsWorldDebugSystem> OnDestroy;


        public EwdsView     View     { get; set; }
        public EwdsEntities Entities { get; set; }
        public EwdsSort     Sort     { get; set; }

        public string             WorldName    { get; }
        public EntityNameSettings NameSettings { get; }
        public EcsWorld           World        { get; private set; }

      
      
        public EcsWorldDebugSystem(string worldName = null, EntityNameSettings nameSettings = default){
            WorldName    = worldName;
            NameSettings = nameSettings;
        }

      

        public void PreInit(IEcsSystems systems) {
            InitWorld();
         
            View     = this.CreateView();
            Entities = new EwdsEntities(this);
            Sort     = new EwdsSort(this);
         
            InitEntities();
         
         
            ActiveSystems.Add(this.GetDebugName(), this);
            OnInit?.Invoke(this);

            void InitWorld() {
                World = systems.GetWorld(WorldName) ?? throw new Exception($"Cant find required world! ({WorldName})");
                World.AddEventListener(this);
            }

            void InitEntities() => ForeachEntity(OnEntityCreated);
        }
      
        public void Run(IEcsSystems systems) {
            Entities.UpdateDirtyEntities();
            Sort.UpdateSortedEntities();
            OnUpdate?.Invoke(this);
        }
      
      

        public int ForeachEntity(Action<int> action) {
            int count = World.GetAllEntities(ref ENTITIES_CACHE);
            for (var i = 0; i < count; i++) action.Invoke(ENTITIES_CACHE[i]);
            return count;
        }
      
        public int ForeachComponent(int entity, Action<object> action) {
            int count = World.GetComponents(entity, ref COMPONENTS_CACHE);
            for (var i = 0; i < count; i++) action.Invoke(COMPONENTS_CACHE[i]);
            return count;
        }
      
        public int ForeachPool(Action<IEcsPool> action) {
            int count = World.GetAllPools(ref POOLS_CACHE);
            for (var i = 0; i < count; i++) action.Invoke(POOLS_CACHE[i]);
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

#endif