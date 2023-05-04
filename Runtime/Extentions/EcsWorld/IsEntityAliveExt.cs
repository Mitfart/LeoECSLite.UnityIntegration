namespace LeoECSLite.UnityIntegration.Extentions.EcsWorld {
  public static class IsEntityAliveExt {
    public static bool IsEntityAlive(this Leopotam.EcsLite.EcsWorld world, int entity) => world.GetEntityGen(entity) > 0;
  }
}