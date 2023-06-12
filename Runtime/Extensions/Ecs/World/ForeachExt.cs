using System;
using Leopotam.EcsLite;

namespace Mitfart.LeoECSLite.UnityIntegration.Extensions {
  public static class ForeachExt {
    public static void ForeachEntity(this EcsWorld world, Action<int> action) {
      int[] entities = StaticCache.Entities;
      int   count    = world.GetAllEntities(ref entities);

      for (var i = 0; i < count; i++)
        action.Invoke(entities[i]);
    }


    public static void ForeachComponent(this EcsWorld world, int entity, Action<object> action) {
      object[] components = StaticCache.Components;
      int      count      = world.GetComponents(entity, ref components);

      for (var i = 0; i < count; i++)
        action.Invoke(components[i]);
    }


    public static void ForeachComponentType(
      this EcsWorld world,
      int           entity,
      Action<Type>  action
    ) {
      Type[] types = StaticCache.Types;
      int    count = world.GetComponentTypes(entity, ref types);

      for (var i = 0; i < count; i++)
        action.Invoke(types[i]);
    }


    public static void ForeachPool(this EcsWorld world, Action<IEcsPool> action) {
      IEcsPool[] pools = StaticCache.Pools;
      int        count = world.GetAllPools(ref pools);

      for (var i = 0; i < count; i++)
        action.Invoke(pools[i]);
    }
  }
}