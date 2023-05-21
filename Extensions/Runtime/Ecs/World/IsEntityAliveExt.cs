using Leopotam.EcsLite;

namespace Extensions.Runtime.Ecs.World {
  public static class IsEntityAliveExt {
    public static bool IsEntityAlive(this EcsWorld world, int entity) => world.GetEntityGen(entity) > 0;
  }
}