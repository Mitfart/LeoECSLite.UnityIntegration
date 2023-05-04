using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration.Extentions {
  public static class IsEntityAliveExt {
    public static bool IsEntityAlive(this EcsWorld world, int entity) => world.GetEntityGen(entity) > 0;
  }
}