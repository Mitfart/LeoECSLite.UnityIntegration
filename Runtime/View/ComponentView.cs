#if UNITY_EDITOR
using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.View {
  [Serializable]
  public class ComponentView {
    [HideInInspector]    public string componentName;
    [SerializeReference] public object component;

    public EntityView EntityView    { get; private set; }
    public IEcsPool   Pool          { get; private set; }
    public Type       ComponentType { get; private set; }

    public EcsWorld World  => EntityView.World;
    public int      Entity => EntityView.Entity;



    public ComponentView(EntityView entityView) {
      EntityView = entityView;
    }

    public ComponentView Init(Type componentType) {
      if (ComponentType == componentType)
        return this;

      ComponentType = componentType;
      componentName = componentType.Name;
      Pool          = World.GetPoolByType(componentType);
      return this;
    }


    public void Refresh()              => component = Pool.GetRaw(Entity);
    public void SetValue()             => Pool.SetRaw(Entity, component);
    public void SetValue(object value) => Pool.SetRaw(Entity, component = value);
  }
}
#endif