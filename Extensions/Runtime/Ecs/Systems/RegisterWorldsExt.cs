using LeoECSLite.UnityAdapter.WorldsLocator;
using Leopotam.EcsLite;

namespace Extensions.Runtime.Ecs.Systems {
  public static class RegisterWorldsExt {
    public static IEcsSystems RegisterWorlds(this IEcsSystems systems) {
      EcsWorldsLocator.RegisterAllWorlds(systems);
      return systems;
    }
  }
}