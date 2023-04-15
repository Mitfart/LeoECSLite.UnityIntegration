#if UNITY_EDITOR
using System;
using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration.Editor.Component {
  public class ComponentData<T> : ComponentData where T : struct {
    public T component;

    private EcsPool<T> _typedPool;

    public override IEcsPool Pool => _typedPool;
    public override Type     Type => typeof(T);

    private void OnValidate() {
      Pool.SetRaw(Entity, component);
    }



    protected override void OnInit() {
      _typedPool = World.GetPool<T>();
    }

    protected override void OnRefresh() {
      component = _typedPool.Get(Entity);
    }
  }
}
#endif