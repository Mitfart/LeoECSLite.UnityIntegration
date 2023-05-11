#if UNITY_EDITOR
using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace LeoECSLite.UnityIntegration.Editor.Component {
  public class ComponentView {
    [field: SerializeField] public object RawValue { get; protected set; }

    public int      Entity        { get; }
    public IEcsPool Pool          { get; }
    public Type     ComponentType { get; }



    public ComponentView(Type component, int entity, EcsWorld world) {
      Entity        = entity;
      Pool          = world.GetPoolByType(component);
      ComponentType = component;
    }

    public void Refresh() => RawValue = Pool.GetRaw(Entity);
  }
}
#endif