#if UNITY_EDITOR
using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace LeoECSLite.UnityIntegration.Editor.Component {
  public abstract class ComponentData : ScriptableObject {
    public abstract EcsWorld World  { get; protected set; }
    public abstract int      Entity { get; protected set; }
    public abstract IEcsPool Pool   { get; }

    public abstract Type   Type     { get; }
    public abstract object RawValue { get; }


    public abstract ComponentData Init(int entity, EcsWorld world);
    public abstract ComponentData RefreshValue();
  }
}
#endif