#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;

namespace Mitfart.LeoECSLite.UnityIntegration {
    public class EwdsEntities : IEcsWorldEventListener {
        private EcsWorldDebugSystem _debugSystem;
    
        public HashSet<int> Dirty { get; }
        public HashSet<int> Alive { get; }
    
        public event Action<int> OnCreate;
        public event Action<int> OnChange; // ????????????
        public event Action<int> OnDestroy;

    
    
        public EwdsEntities(EcsWorldDebugSystem debugSystem) {
            _debugSystem = debugSystem;

            int size = _debugSystem.GetWorldSize();
            Dirty = new HashSet<int>(size);
            Alive = new HashSet<int>(size);
        }



        public void UpdateDirtyEntities() {
            foreach (int entity in Dirty)
                _debugSystem.View.UpdateEntity(entity);
      
            Dirty.Clear();
        }
    
    
    
        public void OnEntityCreated(int entity) {
            _debugSystem.View.GetOrCreate(entity).Activate();

            Dirty.Add(entity);
            Alive.Add(entity);
      
            OnCreate?.Invoke(entity);
        }
    
        public void OnEntityChanged(int entity) {
            Dirty.Add(entity);
      
            // OnChange?.Invoke(entity); 
        }
    
        public void OnEntityDestroyed(int entity) {
            if (_debugSystem.View.TryGetEntityView(entity, out MonoEntityView view)) 
                view.Deactivate();

            Dirty.Remove(entity);
            Alive.Remove(entity);
      
            OnDestroy?.Invoke(entity);
        }
        public void OnFilterCreated(EcsFilter filter)  { }
        public void OnWorldResized(int        newSize) { }
        public void OnWorldDestroyed(EcsWorld world)   { }
    }
}

#endif