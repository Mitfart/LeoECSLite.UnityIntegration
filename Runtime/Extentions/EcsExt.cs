using System;
using System.Reflection;
using Leopotam.EcsLite;

namespace Mitfart.LeoECSLite.UnityIntegration.Extentions {
  public static class EcsExt {
    private static readonly MethodInfo Get_Pool_Method_Info = typeof(EcsWorld).GetMethod(nameof(EcsWorld.GetPool));
    private static readonly MethodInfo Filter_Method_Info   = typeof(EcsWorld).GetMethod(nameof(EcsWorld.Filter));

    private static readonly MethodInfo Exc_Method_Info =
      typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Exc));

    private static readonly MethodInfo Inc_Method_Info =
      typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Inc));


    public static IEcsPool GetPool(this EcsWorld world, Type type) {
      var pool = world.GetPoolByType(type);
      if (pool != null) return pool;

      var getPool = Get_Pool_Method_Info.MakeGenericMethod(type);
      pool = (IEcsPool)getPool.Invoke(world, null);

      return pool;
    }


    public static EcsWorld.Mask Filter(this EcsWorld world, Type type) {
      var getFilter = Filter_Method_Info.MakeGenericMethod(type);
      return (EcsWorld.Mask)getFilter.Invoke(world, null);
    }


    public static EcsWorld.Mask Inc(this EcsWorld.Mask mask, Type type) {
      var method = Inc_Method_Info.MakeGenericMethod(type);
      return (EcsWorld.Mask)method.Invoke(mask, null);
    }


    public static EcsWorld.Mask Exc(this EcsWorld.Mask mask, Type type) {
      var method = Exc_Method_Info.MakeGenericMethod(type);
      return (EcsWorld.Mask)method.Invoke(mask, null);
    }
  }
}
