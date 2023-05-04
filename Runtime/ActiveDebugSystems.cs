using System;
using System.Collections.Generic;
using LeoECSLite.UnityIntegration.Extentions;
using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration {
  public static class ActiveDebugSystems {
    private static readonly Dictionary<string, EcsWorldDebugSystem>   _SystemByNames;
    private static readonly Dictionary<EcsWorld, EcsWorldDebugSystem> _SystemByWorlds;

    public static int Count => _SystemByNames.Count;


    static ActiveDebugSystems() {
      _SystemByNames  = new Dictionary<string, EcsWorldDebugSystem>();
      _SystemByWorlds = new Dictionary<EcsWorld, EcsWorldDebugSystem>();
    }



    public static void Register(EcsWorldDebugSystem system) {
      _SystemByNames.Add(system.DebugName(), system);
      _SystemByWorlds.Add(system.World, system);
    }

    public static void Unregister(EcsWorldDebugSystem system) {
      _SystemByNames.Remove(system.DebugName());
      _SystemByWorlds.Remove(system.World);
    }



    public static bool TryGet(string   worldName, out EcsWorldDebugSystem system) => _SystemByNames.TryGetValue(worldName.ToWorldDebugName(), out system);
    public static bool TryGet(EcsWorld world,     out EcsWorldDebugSystem system) => _SystemByWorlds.TryGetValue(world, out system);



    public static void Foreach(Action<EcsWorldDebugSystem> action) {
      foreach (EcsWorldDebugSystem system in _SystemByNames.Values)
        action?.Invoke(system);
    }
  }
}