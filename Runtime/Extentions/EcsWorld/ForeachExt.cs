﻿using System;
using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration.Extentions.EcsWorld {
  public static class ForeachExt {
    public static void ForeachEntity(this Leopotam.EcsLite.EcsWorld world, Action<int> action) {
      int[] entities = StaticCache.Entities;
      int   count    = world.GetAllEntities(ref entities);

      for (var i = 0; i < count; i++)
        action.Invoke(entities[i]);
    }


    public static void ForeachComponent(this Leopotam.EcsLite.EcsWorld world, int entity, Action<object> action) {
      object[] components = StaticCache.Components;
      int      count      = world.GetComponents(entity, ref components);

      for (var i = 0; i < count; i++)
        action.Invoke(components[i]);
    }


    public static void ForeachComponentType(
      this Leopotam.EcsLite.EcsWorld world,
      int                            entity,
      Action<System.Type>            action
    ) {
      System.Type[] types = StaticCache.Types;
      int           count = world.GetComponentTypes(entity, ref types);

      for (var i = 0; i < count; i++)
        action.Invoke(types[i]);
    }


    public static void ForeachPool(this Leopotam.EcsLite.EcsWorld world, Action<IEcsPool> action) {
      IEcsPool[] pools = StaticCache.Pools;
      int        count = world.GetAllPools(ref pools);

      for (var i = 0; i < count; i++)
        action.Invoke(pools[i]);
    }
  }
}