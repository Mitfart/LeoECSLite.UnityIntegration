using System.Reflection;
using Leopotam.EcsLite;

namespace Extensions.Runtime.Ecs.Filter {
  public static class ExcExt {
    private static readonly MethodInfo _Filter_Exc_Method_Info = typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Exc));


    public static EcsWorld.Mask Exc(this EcsWorld.Mask mask, System.Type type) {
      MethodInfo method = _Filter_Exc_Method_Info.MakeGenericMethod(type);
      return (EcsWorld.Mask) method.Invoke(mask, null);
    }
  }
}