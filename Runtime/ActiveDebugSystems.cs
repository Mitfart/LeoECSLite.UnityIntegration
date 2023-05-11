using System;
using System.Collections.Generic;
using LeoECSLite.UnityIntegration.Extensions;
using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration {
  public static class ActiveDebugSystems {
    private static readonly Dictionary<string, EcsWorldDebugSystem>   _SystemsByNames;
    private static readonly Dictionary<EcsWorld, EcsWorldDebugSystem> _SystemsByWorlds;

    public static int Count => _SystemsByNames.Count;

    

    static ActiveDebugSystems() {
      _SystemsByNames  = new Dictionary<string, EcsWorldDebugSystem>();
      _SystemsByWorlds = new Dictionary<EcsWorld, EcsWorldDebugSystem>();
    }



    public static void Register(EcsWorldDebugSystem system) {
      _SystemsByNames.Add(system.DebugName, system);
      _SystemsByWorlds.Add(system.World, system);
    }

    public static void Unregister(EcsWorldDebugSystem system) {
      _SystemsByNames.Remove(system.DebugName);
      _SystemsByWorlds.Remove(system.World);
    }



    public static bool TryGet(string   worldName, out EcsWorldDebugSystem system) => _SystemsByNames.TryGetValue(worldName.ToWorldDebugName(), out system);
    public static bool TryGet(EcsWorld world,     out EcsWorldDebugSystem system) => _SystemsByWorlds.TryGetValue(world, out system);



    public static void Foreach(Action<EcsWorldDebugSystem> action) {
      foreach (EcsWorldDebugSystem system in _SystemsByNames.Values)
        action?.Invoke(system);
    }
  }
}