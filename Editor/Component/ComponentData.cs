#if UNITY_EDITOR
using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace LeoECSLite.UnityIntegration.Editor.Component {
  public abstract class ComponentData : ScriptableObject {
    public EcsWorld World  { get; private set; }
    public int      Entity { get; private set; }

    public abstract IEcsPool Pool     { get; }
    public abstract Type     Type     { get; }

    public object RawValue => Pool.GetRaw(Entity);


    
    public ComponentData Init(int entity, EcsWorld world) {
      if (entity == Entity && world == World)
        return this;

      Entity = entity;
      World  = world;
      OnInit();
      return this;
    }

    public ComponentData Refresh() {
      OnRefresh();
      return this;
    }
    
    
    protected abstract void OnInit();
    protected abstract void OnRefresh();
  }
}
#endif