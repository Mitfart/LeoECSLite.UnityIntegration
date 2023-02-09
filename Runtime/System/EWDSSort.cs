using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;

namespace Mitfart.LeoECSLite.UnityIntegration.System {
  public class EWDSSort {
    private EcsWorldDebugSystem _debugSystem;
    
    public List<int>  SortedAliveEntities { get; }
    public List<Type> ComponentTypes      { get; }
    public EcsFilter  Filter              { get; private set; }
    
    public event Action OnSortFilterChange;


    
    public EWDSSort(EcsWorldDebugSystem debugSystem) {
      _debugSystem        = debugSystem;
      ComponentTypes      = new List<Type>();
      SortedAliveEntities = new List<int>(_debugSystem.World.GetWorldSize());
    }
    
    
    public void UpdateSortedEntities() {
      SortedAliveEntities.Clear();

      if (Filter == null){
        foreach (var e in _debugSystem.Entities.Alive)
          SortedAliveEntities.Add(e);
        return;
      }
         
      foreach (var e in Filter)
        SortedAliveEntities.Add(e);
    }
    
    
    
    public void AddSortSortComponent(Type type) {
      ComponentTypes.Add(type);
      UpdateSortFilter();
    }
      
    public void RemoveSortSortComponent(Type type) {
      ComponentTypes.Remove(type);
      UpdateSortFilter();
    }
      
    private void UpdateSortFilter() {
      if (ComponentTypes.Count <= 0){
        Filter = null;
        OnSortFilterChange?.Invoke();
        return;
      }
         
      var sortMask = _debugSystem.World.Filter(ComponentTypes.First());
      for (var i = 1; i < ComponentTypes.Count; i++)
        sortMask.Inc(ComponentTypes[i]);
      Filter = sortMask.End();
         
      OnSortFilterChange?.Invoke();
    }
  }
}
