#if UNITY_EDITOR
using System;
using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration.Editor.Component {
  public class ComponentData<T> : ComponentData where T : struct {
    public T component;

    public EcsPool<T> TypedPool { get; private set; }
    public T          Value     => TypedPool.Get(Entity);

    public override EcsWorld World    { get; protected set; }
    public override int      Entity   { get; protected set; }
    public override IEcsPool Pool     => TypedPool;
    public override Type     Type     => typeof(T);
    public override object   RawValue => Pool.GetRaw(Entity);



    public override ComponentData Init(int entity, EcsWorld world) {
      if (entity == Entity && world == World)
        return this;

      Entity    = entity;
      World     = world;
      TypedPool = World.GetPool<T>();
      return this;
    }

    public override ComponentData RefreshValue() {
      component = TypedPool.Get(Entity);
      return this;
    }
  }
}
#endif