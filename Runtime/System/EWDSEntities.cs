using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;

namespace Mitfart.LeoECSLite.UnityIntegration.System {
  public class EWDSEntities : IEcsWorldEventListener {
    private EcsWorldDebugSystem _debugSystem;
    
    public HashSet<int> Dirty { get; }
    public HashSet<int> Alive { get; }
    
    public event Action<int> OnCreate;
    public event Action<int> OnChange; // ????????????
    public event Action<int> OnDestroy;

    
    
    public EWDSEntities(EcsWorldDebugSystem debugSystem) {
      _debugSystem = debugSystem;

      var size = _debugSystem.GetWorldSize();
      Dirty = new HashSet<int>(size);
      Alive = new HashSet<int>(size);
    }



    public void UpdateDirtyEntities() {
      foreach (var entity in Dirty)
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
      if (_debugSystem.View.TryGetEntityView(entity, out var view)) 
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
