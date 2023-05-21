using System.Reflection;
using Leopotam.EcsLite;

namespace Extensions.Runtime.Ecs.World {
  public static class FilterExt {
    private static readonly MethodInfo _Filter_Method_Info = typeof(EcsWorld).GetMethod(nameof(EcsWorld.Filter));


    public static EcsWorld.Mask Filter(this EcsWorld world, System.Type type) {
      MethodInfo getFilter = _Filter_Method_Info.MakeGenericMethod(type);
      return (EcsWorld.Mask) getFilter.Invoke(world, null);
    }
  }
}